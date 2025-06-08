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
	private readonly IGameRepository _gameRepository;
	private readonly IEqualityComparer<Set> _setComparer;
	private readonly IEqualityComparer<Card> _cardComparer;
	private readonly IEqualityComparer<Artist> _artistComparer;
	private readonly IEqualityComparer<CardImage> _imageComparer;
	private readonly IEqualityComparer<CardPrice> _priceComparer;
	private readonly IEqualityComparer<CardLegality> _cardLegalityComparer;
	private readonly IEqualityComparer<GameFormat> _gameFormatComparer;
	private readonly IEqualityComparer<Keyword> _keywordComparer;
	private readonly IEqualityComparer<PromoType> _promoTypeComparer;
	private readonly IScryfallApi _scryfallApi;
	private readonly ISetRepository _setRepository;

	//Batching data
	private Dictionary<Guid, Set> _setLookup = new();
	private HashSet<Card> _cardBatch = new();
	private Dictionary<(Guid cardScryfallId, string cardName), List<CardImage>> _imageBatch = new();
	private Dictionary<(Guid cardScryfallId, string cardName), Artist> _cardArtistBatch = new();
	private HashSet<Artist> _artistBatch = new();
	private Dictionary<Guid, List<CardPrice>> _cardPriceBatch = new();
	private Dictionary<Guid, List<CardGamePlatform>> _cardGamePlatformBatch = new();
	private Dictionary<Guid, List<CardPrintFinish>> _cardPrintFinishBatch = new();
	private HashSet<GameFormat> _gameFormatsBatch = new();
	private Dictionary<Guid, List<(string formatName, CardLegality legality)>> _cardLegalitiesBatch = new();
	private HashSet<Keyword> _keywordsBatch = new();
	private Dictionary<Guid, List<(string keywordName, CardKeyword cardKeyword)>> _cardKeywordsBatch = new();
	private HashSet<PromoType> _promoTypesBatch = new();
	private Dictionary<Guid, List<(string promoTypeName, CardPromoType cardPromoType)>> _cardPromoTypesBatch = new();

	public ScryfallIngestionService(
		IArtistRepository artistRepository,
		ICardImageRepository cardImageRepository,
		ICardRepository cardRepository,
		IGameRepository gameRepository,
		IEqualityComparer<Set> setComparer,
		IEqualityComparer<Card> cardComparer,
		IEqualityComparer<Artist> artistComparer,
		IEqualityComparer<CardImage> imageComparer,
		IEqualityComparer<CardPrice> priceComparer,
		IEqualityComparer<CardLegality> cardLegalityComparer,
		IEqualityComparer<GameFormat> gameFormatComparer,
		IEqualityComparer<Keyword> keywordComparer,
		IEqualityComparer<PromoType> promoTypeComparer,
		IScryfallApi scryfallApi,
		ISetRepository setRepository)
	{
		_artistRepository = artistRepository;
		_cardImageRepository = cardImageRepository;
		_cardRepository = cardRepository;
		_gameRepository = gameRepository;
		_setComparer = setComparer;
		_cardComparer = cardComparer;
		_artistComparer = artistComparer;
		_imageComparer = imageComparer;
		_priceComparer = priceComparer;
		_cardLegalityComparer = cardLegalityComparer;
		_setComparer = setComparer;
		_gameFormatComparer = gameFormatComparer;
		_keywordComparer = keywordComparer;
		_promoTypeComparer = promoTypeComparer;
		_scryfallApi = scryfallApi;
		_setRepository = setRepository;
	}

	public async Task<int> UpsertSets()
	{
		UpsertContainer<Set> upsertionData = new();
		IEnumerable<ApiSet> apiSets = await _scryfallApi.GetSets();
		IEnumerable<Set> existingSets = await _setRepository.Get(apiSets.Select(set => set.Id));
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

			if (_cardBatch.Count >= 2000)
			{
				await PersistBatchedData();
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
		List<Card> mappedCards = CardMapper.MapCard(apiCard, set);

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

				_cardArtistBatch[(apiCard.Id, cardFace.Name)] = artist;
				_artistBatch.Add(artist);
				artists.Add(artist);
			}
		}
		else
		{
			Artist? artist = ArtistMapper.MapArtist(apiCard);

			if (artist is not null)
			{
				_cardArtistBatch[(apiCard.Id, apiCard.Name)] = artist;
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

				_imageBatch[(apiCard.Id, cardFace.Name)] = apiCardImages;
				cardImages.AddRange(apiCardImages);
			}
		}
		else
		{
			List<CardImage> apiCardImages = CardImageMapper.MapCardImages(apiCard);

			_imageBatch[(apiCard.Id, apiCard.Name)] = apiCardImages;
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
		List<CardGamePlatform> cardGamePlatformsToUpsert = GameMapper.MapCardGamePlatform(apiCard);

		if (cardGamePlatformsToUpsert is { Count: > 0 })
		{
			_cardGamePlatformBatch[apiCard.Id] = cardGamePlatformsToUpsert;
		}

		return cardGamePlatformsToUpsert;
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
			_cardPrintFinishBatch[apiCard.Id] = apiPrintFinishes;
		}

		return apiPrintFinishes;
	}

	/// <summary>
	/// Batches <see cref="GameFormat"/> entities and <see cref="CardLegality"/> entities.<br/><br/>
	/// <see cref="CardLegality"/> represents relations between <see cref="Card"/> and <see cref="GameFormat"/> entities."/>
	/// </summary>
	/// <param name="apiCard"></param>
	/// <returns>A read-only list of <see cref="CardLegality"/>. Returns an empty list if the <paramref name="apiCard"/> has no logality information.</returns>
	private IReadOnlyList<(string formatName, CardLegality legality)> BatchGameFormatsAndLegalities(ApiCard apiCard)
	{
		IEnumerable<GameFormat> formatsOnCard = GameMapper.MapGameFormat(apiCard);
		_gameFormatsBatch.UnionWith(formatsOnCard.Except(_gameFormatsBatch, _gameFormatComparer));

		List<(string formatName, CardLegality legality)> cardLegalities = CardMapper.MapCardLegalities(apiCard, _gameFormatsBatch);

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
	private IReadOnlyList<(string keywordName, CardKeyword cardKeyword)> BatchKeywordsAndCardRelations(ApiCard apiCard)
	{
		if (apiCard.Keywords is not { Length: > 0 }) return new List<(string keywordName, CardKeyword cardKeyword)>();

		IEnumerable<Keyword> apiCardKeywords = CardMapper.MapKeywords(apiCard);
		_keywordsBatch.UnionWith(apiCardKeywords.Except(_keywordsBatch, _keywordComparer));

		List<(string keywordName, CardKeyword cardKeyword)> keywordsOnCard = CardMapper.MapCardKeywords(apiCard, _keywordsBatch);

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
	private IReadOnlyList<(string promoTypeName, CardPromoType cardPromoType)> BatchPromoTypesAndCardRelations(ApiCard apiCard)
	{
		if (apiCard.PromoTypes is not { Length: > 0 }) return new List<(string promoTypeName, CardPromoType cardPromoType)>();

		IEnumerable<PromoType> apiCardPromoTypes = CardMapper.MapPromoTypes(apiCard);
		_promoTypesBatch.UnionWith(apiCardPromoTypes.Except(_promoTypesBatch, _promoTypeComparer));

		List<(string promoTypeName, CardPromoType cardPromoType)> promoTypesOnCard = CardMapper.MapCardPromoTypes(apiCard, apiCardPromoTypes);

		if (promoTypesOnCard is { Count: > 0 })
		{
			_cardPromoTypesBatch[apiCard.Id] = promoTypesOnCard;
		}

		return promoTypesOnCard;
	}

	/// <summary>
	/// Commits all batched entities to the database.
	/// </summary>
	private async Task PersistBatchedData()
	{
		await UpsertArtists();

		Task<IEnumerable<Card>> upsertedCardsTask = UpsertCards();
		await Task.WhenAll(
			upsertedCardsTask,
			CreateMissingGameFormats(),
			CreateMissingKeywords(),
			CreateMissingPromoTypes()
		);

		IEnumerable<Card> updatedCards = await upsertedCardsTask;

		await Task.WhenAll(
			UpsertImages(),
			UpsertCardPrices(),
			CreateMissingCardGamePlatforms(updatedCards),
			CreateMissingCardPrintFinishes(updatedCards),
			UpsertCardLegalities(),
			CreateMissingCardKeywords(updatedCards),
			CreateMissingCardPromoTypes(updatedCards)
		);
	}

	/// <summary>
	/// Inserts or updates <see cref="Artist"/> entities based on the current <see cref="_artistBatch"/>.<br/>
	/// After upserting, it assigns the <see cref="Artist.Id"/> to the <see cref="Card.ArtistId"/> property of the current <see cref="_cardBatch"/> entities.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="Artist"/> entities.</returns>
	private async Task<int> UpsertArtists()
	{
		IEnumerable<Artist> existingArtists = await _artistRepository.Get(_artistBatch.Select(a => a.ScryfallId!.Value));

		UpsertContainer<Artist> upsertionData = _artistBatch.ToUpsertData<Artist, Guid, int>(
			existingArtists,
			artist => artist.ScryfallId!.Value,
			_artistComparer
		);

		int numberOfAffectedRows = await _artistRepository.Upsert(upsertionData);
		await AssignArtistIdsOnBatchedCards();

		_artistBatch.Clear();
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
			if (batchedCard.ScryfallId.HasValue &&
				_cardArtistBatch.TryGetValue((batchedCard.ScryfallId.Value, batchedCard.Name), out Artist? batchedArtist) &&
				batchedArtist.ScryfallId.HasValue &&
				artistLookup.TryGetValue(batchedArtist.ScryfallId.Value, out Artist? existingArtist))
			{
				batchedCard.ArtistId = existingArtist.Id;
			}
			else
			{
				batchedCard.ArtistId = Artist.DefaultId;
			}
		}

		_cardArtistBatch.Clear();
	}

	/// <summary>
	/// Inserts or updates <see cref="Card"/> entities based on the current <see cref="_cardBatch"/>.<br/>
	/// After upserting, it assigns the <see cref="Card.Id"/> to the <see cref="CardImage.CardId"/> property of the current <see cref="_imageBatch"/> entities.
	/// </summary>
	/// <returns>All upserted <see cref="Card"/> entities.</returns>
	private async Task<IEnumerable<Card>> UpsertCards()
	{
		IEnumerable<Card> existingCards = await _cardRepository.Get(_cardBatch.Select(card => card.ScryfallId!.Value));
		UpsertContainer<Card> upsertionData = new();
		Dictionary<(Guid Value, string), Card> cardLookup = existingCards.ToDictionary(card => (card.ScryfallId!.Value, card.Name));

		foreach (Card batchedCard in _cardBatch)
		{
			if (cardLookup.TryGetValue((batchedCard.ScryfallId!.Value, batchedCard.Name), out Card? existingCard))
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

		await _cardRepository.Upsert(upsertionData);

		IEnumerable<Card> updatedCards = await _cardRepository.Get(_cardBatch.Select(card => card.ScryfallId!.Value));
		AssignCardIdToBatchedEntities(updatedCards);

		_cardBatch.Clear();
		return updatedCards;
	}

	/// <summary>
	/// Assigns <see cref="Card.Id"/> from the provided <paramref name="cards"/> to the batched entities with relations to <see cref="Card"/>.
	/// </summary>
	private void AssignCardIdToBatchedEntities(IEnumerable<Card> cards)
	{
		_imageBatch.AssignCardIdToEntities(cards);
		_cardPriceBatch.AssignCardIdToEntities(cards);
		_cardGamePlatformBatch.AssignCardIdToEntities(cards);
		_cardPrintFinishBatch.AssignCardIdToEntities(cards);
		_cardLegalitiesBatch.AssignCardIdToEntities(cards);
		_cardKeywordsBatch.AssignCardIdToEntities(cards);
		_cardPromoTypesBatch.AssignCardIdToEntities(cards);
	}

	/// <summary>
	/// Creates <see cref="GameFormat"/> entities from <see cref="_gameFormatsBatch"/> if they do not have an existing naming match in the db.
	/// </summary>
	/// <returns>The number of added <see cref="GameFormat"/> entities.</returns>
	private async Task<int> CreateMissingGameFormats()
	{
		IEnumerable<GameFormat> existingFormats = await _gameRepository.GetFormats(SourceType.Scryfall);
		List<GameFormat> missingGameFormats = _gameFormatsBatch.FindMissingEntities(existingFormats);

		int addedFormatsCount = await _gameRepository.Create(missingGameFormats);
		await AssignGameFormatIdsToLegalities();

		_gameFormatsBatch.Clear();
		return addedFormatsCount;
	}

	/// <summary>
	/// Creates <see cref="Keyword"/> entities from <see cref="_keywordsBatch"/> if they do not have an existing naming match in the db.
	/// </summary>
	/// <returns>The number of added <see cref="Keyword"/> entities.</returns>
	private async Task<int> CreateMissingKeywords()
	{
		IEnumerable<Keyword> existingKeywords = await _cardRepository.GetKeywords(SourceType.Scryfall);
		List<Keyword> missingKeywords = _keywordsBatch.FindMissingEntities(existingKeywords);

		int addedKeywordsCount = await _cardRepository.Create(missingKeywords);
		await AssignKeywordIdsToCardKeywords();

		_keywordsBatch.Clear();
		return addedKeywordsCount;
	}

	/// <summary>
	/// Creates <see cref="PromoType"/> entities from <see cref="_promoTypesBatch"/> if they do not have an existing naming match in the db.
	/// </summary>
	/// <returns>The number of added <see cref="PromoType"/> entities.</returns>
	private async Task<int> CreateMissingPromoTypes()
	{
		IEnumerable<PromoType> existingPromoTypes = await _cardRepository.GetPromoTypes(SourceType.Scryfall);
		List<PromoType> missingPromoTypes = _promoTypesBatch.FindMissingEntities(existingPromoTypes);

		int addedPromoTypesCount = await _cardRepository.Create(missingPromoTypes);
		await AssignPromoTypesIdsToCardPromoTypes();

		_promoTypesBatch.Clear();
		return addedPromoTypesCount;
	}

	/// <summary>
	/// Assigns the <see cref="CardLegality.GameFormatId"/> on batched (non-persisted) entities in <see cref="_cardLegalitiesBatch"/>.
	/// </summary>
	private async Task AssignGameFormatIdsToLegalities() =>
		_cardLegalitiesBatch.AssignRelationalIdToEntities(
			await _gameRepository.GetFormats(SourceType.Scryfall),
			(legality, id) => legality.GameFormatId = id
		);

	/// <summary>
	/// Assigns the <see cref="CardKeyword.KeywordId"/> on batched (non-persisted) entities in <see cref="_cardKeywordsBatch"/>.
	/// </summary>
	private async Task AssignKeywordIdsToCardKeywords() =>
		_cardKeywordsBatch.AssignRelationalIdToEntities(
			await _cardRepository.GetKeywords(SourceType.Scryfall),
			(cardKeyword, id) => cardKeyword.KeywordId = id
		);

	/// <summary>
	/// Assigns the <see cref="CardPromoType.PromoTypeId"/> on batched (non-persisted) entities in <see cref="_cardPromoTypesBatch"/>.
	/// </summary>
	private async Task AssignPromoTypesIdsToCardPromoTypes() =>
		_cardPromoTypesBatch.AssignRelationalIdToEntities(
			await _cardRepository.GetPromoTypes(SourceType.Scryfall),
			(cardPromoType, id) => cardPromoType.PromoTypeId = id
		);

	/// <summary>
	/// Inserts or updates <see cref="CardImage"/> entities based on the current<see cref="_imageBatch"/>.<br/>
	/// Skips any images with a <see cref="CardImage.CardId"/> of 0.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="CardImage"/> entities.</returns>
	private async Task<int> UpsertImages()
	{
		List<CardImage> batchedImages = _imageBatch
			.Values
			.SelectMany(imageList => imageList)
			.Where(image => image.CardId != 0)
			.ToList();

		IEnumerable<long> cardIds = batchedImages.Select(image => image.CardId).Distinct();
		IEnumerable<CardImage> existingImages = await _cardImageRepository.GetFromCardIds(cardIds);
		UpsertContainer<CardImage> upsertionData = batchedImages.ToUpsertData<CardImage, (long, int), long>(
			existingImages,
			image => (image.CardId, image.ImageTypeId),
			_imageComparer
		);

		int upsertedCount = await _cardImageRepository.Upsert(upsertionData);
		_imageBatch.Clear();

		return upsertedCount;
	}

	/// <summary>
	/// Inserts or updates <see cref="CardPrice"/> entities based on the current<see cref="_cardPriceBatch"/>.<br/>
	/// Skips any images with a <see cref="CardPrice.CardId"/> of 0.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="CardPrice"/> entities.</returns>
	private async Task<int> UpsertCardPrices()
	{
		List<CardPrice> batchedPrices = _cardPriceBatch
			.Values
			.SelectMany(cardPriceList => cardPriceList)
			.Where(cardPrice => cardPrice.CardId != 0)
			.ToList();

		IEnumerable<long> cardIds = batchedPrices.Select(image => image.CardId).Distinct();
		IEnumerable<CardPrice> existingCardPrices = await _cardRepository.GetCardPrices(cardIds);

		UpsertContainer<CardPrice> upsertionData = batchedPrices.ToUpsertData<CardPrice, (long, int, int), long>(
			existingCardPrices,
			cardPrice => (cardPrice.CardId, cardPrice.Vendor.Id, cardPrice.Currency.Id),
			_priceComparer
		);

		int upsertedCount = await _cardRepository.Upsert(upsertionData);
		_cardPriceBatch.Clear();

		return upsertedCount;
	}

	/// <summary>
	/// Adds all missing <see cref="CardGamePlatform"/> entities to the database from <see cref="_cardGamePlatformBatch"/>.<br/>
	/// <see cref="CardGamePlatform"/> represents the relationship between <see cref="GamePlatform"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardGamePlatform"/> entities.</returns>
	private async Task<int> CreateMissingCardGamePlatforms(IEnumerable<Card> existingCards)
	{
		IEnumerable<CardGamePlatform> existingPlatforms = await _cardRepository.GetCardGamePlatforms(existingCards.Select(card => card.Id));
		List<CardGamePlatform> missingPlatforms = _cardGamePlatformBatch.FindMissingEntities(
			existingEntities: existingPlatforms,
			filterExistingEntities: cgp => (cgp.GamePlatformId, cgp.CardId)
		);

		int addedPlatformsCount = await _cardRepository.Create(missingPlatforms);

		_cardGamePlatformBatch.Clear();
		return addedPlatformsCount;
	}

	/// <summary>
	/// Adds all missing <see cref="CardPrintFinish"/> entities to the database from <see cref="_cardPrintFinishBatch"/>.<br/>
	/// <see cref="CardPrintFinish"/> represents the relationship between <see cref="PrintFinish"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardPrintFinish"/> entities.</returns>
	private async Task<int> CreateMissingCardPrintFinishes(IEnumerable<Card> existingCards)
	{
		IEnumerable<CardPrintFinish> existingPrintFinishes = await _cardRepository.GetCardPrintFinishes(existingCards.Select(card => card.Id));
		List<CardPrintFinish> missingPrintFinishes = _cardPrintFinishBatch.FindMissingEntities(
			existingEntities: existingPrintFinishes,
			filterExistingEntities: cpf => (cpf.PrintFinishId, cpf.CardId)
		);

		int addedPrintFinishesCount = await _cardRepository.Create(missingPrintFinishes);

		_cardPrintFinishBatch.Clear();
		return addedPrintFinishesCount;
	}

	/// <summary>
	/// Inserts or updates <see cref="CardLegality"/> entities based on the current <see cref="_cardLegalitiesBatch"/>.<br/>
	/// Skips any images with a <see cref="CardPrice.CardId"/> of 0.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="CardLegality"/> entities.</returns>
	private async Task<int> UpsertCardLegalities()
	{
		List<CardLegality> batchedCardLegalities = _cardLegalitiesBatch
			.Values
			.SelectMany(cardLegalityTupleList => cardLegalityTupleList)
			.Select(tuple => tuple.legality)
			.Where(cardLegality => cardLegality.CardId != 0 && cardLegality.GameFormatId != 0)
			.ToList();

		IEnumerable<long> cardIds = batchedCardLegalities.Select(cardLegality => cardLegality.CardId).Distinct();
		IEnumerable<CardLegality> existingCardLegalities = await _cardRepository.GetCardLegalities(cardIds);

		UpsertContainer<CardLegality> upsertionData = batchedCardLegalities.ToUpsertData<CardLegality, (long, long), long>(
			existingCardLegalities,
			cardLegality => (cardLegality.CardId, cardLegality.GameFormatId),
			_cardLegalityComparer
		);

		int upsertedCount = await _cardRepository.Upsert(upsertionData);

		_cardLegalitiesBatch.Clear();
		return upsertedCount;
	}

	/// <summary>
	/// Adds all missing <see cref="CardKeyword"/> entities to the database from <see cref="_cardKeywordsBatch"/>.<br/>
	/// <see cref="CardKeyword"/> represents the relationship between <see cref="Keyword"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardKeyword"/> entities.</returns>
	private async Task<int> CreateMissingCardKeywords(IEnumerable<Card> existingCards)
	{
		IEnumerable<CardKeyword> existingCardKeywords = await _cardRepository.GetCardKeywords(existingCards.Select(card => card.Id));

		List<CardKeyword> missingCardKeywords = _cardKeywordsBatch.FindMissingEntities(
			existingEntities: existingCardKeywords,
			omitDefaultValues: ck => ck.KeywordId != 0 && ck.CardId != 0,
			filterExistingEntities: ck => (ck.KeywordId, ck.CardId)
		);

		int addedKeywordsCount = await _cardRepository.Create(missingCardKeywords);

		_cardKeywordsBatch.Clear();
		return addedKeywordsCount;
	}

	/// <summary>
	/// Adds all missing <see cref="CardPromoType"/> entities to the database from <see cref="_cardPromoTypesBatch"/>.<br/>
	/// <see cref="CardPromoType"/> represents the relationship between <see cref="PromoType"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardKeyword"/> entities.</returns>
	private async Task<int> CreateMissingCardPromoTypes(IEnumerable<Card> existingCards)
	{
		IEnumerable<CardPromoType> existingCardPromoTypes = await _cardRepository.GetCardPromoTypes(existingCards.Select(card => card.Id));

		List<CardPromoType> missingCardPromoTypes = _cardPromoTypesBatch.FindMissingEntities(
			existingEntities: existingCardPromoTypes,
			omitDefaultValues: cpt => cpt.PromoTypeId != 0 && cpt.CardId != 0,
			filterExistingEntities: cpt => (cpt.PromoTypeId, cpt.CardId)
		);

		int addedPromoTypesCount = await _cardRepository.Create(missingCardPromoTypes);

		_cardPromoTypesBatch.Clear();
		return addedPromoTypesCount;
	}
}
