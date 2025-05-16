using CardAtlas.Server.Extensions;
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
	//Dependencies
	private readonly IArtistRepository _artistRepository;
	private readonly ICardImageRepository _cardImageRepository;
	private readonly ICardRepository _cardRepository;
	private readonly IEqualityComparer<Keyword> _keywordComparer;
	private readonly IGameRepository _gameRepository;
	private readonly IEqualityComparer<Set> _setComparer;
	private readonly IEqualityComparer<Card> _cardComparer;
	private readonly IEqualityComparer<Artist> _artistComparer;
	private readonly IEqualityComparer<GameFormat> _gameFormatComparer;
	private readonly IEqualityComparer<CardImage> _imageComparer;
	private readonly IScryfallApi _scryfallApi;
	private readonly ISetRepository _setRepository;

	//Batching data
	private Dictionary<Guid, Set> _setLookup = new();
	private HashSet<Card> _cardBatch = new();
	private Dictionary<string, List<CardImage>> _imageBatch = new();
	private Dictionary<string, Artist> _cardArtistBatch = new();
	private HashSet<Artist> _artistBatch = new();
	private Dictionary<Guid, List<CardPrice>> _cardPriceBatch = new();
	private Dictionary<Guid, List<CardGameType>> _cardAvailabilityBatch = new();
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
		IEqualityComparer<Card> cardComparer,
		IEqualityComparer<Artist> artistComparer,
		IEqualityComparer<GameFormat> gameFormatComparer,
		IEqualityComparer<CardImage> imageComparer,
		IScryfallApi scryfallApi,
		ISetRepository setRepository)
	{
		_artistRepository = artistRepository;
		_cardImageRepository = cardImageRepository;
		_cardRepository = cardRepository;
		_keywordComparer = keywordComparer;
		_gameRepository = gameRepository;
		_setComparer = setComparer;
		_cardComparer = cardComparer;
		_artistComparer = artistComparer;
		_gameFormatComparer = gameFormatComparer;
		_imageComparer = imageComparer;
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
				await PersistBatchedData();
				ClearBatchedData();
			}
		}

		await PersistBatchedData();
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
	private IReadOnlyList<CardGameType> BatchCardGameTypeAvailability(ApiCard apiCard)
	{
		List<CardGameType> availabilityToUpsert = GameMapper.MapGameTypeAvailability(apiCard);

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

	private async Task PersistBatchedData()
	{
		await UpsertArtists();

		await Task.WhenAll(
			UpsertCards(),
			UpsertGameFormats()
		//keywords, promotypes
		);

		IEnumerable<Card> updatedCards = await _cardRepository.Get(_cardBatch.Select(card => card.ScryfallId!.Value));
		Dictionary<Guid, Card> cardLookup = updatedCards.ToDictionary(card => card.ScryfallId!.Value);

		await Task.WhenAll(
			UpsertImages()
		//BatchCardPrices(apiCard);
		//BatchCardGameTypeAvailability(apiCard);
		//PrintFinishes(apiCard);
		//Legalities(apiCard);
		//KeywordRelations(apiCard);
		//PromoTypeRelations(apiCard);
		);
	}



	private async Task<int> UpsertArtists()
	{
		UpsertContainer<Artist> upsertionData = new();
		IEnumerable<Artist> existingArtists = await _artistRepository.Get(_artistBatch.Select(artist => artist.ScryfallId!.Value));
		Dictionary<Guid, Artist> artistLookup = existingArtists.ToDictionary(artist => artist.ScryfallId!.Value);

		foreach (Artist batchedArtist in _artistBatch)
		{
			if (artistLookup.TryGetValue(batchedArtist.ScryfallId!.Value, out Artist? existingArtist))
			{
				batchedArtist.Id = existingArtist.Id;

				if (_artistComparer.Equals(existingArtist, batchedArtist)) continue;

				upsertionData.ToUpdate.Add(batchedArtist);
			}
			else
			{
				upsertionData.ToInsert.Add(batchedArtist);
			}
		}

		int numberOfAffectedRows = await _artistRepository.Upsert(upsertionData);
		await AssignArtistIdsOnBatchedCards();

		return numberOfAffectedRows;
	}

	private async Task AssignArtistIdsOnBatchedCards()
	{
		IEnumerable<Artist> existingArtists = await _artistRepository.Get(_artistBatch.Select(artist => artist.ScryfallId!.Value));
		Dictionary<Guid, Artist> artistLookup = existingArtists.ToDictionary(artist => artist.ScryfallId!.Value);

		foreach (Card batchedCard in _cardBatch)
		{
			if (_cardArtistBatch.TryGetValue($"{batchedCard.ScryfallId}_{batchedCard.Name}", out Artist? batchedArtist) &&
				artistLookup.TryGetValue(batchedArtist.ScryfallId!.Value, out Artist? existingArtist))
			{
				batchedCard.ArtistId = batchedArtist.Id;
			}
		}
	}

	private async Task<int> UpsertCards()
	{
		IEnumerable<Card> existingCards = await _cardRepository.Get(_cardBatch.Select(card => card.ScryfallId!.Value));
		UpsertContainer<Card> upsertionData = new();
		Dictionary<Guid, Card> cardLookup = existingCards.ToDictionary(card => card.ScryfallId!.Value);

		foreach (Card batchedCard in _cardBatch)
		{
			if (cardLookup.TryGetValue(batchedCard.ScryfallId!.Value, out Card? existingCard))
			{
				batchedCard.ArtistId = existingCard.ArtistId;
				batchedCard.ParentCardId = existingCard.ParentCardId;
				batchedCard.Id = existingCard.Id;

				if (_cardComparer.Equals(existingCard, batchedCard)) continue;

				upsertionData.ToUpdate.Add(batchedCard);
			}
			else
			{
				upsertionData.ToInsert.Add(batchedCard);
			}
		}

		int affectedtedNumberOfRows = await _cardRepository.Upsert(upsertionData);
		await AssignCardIdToBatchedImages();

		return affectedtedNumberOfRows;
	}

	private async Task<int> UpsertGameFormats()
	{
		UpsertContainer<GameFormat> upsertionData = new();
		IEnumerable<GameFormat> existingFormats = await _gameRepository.GetFormats(SourceType.Scryfall);

		foreach (GameFormat batchedFormat in _gameFormatsBatch)
		{
			GameFormat? existingFormat = existingFormats.FirstWithNameOrDefault(batchedFormat.Name, SourceType.Scryfall);

			if (existingFormat is null)
			{
				upsertionData.ToInsert.Add(batchedFormat);
			}
			else
			{
				batchedFormat.Id = existingFormat.Id;
				if (_gameFormatComparer.Equals(existingFormat, batchedFormat)) continue;

				upsertionData.ToUpdate.Add(batchedFormat);
			}
		}

		return await _gameRepository.UpsertGameFormat(upsertionData);
	}

	private async Task<int> UpsertImages()
	{
		UpsertContainer<CardImage> upsertionData = new();
		IEnumerable<CardImage> existingImages = await _cardImageRepository.Get(SourceType.Scryfall);

		foreach (CardImage batchedImage in _imageBatch.Values.SelectMany(x => x))
		{
			if (batchedImage.CardId == default(int)) continue;
			CardImage? existingImage = existingImages.SingleOrDefault(cardImage => cardImage.CardId == batchedImage.CardId && cardImage.Type == (ImageTypeKind)batchedImage.ImageTypeId);

			if (existingImage is null)
			{
				upsertionData.ToInsert.Add(batchedImage);
			}
			else
			{
				batchedImage.Id = existingImage.Id;
				if (_imageComparer.Equals(existingImage, batchedImage)) continue;

				upsertionData.ToUpdate.Add(batchedImage);
			}
		}

		return await _cardImageRepository.Upsert(upsertionData);
	}

	private async Task AssignCardIdToBatchedImages()
	{
		IEnumerable<Card> updatedCards = await _cardRepository.Get(_cardBatch.Select(card => card.ScryfallId!.Value));

		foreach (Card card in updatedCards)
		{
			if (_imageBatch.TryGetValue($"{card.ScryfallId}_{card.Name}", out List<CardImage>? matchingImages))
			{
				foreach (CardImage image in matchingImages)
				{
					image.CardId = card.Id;
				}
			}
		}
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
