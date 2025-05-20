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
	private Dictionary<Guid, List<CardGamePlatform>> _cardGamePlatformBatch = new();
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
		await UpsertAndCacheSetEntities();

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

	/// <summary>
	/// Upserts <see cref="Set"/> entities from Scryfall API and caches them in the <see cref="_setLookup"/> for batching <see cref="Card"/> entities.<br/>
	/// Should be called once before upserting <see cref="Card"/> entities.
	/// </summary>
	private async Task UpsertAndCacheSetEntities()
	{
		await UpsertSets();
		IEnumerable<Set> allScryfallSets = await _setRepository.Get(SourceType.Scryfall);
		_setLookup = allScryfallSets
			.Where(set => set.ScryfallId.HasValue)
			.ToDictionary(set => set.ScryfallId!.Value);
	}

	/// <summary>
	/// Batches all the data from <paramref name="apiCard"/> into their respective batching dictionaries.<br/>
	/// </summary>
	private void BatchCardData(ApiCard apiCard)
	{
		BatchCards(apiCard);
		BatchArtistsAndCardRelations(apiCard);
		BatchCardImages(apiCard);
		BatchCardPrices(apiCard);
		BatchCardGamePlatform(apiCard);
		BatchPrintFinishes(apiCard);
		BatchGameFormatsAndLegalities(apiCard);
		BatchKeywordsAndCardRelations(apiCard);
		BatchPromoTypesAndCardRelations(apiCard);
	}

	/// <summary>
	/// Batches <see cref="Card"/> entities from information on <paramref name="apiCard"/>.<br/><br/>
	/// Sets the <see cref="Card.SetId"/> based on the <see cref="_setLookup"/>.<br/>
	/// Does not set <see cref="Card.ArtistId"/>, that is done in <see cref="UpsertArtists"/> on the <see cref="Card"/> entities in the <see cref="_cardBatch"/>.
	/// </summary>
	/// <returns>>A read-only list of <see cref="Card"/>.</returns>
	private IReadOnlyList<Card> BatchCards(ApiCard apiCard)
	{
		Set set = _setLookup[apiCard.SetId];

		List<Card> mappedCards = apiCard.CardFaces is not { Length: > 0 }
			? new List<Card> { CardMapper.MapCard(apiCard, set) }
			: apiCard.CardFaces.Select(cardFace => CardMapper.MapCard(apiCard, set, cardFace: cardFace)).ToList();

		if (mappedCards is { Count: > 1 })
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

	/// <summary>
	/// Batches <see cref="Artist"/> entities from artist information on <paramref name="apiCard"/>.<br/><br/>
	/// <see cref="Artist"/> entities are batched based with composite key of <see cref="Card.Id"/> and <see cref="Card.Name"/>.
	/// </summary>
	/// <returns>A read-only list of <see cref="Artist"/>. Returns an empty list if the <paramref name="apiCard"/> has no images yet.</returns>
	private IReadOnlyList<Artist> BatchArtistsAndCardRelations(ApiCard apiCard)
	{
		List<Artist> artists = new();

		if (apiCard.CardFaces is { Length: > 0 })
		{
			foreach (CardFace cardFace in apiCard.CardFaces)
			{
				Artist? artist = ArtistMapper.MapArtist(cardFace);
				if (artist is null) continue;

				_cardArtistBatch[$"{apiCard.Id}_{cardFace.Name}"] = artist;
				_artistBatch.Add(artist);
				artists.Add(artist);
			}
		}
		else
		{
			Artist? artist = ArtistMapper.MapArtist(apiCard);

			if (artist is not null)
			{
				_cardArtistBatch[$"{apiCard.Id}_{apiCard.Name}"] = artist;
				_artistBatch.Add(artist);
				artists.Add(artist);
			}
		}

		return artists;
	}

	/// <summary>
	/// Batches <see cref="CardImage"/> entities from image information on <paramref name="apiCard"/>.<br/><br/>
	/// <see cref="CardImage"/> entities are batched based with composite key of <see cref="Card.Id"/> and <see cref="Card.Name"/>.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardImage"/>. Returns an empty list if the <paramref name="apiCard"/> has no images yet.</returns>
	private IReadOnlyList<CardImage> BatchCardImages(ApiCard apiCard)
	{
		List<CardImage> cardImages = new();

		if (apiCard.CardFaces is { Length: > 0 })
		{
			foreach (CardFace cardFace in apiCard.CardFaces)
			{
				List<CardImage> apiCardImages = CardImageMapper.MapCardImages(apiCard, cardFace);

				_imageBatch[$"{apiCard.Id}_{cardFace.Name}"] = apiCardImages;
				cardImages.AddRange(apiCardImages);
			}
		}
		else
		{
			List<CardImage> apiCardImages = CardImageMapper.MapCardImages(apiCard);

			_imageBatch[$"{apiCard.Id}_{apiCard.Name}"] = apiCardImages;
			cardImages.AddRange(apiCardImages);
		}

		return cardImages;
	}

	/// <summary>
	/// Batches <see cref="CardPrice"/> entities from price information on <paramref name="apiCard"/>.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardPrice"/>. Returns an empty list if the <paramref name="apiCard"/> has no pricing information.</returns>
	private IReadOnlyList<CardPrice> BatchCardPrices(ApiCard apiCard)
	{
		List<CardPrice> pricesToUpsert = CardPriceMapper.MapCardPrices(apiCard);

		if (pricesToUpsert is { Count: > 0 })
		{
			_cardPriceBatch[apiCard.Id] = pricesToUpsert;
		}

		return pricesToUpsert;
	}

	/// <summary>
	/// Batches <see cref="CardGamePlatform"/> entities from game information on <paramref name="apiCard"/>.<br/><br/>
	/// <see cref="CardGamePlatform"/> represents relations between <see cref="Card"/> and <see cref="GamePlatform"/> entities.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardGamePlatform"/>. Returns an empty list if the <paramref name="apiCard"/> has no "game" information.</returns>
	private IReadOnlyList<CardGamePlatform> BatchCardGamePlatform(ApiCard apiCard)
	{
		List<CardGamePlatform> availabilityToUpsert = GameMapper.MapCardGamePlatform(apiCard);

		if (availabilityToUpsert is { Count: > 0 })
		{
			_cardGamePlatformBatch[apiCard.Id] = availabilityToUpsert;
		}

		return availabilityToUpsert;
	}

	/// <summary>
	/// Batches <see cref="CardPrintFinish"/> entities from print information on <paramref name="apiCard"/>.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardPrintFinish"/>. Returns an empty list if the <paramref name="apiCard"/> has no print finish entries.</returns>
	private IReadOnlyList<CardPrintFinish> BatchPrintFinishes(ApiCard apiCard)
	{
		List<CardPrintFinish> apiPrintFinishes = CardMapper.MapCardPrintFinishes(apiCard);

		if (apiPrintFinishes is { Count: > 0 })
		{
			_printFinishBatch[apiCard.Id] = apiPrintFinishes;
		}

		return apiPrintFinishes;
	}

	/// <summary>
	/// Batches <see cref="GameFormat"/> entities and <see cref="CardLegality"/> entities.<br/><br/>
	/// <see cref="CardLegality"/> represents relations between <see cref="Card"/> and <see cref="GameFormat"/> entities."/>
	/// </summary>
	/// <param name="apiCard"></param>
	/// <returns>A read-only list of <see cref="CardLegality"/>. Returns an empty list if the <paramref name="apiCard"/> has no logality information.</returns>
	private IReadOnlyList<CardLegality> BatchGameFormatsAndLegalities(ApiCard apiCard)
	{
		IEnumerable<GameFormat> formatsOnCard = GameMapper.MapGameFormat(apiCard);
		_gameFormatsBatch.UnionWith(formatsOnCard);

		List<CardLegality> cardLegalities = CardMapper.MapCardLegalities(apiCard, _gameFormatsBatch);

		if (cardLegalities is { Count: > 0 })
		{
			_cardLegalitiesBatch[apiCard.Id] = cardLegalities;
		}

		return cardLegalities;
	}

	/// <summary>
	/// Batches <see cref="Keyword"/> entities and <see cref="CardKeyword"/> entities.<br/><br/>
	/// <see cref="CardKeyword"/> represents relations between <see cref="Card"/> and <see cref="Keyword"/> entities.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardKeyword"/>. Returns an empty list if the <paramref name="apiCard"/> has no keywords.</returns>
	private IReadOnlyList<CardKeyword> BatchKeywordsAndCardRelations(ApiCard apiCard)
	{
		if (apiCard.Keywords is not { Length: > 0 }) return new List<CardKeyword>();

		IEnumerable<Keyword> apiCardKeywords = CardMapper.MapKeywords(apiCard);
		_keywordsBatch.UnionWith(apiCardKeywords);

		List<CardKeyword> keywordsOnCard = CardMapper.MapCardKeywords(apiCard, _keywordsBatch);

		if (keywordsOnCard is { Count: > 0 })
		{
			_cardKeywordsBatch[apiCard.Id] = keywordsOnCard;
		}

		return keywordsOnCard;
	}

	/// <summary>
	/// Batches <see cref="PromoType"/> entities and <see cref="CardPromoType"/> entities.<br/><br/>
	/// <see cref="CardPromoType"/> represents relations between <see cref="Card"/> and <see cref="PromoType"/> entities.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardPromoType"/>. Returns an empty list if the <paramref name="apiCard"/> has no promo types.</returns>
	private IReadOnlyList<CardPromoType> BatchPromoTypesAndCardRelations(ApiCard apiCard)
	{
		if (apiCard.PromoTypes is not { Length: > 0 }) return new List<CardPromoType>();

		IEnumerable<PromoType> apiCardPromoTypes = CardMapper.MapPromoTypes(apiCard);
		_promoTypesBatch.UnionWith(apiCardPromoTypes);

		List<CardPromoType> promoTypesOnCard = CardMapper.MapCardPromoTypes(apiCard, apiCardPromoTypes);

		if (promoTypesOnCard is { Count: > 0 })
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
			CreateMissingGameFormats(),
			CreateMissingKeywords(),
			CreateMissingPromoTypes()
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


	/// <summary>
	/// Inserts or updates <see cref="Artist"/> entities based on the current <see cref="_artistBatch"/>.<br/>
	/// After upserting, it assigns the <see cref="Artist.Id"/> to the <see cref="Card.ArtistId"/> property of the current <see cref="_cardBatch"/> entities.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="Artist"/> entities.</returns>
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

	/// <summary>
	/// Assigns <see cref="Artist.Id"/> to <see cref="Card"/> entities in the current <see cref="_cardBatch"/>.<br/>
	/// Assigns <see cref="Artist.DefaultId"/> to <see cref="Card"/> entity if no matching <see cref="Artist"/> is found in <see cref="_cardArtistBatch"/> or in the db.
	/// </summary>
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
			else
			{
				batchedCard.ArtistId = Artist.DefaultId;
			}
		}
	}

	/// <summary>
	/// Inserts or updates <see cref="Card"/> entities based on the current <see cref="_cardBatch"/>.<br/>
	/// After upserting, it assigns the <see cref="Card.Id"/> to the <see cref="CardImage.CardId"/> property of the current <see cref="_imageBatch"/> entities.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="Card"/> entities.</returns>
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
		IEnumerable<Card> updatedCards = await _cardRepository.Get(_cardBatch.Select(card => card.ScryfallId!.Value));

		AssignCardIdToBatchedImages(updatedCards);
		AssignCardIdToBatchedPrices(updatedCards);

		return affectedtedNumberOfRows;
	}

	/// <summary>
	/// Assigns <see cref="Card.Id"/> from <paramref name="cardsWithIdentity"/> to <see cref="CardImage.CardId"/> on entities in the current <see cref="_imageBatch"/>.
	/// </summary>
	private void AssignCardIdToBatchedImages(IEnumerable<Card> cardsWithIdentity)
	{
		foreach (Card card in cardsWithIdentity)
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

	/// <summary>
	/// Assigns <see cref="Card.Id"/> from <paramref name="cardsWithIdentity"/> to <see cref="CardPrice.CardId"/> on entities in the current <see cref="_cardPriceBatch"/>.
	/// </summary>
	private void AssignCardIdToBatchedPrices(IEnumerable<Card> cardsWithIdentity)
	{
		foreach (Card card in cardsWithIdentity)
		{
			if (_cardPriceBatch.TryGetValue(card.ScryfallId!.Value, out List<CardPrice>? prices))
			{
				foreach (CardPrice price in prices)
				{
					price.CardId = card.Id;
				}
			}
		}
	}

	/// <summary>
	/// Creates <see cref="GameFormat"/> entities from <see cref="_gameFormatsBatch"/> if they do not have an existing naming match in the db.
	/// </summary>
	/// <returns>The number of added <see cref="GameFormat"/> entities.</returns>
	private async Task<int> CreateMissingGameFormats()
	{
		List<GameFormat> missingGameFormats = new();
		IEnumerable<GameFormat> existingFormats = await _gameRepository.GetFormats(SourceType.Scryfall);

		foreach (GameFormat batchedFormat in _gameFormatsBatch)
		{
			GameFormat? existingFormat = existingFormats.FirstWithNameOrDefault(batchedFormat.Name, SourceType.Scryfall);

			if (existingFormat is null)
			{
				missingGameFormats.Add(batchedFormat);
			}
		}

		return missingGameFormats.Count > 0 ?
			(await _gameRepository.CreateFormats(missingGameFormats)).Count()
			: 0;
	}

	/// <summary>
	/// Creates <see cref="Keyword"/> entities from <see cref="_keywordsBatch"/> if they do not have an existing naming match in the db.
	/// </summary>
	/// <returns>The number of added <see cref="Keyword"/> entities.</returns>
	private async Task<int> CreateMissingKeywords()
	{
		List<Keyword> missingKeywords = new();
		IEnumerable<Keyword> existingKeywords = await _cardRepository.GetKeywords(SourceType.Scryfall);

		foreach (Keyword batchedKeyword in _keywordsBatch)
		{
			Keyword? existingKeyword = existingKeywords.FirstWithNameOrDefault(batchedKeyword.Name, SourceType.Scryfall);

			if (existingKeyword is null)
			{
				missingKeywords.Add(batchedKeyword);
			}
		}

		return missingKeywords.Count > 0
			? (await _cardRepository.CreateKeywords(missingKeywords)).Count()
			: 0;
	}

	private async Task<int> CreateMissingPromoTypes()
	{
		List<PromoType> missingPromoTypes = new();
		IEnumerable<PromoType> existingPromoTypes = await _cardRepository.GetPromoTypes(SourceType.Scryfall);

		foreach (PromoType batchedPromoType in _promoTypesBatch)
		{
			PromoType? existingPromoType = existingPromoTypes.FirstWithNameOrDefault(batchedPromoType.Name, SourceType.Scryfall);
			if (existingPromoType is null)
			{
				missingPromoTypes.Add(batchedPromoType);
			}
		}

		return missingPromoTypes.Count > 0
			? (await _cardRepository.CreatePromoTypes(missingPromoTypes)).Count()
			: 0;
	}

	/// <summary>
	/// Inserts or updates <see cref="CardImage"/> entities based on the current<see cref="_imageBatch"/>.<br/>
	/// Skips any images with a <see cref="CardImage.CardId"/> of 0.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="CardImage"/> entities.</returns>
	private async Task<int> UpsertImages()
	{
		UpsertContainer<CardImage> upsertionData = new();
		IEnumerable<CardImage> existingImages = await _cardImageRepository.Get(SourceType.Scryfall);

		foreach (CardImage batchedImage in _imageBatch.Values.SelectMany(image => image))
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

	private void ClearBatchedData()
	{
		_cardBatch.Clear();
		_imageBatch.Clear();
		_cardArtistBatch.Clear();
		_artistBatch.Clear();
		_cardPriceBatch.Clear();
		_cardGamePlatformBatch.Clear();
		_printFinishBatch.Clear();
		_gameFormatsBatch.Clear();
		_cardLegalitiesBatch.Clear();
		_keywordsBatch.Clear();
		_cardKeywordsBatch.Clear();
		_promoTypesBatch.Clear();
		_cardPromotypesBatch.Clear();
	}
}
