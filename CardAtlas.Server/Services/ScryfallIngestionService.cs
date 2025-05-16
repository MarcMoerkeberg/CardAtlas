using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using CardAtlas.Server.Services.Interfaces;
using ScryfallApi;
using ScryfallApi.Models.Types;
using ApiCard = ScryfallApi.Models.Card;
using ApiSet = ScryfallApi.Models.Set;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Services;

public class ScryfallIngestionService : IScryfallIngestionService
{
	private readonly IArtistRepository _artistRepository;
	private readonly ICardImageRepository _cardImageRepository;
	private readonly ICardRepository _cardRepository;
	private readonly IEqualityComparer<Keyword> _keywordComparer;
	private readonly IGameRepository _gameRepository;
	private readonly IEqualityComparer<Set> _setComparer;
	private readonly IScryfallApi _scryfallApi;
	private readonly ISetRepository _setRepository;

	//Batching data
	private Dictionary<Guid, Set> _setLookup = new();
	private HashSet<Card> _cardBatch = new();
	private Dictionary<string, List<CardImage>> _imageBatch = new();
	private Dictionary<string, Artist> _cardArtistBatch = new();
	private HashSet<Artist> _artistBatch = new();
	private Dictionary<Guid, List<CardPrice>> _cardPriceBatch = new();
	private Dictionary<Guid, List<CardGameTypeAvailability>> _cardAvailabilityBatch = new();
	private Dictionary<Guid, List<CardPrintFinish>> _printFinishBatch = new();
	private HashSet<GameFormat> _gameFormatsBatch = new();
	private Dictionary<Guid, List<CardLegality>> _cardLegalitiesBatch = new();
	private HashSet<Keyword> _keywordsBatch = new();
	private Dictionary<Guid, List<CardKeyword>> _cardKeywordsBatch = new();
	private HashSet<PromoType> _promoTypesBatch = new();
	private Dictionary<Guid, List<CardPromoType>> _cardPromotypesBatch = new();

	public ScryfallIngestionService(
		IArtistRepository artistRepository,
		ICardImageRepository cardImageRepository,
		ICardRepository cardRepository,
		IEqualityComparer<Keyword> keywordComparer,
		IGameRepository gameRepository,
		IEqualityComparer<Set> setComparer,
		IScryfallApi scryfallApi,
		ISetRepository setRepository)
	{
		_artistRepository = artistRepository;
		_cardImageRepository = cardImageRepository;
		_cardRepository = cardRepository;
		_keywordComparer = keywordComparer;
		_gameRepository = gameRepository;
		_setComparer = setComparer;
		_scryfallApi = scryfallApi;
		_setRepository = setRepository;
	}

	public async Task<int> UpsertSets()
	{
		UpsertContainer<Set> upsertionData = new();
		IEnumerable<ApiSet> apiSets = await _scryfallApi.GetSets();
		IEnumerable<Set> existingSets = await _setRepository.GetFromScryfallIds(apiSets.Select(set => set.Id));
		Dictionary<Guid, Set> existingSetsByScryfallId = existingSets
			.Where(set => set.ScryfallId.HasValue)
			.ToDictionary(set => set.ScryfallId!.Value);

		foreach (ApiSet apiSet in apiSets)
		{
			Set mappedSet = SetMapper.MapSet(apiSet);

			if (existingSetsByScryfallId.TryGetValue(apiSet.Id, out Set? existingSet))
			{
				mappedSet.Id = existingSet.Id;
				if (_setComparer.Equals(existingSet, mappedSet)) continue;

				SetMapper.MergeProperties(existingSet, mappedSet);
				upsertionData.ToUpdate.Add(mappedSet);
			}
			else
			{
				upsertionData.ToInsert.Add(mappedSet);
			}
		}

		int affectedNumberOfRows = await _setRepository.Upsert(upsertionData);

		return affectedNumberOfRows;
	}

	public async Task UpsertCardCollection()
	{
		//TODO: Add logging (maybe a db table entry for history) in controller to see that this method was called.
		await UpdateAndCacheSetEntities();

		await foreach (ApiCard apiCard in _scryfallApi.GetBulkCardDataAsync(BulkDataType.AllCards))
		{
			BatchCardData(apiCard);

			if (_cardBatch.Count >= 1000)
			{
				CommitBatchedData();
				ClearBatchedData();
			}
		}

		CommitBatchedData();
	}

	private async Task UpdateAndCacheSetEntities()
	{
		await UpsertSets();
		IEnumerable<Set> allScryfallSets = await _setRepository.Get(SourceType.Scryfall);
		_setLookup = allScryfallSets
			.Where(set => set.ScryfallId.HasValue)
			.ToDictionary(set => set.ScryfallId!.Value);
	}

	private void BatchCardData(ApiCard apiCard)
	{
		BatchCards(apiCard);
		BatchArtistsAndCardRelations(apiCard);
		BatchCardImages(apiCard);
		BatchCardPrices(apiCard);
		BatchCardGameTypeAvailability(apiCard);
		BatchPrintFinishes(apiCard);
		BatchGameFormatsAndLegalities(apiCard);
		BatchKeywordsAndCardRelations(apiCard);
		BatchPromoTypesAndCardRelations(apiCard);
	}

	private IReadOnlyList<Card> BatchCards(ApiCard apiCard)
	{
		Set set = _setLookup[apiCard.SetId];

		List<Card> mappedCards = apiCard.CardFaces is not { Length: > 0 }
			? new List<Card> { CardMapper.MapCard(apiCard, set) }
			: apiCard.CardFaces.Select(cardFace => CardMapper.MapCard(apiCard, set, cardFace: cardFace)).ToList();

		if (mappedCards.Count > 0)
		{
			Card parentCard = mappedCards.First();

			foreach (Card childCard in mappedCards.Skip(1))
			{
				childCard.ParentCard = parentCard;
			}
		}

		_cardBatch.UnionWith(mappedCards);
		return mappedCards;
	}


	private IReadOnlyList<Artist> BatchArtistsAndCardRelations(ApiCard apiCard)
	{
		List<Artist> artists = new();

		if (apiCard.CardFaces is { Length: > 0 })
		{
			foreach (CardFace cardFace in apiCard.CardFaces)
			{
				Artist artist = ArtistMapper.MapArtist(cardFace);

				_cardArtistBatch[$"{apiCard.Id}_{cardFace.Name}"] = artist;
				_artistBatch.Add(artist);
				artists.Add(artist);
			}
		}
		else
		{
			Artist artist = ArtistMapper.MapArtist(apiCard);

			_cardArtistBatch[$"{apiCard.Id}_{apiCard.Name}"] = artist;
			_artistBatch.Add(artist);
			artists.Add(artist);
		}

		return artists;
	}

	private IReadOnlyList<CardImage> BatchCardImages(ApiCard apiCard)
	{
		List<CardImage> cardImages = new();

		if (apiCard.CardFaces is { Length: > 0 })
		{
			foreach (CardFace cardFace in apiCard.CardFaces)
			{
				var apiCardImages = CardImageMapper.MapCardImages(apiCard, cardFace);

				_imageBatch[$"{apiCard.Id}_{cardFace.Name}"] = apiCardImages;
				cardImages.AddRange(apiCardImages);
			}
		}
		else
		{
			var apiCardImages = CardImageMapper.MapCardImages(apiCard);

			_imageBatch[$"{apiCard.Id}_{apiCard.Name}"] = apiCardImages;
			cardImages.AddRange(apiCardImages);
		}

		return cardImages;
	}

	private IReadOnlyList<CardPrice> BatchCardPrices(ApiCard apiCard)
	{
		List<CardPrice> pricesToUpsert = CardPriceMapper.MapCardPrices(apiCard);

		if (pricesToUpsert.Count > 0)
		{
			_cardPriceBatch[apiCard.Id] = pricesToUpsert;
		}

		return pricesToUpsert;
	}
	private IReadOnlyList<CardGameTypeAvailability> BatchCardGameTypeAvailability(ApiCard apiCard)
	{
		List<CardGameTypeAvailability> availabilityToUpsert = GameMapper.MapGameTypeAvailability(apiCard);

		if (availabilityToUpsert.Count > 0)
		{
			_cardAvailabilityBatch[apiCard.Id] = availabilityToUpsert;
		}

		return availabilityToUpsert;
	}

	private IReadOnlyList<CardPrintFinish> BatchPrintFinishes(ApiCard apiCard)
	{
		List<CardPrintFinish> apiPrintFinishes = CardMapper.MapCardPrintFinishes(apiCard);

		if (apiPrintFinishes.Count > 0)
		{
			_printFinishBatch[apiCard.Id] = apiPrintFinishes;
		}

		return apiPrintFinishes;
	}

	private IReadOnlyList<CardLegality> BatchGameFormatsAndLegalities(ApiCard apiCard)
	{
		IEnumerable<GameFormat> formatsOnCard = GameMapper.MapGameFormat(apiCard);
		_gameFormatsBatch.UnionWith(formatsOnCard);

		List<CardLegality> cardLegalities = CardMapper.MapCardLegalities(apiCard, _gameFormatsBatch);

		if (cardLegalities.Count > 0)
		{
			_cardLegalitiesBatch[apiCard.Id] = cardLegalities;
		}

		return cardLegalities;
	}

	private IReadOnlyList<CardKeyword> BatchKeywordsAndCardRelations(ApiCard apiCard)
	{
		if (apiCard.Keywords is not { Length: > 0 }) return new List<CardKeyword>();

		IEnumerable<Keyword> apiCardKeywords = CardMapper.MapKeywords(apiCard);
		_keywordsBatch.UnionWith(apiCardKeywords);

		List<CardKeyword> keywordsOnCard = CardMapper.MapCardKeywords(apiCard, _keywordsBatch);

		if (keywordsOnCard.Count > 0)
		{
			_cardKeywordsBatch[apiCard.Id] = keywordsOnCard;
		}

		return keywordsOnCard;
	}

	private IReadOnlyList<CardPromoType> BatchPromoTypesAndCardRelations(ApiCard apiCard)
	{
		if (apiCard.PromoTypes is not { Length: > 0 }) return new List<CardPromoType>();

		IEnumerable<PromoType> apiCardPromoTypes = CardMapper.MapPromoTypes(apiCard);
		_promoTypesBatch.UnionWith(apiCardPromoTypes);

		List<CardPromoType> promoTypesOnCard = CardMapper.MapCardPromoTypes(apiCard, apiCardPromoTypes);

		if (promoTypesOnCard.Count > 0)
		{
			_cardPromotypesBatch[apiCard.Id] = promoTypesOnCard;
		}

		return promoTypesOnCard;
	}

	private void CommitBatchedData()
	{
		throw new NotImplementedException();
	}

	private void ClearBatchedData()
	{
		_cardBatch.Clear();
		_imageBatch.Clear();
		_cardArtistBatch.Clear();
		_artistBatch.Clear();
		_cardPriceBatch.Clear();
		_cardAvailabilityBatch.Clear();
		_printFinishBatch.Clear();
		_gameFormatsBatch.Clear();
		_cardLegalitiesBatch.Clear();
		_keywordsBatch.Clear();
		_cardKeywordsBatch.Clear();
		_promoTypesBatch.Clear();
		_cardPromotypesBatch.Clear();
	}
}
