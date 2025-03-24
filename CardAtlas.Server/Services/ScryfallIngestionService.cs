using CardAtlas.Server.Extensions;
using CardAtlas.Server.Helpers;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.Server.Services.Interfaces;
using ScryfallApi;
using ScryfallApi.Models.Types;
using ApiCard = ScryfallApi.Models.Card;
using ApiSet = ScryfallApi.Models.Set;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Services;

public class ScryfallIngestionService : IScryfallIngestionService
{
	private readonly IArtistService _artistService;
	private readonly ICardImageService _cardImageService;
	private readonly ICardService _cardService;
	private readonly IEqualityComparer<Keyword> _keywordComparer;
	private readonly IGameService _gameService;
	private readonly IScryfallApi _scryfallApi;
	private readonly ISetService _setService;

	public ScryfallIngestionService(
		IArtistService artistService,
		ICardImageService cardImageService,
		ICardService cardService,
		IEqualityComparer<Keyword> keywordComparer,
		IGameService gameService,
		IScryfallApi scryfallApi,
		ISetService setService)
	{
		_artistService = artistService;
		_cardImageService = cardImageService;
		_cardService = cardService;
		_keywordComparer = keywordComparer;
		_gameService = gameService;
		_scryfallApi = scryfallApi;
		_setService = setService;
	}

	public async Task UpsertCardCollection()
	{
		//await UpsertSets();

		await foreach (ApiCard apiCard in _scryfallApi.GetBulkCardDataAsync(BulkDataType.AllCards))
		{
			//TODO: Add memorycache module
			await UpsertCard(apiCard);

			await Task.WhenAll(
				UpsertCardImages(apiCard),
				UpsertCardPrices(apiCard),
				UpdatePrintFinishes(apiCard),
				UpdateGameTypes(apiCard),
				CreateMissingGameFormats(apiCard),
				UpsertLegality(apiCard),
				UpsertKeywords(apiCard),
				UpsertPromoTypes(apiCard)
			);
		}
	}

	public async Task<int> UpsertSets()
	{
		int rowsAffected = 0;
		IEnumerable<ApiSet> apiSets = await _scryfallApi.GetSets();

		foreach (ApiSet apiSet in apiSets)
		{
			Set mappedSet = SetMapper.MapSet(apiSet);
			Set? existingSet = await _setService.GetFromScryfallId(apiSet.Id);

			if (existingSet is null)
			{
				await _setService.Create(mappedSet);
			}
			else
			{
				mappedSet.Id = existingSet.Id;
				await _setService.UpdateIfChanged(mappedSet);
			}

			rowsAffected++;
		}

		return rowsAffected;
	}

	public async Task<IEnumerable<Card>> UpsertCard(ApiCard apiCard)
	{
		return apiCard.CardFaces is { Length: > 1 }
			? await UpsertMultipleCards(apiCard, apiCard.CardFaces)
			: [await UpsertSingleCard(apiCard)];
	}

	/// <summary>
	/// Upserts data from <paramref name="apiCard"/> and <paramref name="cardFaces"/> into the database.<br/>
	/// Should be called when <paramref name="apiCard"/> has multiple <see cref="CardFace"/>.
	/// </summary>
	/// <returns>All <see cref="Card"/> objects that were created or updated.</returns>
	private async Task<IEnumerable<Card>> UpsertMultipleCards(ApiCard apiCard, IEnumerable<CardFace> cardFaces)
	{
		Set set = await GetOrCreateSet(apiCard.SetId);
		var cards = new HashSet<Card>();
		bool isFirstCardFace = true;
		long? parentId = null;

		foreach (CardFace cardFace in cardFaces)
		{
			Artist artist = await GetOrCreateArtist(apiCard, cardFace);
			Card? existingCard = await GetExistingCard(apiCard, cardFace);

			Card mappedCard = CardMapper.MapCard(apiCard, set, artist, cardFace);
			mappedCard.ParentCardId = parentId;

			if (existingCard is null)
			{
				cards.Add(await _cardService.Create(mappedCard));
			}
			else
			{
				mappedCard.Id = existingCard.Id;
				cards.Add(await _cardService.UpdateIfChanged(mappedCard));
			}

			if (isFirstCardFace)
			{
				parentId = cards.First().Id;
				isFirstCardFace = false;
			}
		}

		return cards;
	}

	/// <summary>
	/// Upserts data from <paramref name="apiCard"/> into the database.<br/>
	/// Should not be called when <paramref name="apiCard"/> has multiple <see cref="CardFace"/>.
	/// </summary>
	/// <returns>The <see cref="Card"/> that was created or updated.</returns>
	private async Task<Card> UpsertSingleCard(ApiCard apiCard)
	{
		Set set = await GetOrCreateSet(apiCard.SetId);
		Artist artist = await GetOrCreateArtist(apiCard);
		Card mappedCard = CardMapper.MapCard(apiCard, set, artist);
		Card? existingCard = await GetExistingCard(apiCard);

		if (existingCard is null)
		{
			return await _cardService.Create(mappedCard);
		}
		else
		{
			mappedCard.Id = existingCard.Id;
			return await _cardService.UpdateIfChanged(mappedCard);
		}
	}

	/// <summary>
	/// Gets the <see cref="Set""/> or creates a new if no matching set is found.<br/>
	/// Uses <see cref="IScryfallApi"/> to fetch set data if no match is found.
	/// </summary>
	/// <returns>The existing or newly created <see cref="Set"/>.</returns>
	private async Task<Set> GetOrCreateSet(Guid scryfallSetId)
	{
		Set? persistedSet = await _setService.GetFromScryfallId(scryfallSetId);
		if (persistedSet is not null) return persistedSet;

		ApiSet apiSet = await _scryfallApi.GetSet(scryfallSetId);
		Set newSet = SetMapper.MapSet(apiSet);

		return await _setService.Create(newSet);
	}

	/// <summary>
	/// Gets the <see cref="Artist"/> from <paramref name="apiCard"/>.<br/>
	/// Creates a new <see cref="Artist"/> if no matching artist is found in the database.<br/>
	/// Returns the default <see cref="Artist"/> if no artist data is available.
	/// </summary>
	/// <param name="cardFace">Is used for getting artist information if provided.</param>
	/// <returns>The existing, newly created or default <see cref="Artist"/>.</returns>
	private async Task<Artist> GetOrCreateArtist(ApiCard apiCard, CardFace? cardFace = null)
	{
		Artist artistFromCard = cardFace is not null
			? ArtistMapper.MapArtist(cardFace)
			: ArtistMapper.MapArtist(apiCard);

		return artistFromCard.ScryfallId.HasValue
			? await _artistService.GetFromScryfallId(artistFromCard.ScryfallId.Value) ?? await _artistService.Create(artistFromCard)
			: await _artistService.Get(Artist.DefaultArtistId);
	}

	/// <summary>
	/// Gets cards from the database with the scryfall id as <paramref name="apiCard"/>.<br/>
	/// Returns the first <see cref="Card"/> entry if multiple is found or null if none is found.
	/// </summary>
	/// <returns>The first <see cref="Card"/> with matching scryfall id. Returns null if no match is found.</returns>
	private async Task<Card?> GetExistingCard(ApiCard apiCard)
	{
		IEnumerable<Card> existingCard = await _cardService.GetFromScryfallId(apiCard.Id);

		return existingCard.FirstOrDefault();
	}

	/// <summary>
	/// Gets cards from the database with the scryfall id as <paramref name="apiCard"/>.<br/>
	/// Returns the first card that matches the <paramref name="cardFace"/> or null if none is found.
	/// </summary>
	/// <returns>The first <see cref="Card"/> that matches the <paramref name="cardFace"/>. Returns null if no match is found.</returns>
	private async Task<Card?> GetExistingCard(ApiCard apiCard, CardFace cardFace)
	{
		IEnumerable<Card> existingCards = await _cardService.GetFromScryfallId(apiCard.Id);

		return existingCards.Any()
			? existingCards.FirstWithNameOrDefault(cardFace.Name)
			: null;
	}

	public async Task<IEnumerable<CardImage>> UpsertCardImages(ApiCard apiCard)
	{
		return apiCard.CardFaces is { Length: > 1 }
			? await UpsertMultipleCardImagery(apiCard, apiCard.CardFaces)
			: await UpsertSingleCardImagery(apiCard);
	}

	/// <summary>
	/// Creates or updates card imagery data from <paramref name="apiCard"/> to the database.<br/>
	/// Should be called when <paramref name="apiCard"/> has multiple <see cref="CardFace"/>.
	/// </summary>
	/// <returns>All created or updated <see cref="CardImage"/> objects.</returns>
	private async Task<IEnumerable<CardImage>> UpsertMultipleCardImagery(ApiCard apiCard, IEnumerable<CardFace> cardFaces)
	{
		var upsertedCardImages = new List<CardImage>();

		foreach (CardFace cardFace in cardFaces)
		{
			Card? cardImageryOwner = await GetExistingCard(apiCard, cardFace);
			if (cardImageryOwner is null) continue;

			IEnumerable<CardImage> apiCardImages = CardImageMapper.MapCardImages(cardImageryOwner.Id, apiCard, cardFace);

			upsertedCardImages.AddRange(await UpsertCardImages(cardImageryOwner, apiCardImages));
		}

		return upsertedCardImages;
	}

	/// <summary>
	/// Creates or updates card imagery data from <paramref name="apiCard"/> to the database.<br/>
	/// Should not be called when <paramref name="apiCard"/> has multiple <see cref="CardFace"/>.
	/// </summary>
	/// <returns>All created or updated <see cref="CardImage"/> objects.</returns>
	private async Task<IEnumerable<CardImage>> UpsertSingleCardImagery(ApiCard apiCard)
	{
		Card? cardImageryOwner = await GetExistingCard(apiCard);
		if (cardImageryOwner is null) return new List<CardImage>();

		IEnumerable<CardImage> imagesToUpsert = CardImageMapper.MapCardImages(cardImageryOwner.Id, apiCard);

		return await UpsertCardImages(cardImageryOwner, imagesToUpsert);
	}

	/// <summary>
	/// Creates or updates card <paramref name="imagesToUpsert"/>.<br/>
	/// When updating an existing card image, the existing image is found by type and source.
	/// </summary>
	/// <returns>All created or updated <see cref="CardImage"/> objects.</returns>
	private async Task<IEnumerable<CardImage>> UpsertCardImages(Card imageryOwner, IEnumerable<CardImage> imagesToUpsert)
	{
		var upsertedCardImages = new List<CardImage>();
		if (!imagesToUpsert.Any()) return upsertedCardImages;

		IEnumerable<CardImage> existingImages = imageryOwner.Images.Any()
			? imageryOwner.Images
			: await _cardImageService.GetFromCardId(imageryOwner.Id);

		foreach (CardImage imageToUpsert in imagesToUpsert)
		{
			CardImage? existingCardImage = existingImages.FindMatchByTypeAndSource(imageToUpsert);

			if (existingCardImage is null)
			{
				upsertedCardImages.Add(await _cardImageService.Create(imageToUpsert));
			}
			else
			{
				imageToUpsert.Id = existingCardImage.Id;
				upsertedCardImages.Add(await _cardImageService.UpdateIfChanged(imageToUpsert));
			}
		}

		return upsertedCardImages;
	}

	public async Task<IEnumerable<CardPrice>> UpsertCardPrices(ApiCard apiCard)
	{
		var upsertedCardPrices = new List<CardPrice>();

		IEnumerable<Card> existingCards = await _cardService.GetFromScryfallId(apiCard.Id);
		if (!existingCards.Any()) return upsertedCardPrices;

		foreach (Card card in existingCards)
		{
			upsertedCardPrices.AddRange(await UpsertCardPrices(card, apiCard));
		}

		return upsertedCardPrices;
	}

	/// <summary>
	/// Creates or updates card prices from <paramref name="apiCard"/> to the database.
	/// </summary>
	/// <returns>All created or updated <see cref="CardPrice"/> objects. May be none if <paramref name="apiCard"/> contains no price data.</returns>
	private async Task<IEnumerable<CardPrice>> UpsertCardPrices(Card cardWithPrices, ApiCard apiCard)
	{
		var upsertedCardPrices = new List<CardPrice>();
		IEnumerable<CardPrice> pricesToUpsert = CardPriceMapper.MapCardPrices(cardWithPrices.Id, apiCard);
		if (!pricesToUpsert.Any()) return upsertedCardPrices;

		IEnumerable<CardPrice> existingPrices = cardWithPrices.Prices.Any()
			? cardWithPrices.Prices
			: await _cardService.GetPrices(cardWithPrices.Id);

		foreach (CardPrice priceToUpsert in pricesToUpsert)
		{
			CardPrice? existingCardPrice = existingPrices.FindMatchByVendorAndCurrency(priceToUpsert);

			if (existingCardPrice is null)
			{
				upsertedCardPrices.Add(await _cardService.CreatePrice(priceToUpsert));
			}
			else
			{
				priceToUpsert.Id = existingCardPrice.Id;
				upsertedCardPrices.Add(await _cardService.UpdatePriceIfChanged(priceToUpsert));
			}
		}

		return upsertedCardPrices;
	}

	public async Task<IEnumerable<CardPrintFinish>> UpdatePrintFinishes(ApiCard apiCard)
	{
		var printFinishes = new List<CardPrintFinish>();

		IEnumerable<Card> existingCards = await _cardService.GetFromScryfallId(apiCard.Id);
		if (!existingCards.Any()) return printFinishes;

		foreach (Card card in existingCards)
		{
			printFinishes.AddRange(await CreateMissingPrintFinishes(card, apiCard));
		}

		return printFinishes;
	}

	/// <summary>
	/// Creates finishes from <paramref name="apiCard"/> that are missing from <paramref name="card"/>.
	/// </summary>
	/// <returns>All <see cref="CardPrintFinish"/> associated with <paramref name="card"/>.</returns>
	private async Task<IEnumerable<CardPrintFinish>> CreateMissingPrintFinishes(Card card, ApiCard apiCard)
	{
		HashSet<PrintFinishType> apiPrintFinishes = CardMapper.MapPrintFinishes(apiCard);
		IEnumerable<CardPrintFinish> missingFinishes = apiPrintFinishes
			.Where(apiFinish => !card.PrintFinishes.Contains(apiFinish))
			.Select(apiFinish =>
				new CardPrintFinish
				{
					CardId = card.Id,
					PrintFinishId = (int)apiFinish
				}
			);

		if (!missingFinishes.Any()) return card.CardPrintFinishes;

		var newPrintFinishes = await _cardService.CreateCardPrintFinishes(missingFinishes);

		return card.CardPrintFinishes
			.Union(newPrintFinishes);
	}

	public async Task<IEnumerable<CardGameType>> UpdateGameTypes(ApiCard apiCard)
	{
		var gameTypes = new List<CardGameType>();

		IEnumerable<Card> existingCards = await _cardService.GetFromScryfallId(apiCard.Id);
		if (!existingCards.Any()) return gameTypes;

		foreach (Card card in existingCards)
		{
			gameTypes.AddRange(await CreateMissingGameTypes(card, apiCard));
		}

		return gameTypes;
	}

	/// <summary>
	/// Creates <see cref="CardGameType"/> entities from <paramref name="apiCard"/> that are missing on <paramref name="card"/>.
	/// </summary>
	/// <returns>All <see cref="CardGameType"/> associated with <paramref name="card"/> (including newly created ones).</returns>
	private async Task<IEnumerable<CardGameType>> CreateMissingGameTypes(Card card, ApiCard apiCard)
	{
		IEnumerable<CardGameType> missingGameTypes = GameHelpers.GetMissingGameTypes(card, apiCard);

		if (!missingGameTypes.Any()) return card.GameTypes;

		var newGameTypes = await _gameService.CreateCardGameTypes(missingGameTypes);

		return card.GameTypes
			.Union(newGameTypes);
	}

	public async Task<IEnumerable<CardLegality>> UpsertLegality(ApiCard apiCard)
	{
		var upsertedLegalities = new List<CardLegality>();
		HashSet<GameFormat> gameFormats = await CreateMissingGameFormats(apiCard);
		IEnumerable<Card> existingCards = await _cardService.GetFromScryfallId(apiCard.Id);

		foreach (Card card in existingCards)
		{
			HashSet<CardLegality> legalitiesToUpsert = CardMapper.MapCardLegalities(card.Id, apiCard, gameFormats);
			upsertedLegalities.AddRange(await UpsertLegalities(card, legalitiesToUpsert));
		}

		return upsertedLegalities;
	}

	/// <summary>
	/// Creates or updates legalities associated with the <paramref name="card"/>.
	/// </summary>
	/// <returns>All created or updated <see cref="CardLegality"/> entities.</returns>
	private async Task<IEnumerable<CardLegality>> UpsertLegalities(Card card, HashSet<CardLegality> legalitiesToUpsert)
	{
		var legalitiesToUpdate = new HashSet<CardLegality>();
		var legalitiesToCreate = new HashSet<CardLegality>();
		Dictionary<(long CardId, int GameFormatId), CardLegality> existingLegalities = card.Legalities.ToDictionary(legality => (legality.CardId, legality.GameFormatId));

		foreach (CardLegality upsertLegality in legalitiesToUpsert)
		{
			if (existingLegalities.TryGetValue((upsertLegality.CardId, upsertLegality.GameFormatId), out CardLegality? existingLegality))
			{
				upsertLegality.Id = existingLegality.Id;
				legalitiesToUpdate.Add(upsertLegality);
			}
			else
			{
				legalitiesToCreate.Add(upsertLegality);
			}
		}

		IEnumerable<CardLegality>[] upsertedLegalitiesArray = await Task.WhenAll(
			_cardService.UpdateCardLegalitiesIfChanged(legalitiesToUpdate),
			_cardService.CreateCardLegalities(legalitiesToCreate)
		);

		return upsertedLegalitiesArray.SelectMany(upsertedLegalities => upsertedLegalities);
	}

	/// <summary>
	/// Creates a new <see cref="GameFormat"/> for each format not already existing.
	/// </summary>
	/// <returns>All <see cref="GameFormat"/> entities with <see cref="SourceType.Scryfall"/> as their source.</returns>
	private async Task<HashSet<GameFormat>> CreateMissingGameFormats(ApiCard apiCard)
	{
		HashSet<GameFormat> existingFormats = await _gameService.GetFormats(SourceType.Scryfall);
		HashSet<GameFormat> missingFormats = GameHelpers.GetMissingGameFormats(apiCard, existingFormats);

		if (missingFormats.Any())
		{
			await _gameService.CreateFormats(missingFormats);
		}

		return existingFormats.Union(missingFormats).ToHashSet();
	}

	public async Task<IEnumerable<Keyword>> UpsertKeywords(ApiCard apiCard)
	{
		IEnumerable<Card> existingCards = await _cardService.GetFromScryfallId(apiCard.Id);
		IEnumerable<Keyword> keywordsOnApiCard = await GetOrCreateKeywords(apiCard);

		if (!keywordsOnApiCard.Any()) return new List<Keyword>();

		IEnumerable<Keyword> keywordsOnCards = existingCards.First().CardKeywords.Select(keywordRelation => keywordRelation.Keyword);
		bool hasNewKeywords = keywordsOnApiCard.Except(keywordsOnCards, _keywordComparer).Any();
		if (!hasNewKeywords) return keywordsOnApiCard;

		foreach (Card card in existingCards)
		{
			await UpsertCardKeywords(card, keywordsOnApiCard);
		}

		return keywordsOnApiCard;
	}

	/// <summary>
	/// Creates any new keywords, then returns all <see cref="Keyword"/> entities associated with the <paramref name="apiCard"/>.
	/// </summary>
	/// <returns>Any <see cref="Keyword"/> entities associated with the <paramref name="apiCard"/>.</returns>
	private async Task<IEnumerable<Keyword>> GetOrCreateKeywords(ApiCard apiCard)
	{
		HashSet<Keyword> apiCardKeywords = CardMapper.MapKeywords(apiCard);
		IEnumerable<Keyword> allScryfallKeywords = await CreateMissingKeywords(apiCardKeywords);

		var keywordsOnCard = new HashSet<Keyword>();
		foreach (Keyword apiCardKeyword in apiCardKeywords)
		{
			Keyword? existingKeyword = allScryfallKeywords.FirstWithNameOrDefault(apiCardKeyword.Name, SourceType.Scryfall);

			if (existingKeyword is not null)
			{
				keywordsOnCard.Add(existingKeyword);
			}
		}

		return keywordsOnCard;
	}

	/// <summary>
	/// Compares all existing <see cref="Keyword"/> entities to those on the <paramref name="apiCardKeywords"/> and creates the ones missing.
	/// </summary>
	/// <returns>All <see cref="Keyword"/> entities with <see cref="SourceType.Scryfall"/> after adding any missing entries.</returns>
	private async Task<IEnumerable<Keyword>> CreateMissingKeywords(IEnumerable<Keyword> apiCardKeywords)
	{
		IEnumerable<Keyword> existingKeywords = await _cardService.GetKeywords(SourceType.Scryfall);

		IEnumerable<Keyword> missingKeywords = apiCardKeywords
			.Where(apiKeyword => !existingKeywords.ExistsWithName(apiKeyword.Name, SourceType.Scryfall));

		if (!missingKeywords.Any()) return existingKeywords;

		IEnumerable<Keyword> newKeywords = await _cardService.CreateKeywords(missingKeywords);

		return existingKeywords.Union(newKeywords);
	}

	/// <summary>
	/// Creates or updates keyword relations for the <paramref name="card"/> with the procided <see cref="Keyword"/> entities in <paramref name="keywordsOnApiCard"/>.
	/// </summary>
	/// <param name="card"></param>
	/// <param name="keywordsOnApiCard">Entities from this be get a <see cref="CardKeyword"/> relating it to the <paramref name="card"/>.</param>
	/// <returns>All the created or updated <see cref="CardKeyword"/> entities.</returns>
	private async Task<IEnumerable<CardKeyword>> UpsertCardKeywords(Card card, IEnumerable<Keyword> keywordsOnApiCard)
	{
		HashSet<CardKeyword> cardKeywordsToCreate = new();
		HashSet<CardKeyword> cardKeywordsToUpdate = new();
		HashSet<CardKeyword> apiCardKeywords = CardMapper.MapCardKeywords(card.Id, keywordsOnApiCard);

		Dictionary<(long CardId, int KeywordId), CardKeyword> existingCardKeywords = card.CardKeywords.ToDictionary(keyword => (keyword.CardId, keyword.KeywordId));

		foreach (CardKeyword cardKeyword in apiCardKeywords)
		{
			if (existingCardKeywords.TryGetValue((cardKeyword.CardId, cardKeyword.KeywordId), out CardKeyword? existingCardKeyword))
			{
				cardKeywordsToUpdate.Add(cardKeyword);
			}
			else
			{
				cardKeyword.CardId = card.Id;
				cardKeywordsToCreate.Add(cardKeyword);
			}
		}

		IEnumerable<CardKeyword>[] upsertedCards = await Task.WhenAll(
			_cardService.CreateCardKeywords(cardKeywordsToCreate),
			_cardService.UpdateCardKeywords(cardKeywordsToUpdate)
		);

		return upsertedCards.SelectMany(upsertedCardKeywords => upsertedCardKeywords);
	}

	private async Task UpsertPromoTypes(ApiCard apiCard)
	{
		if (apiCard.PromoTypes is not { Length: > 0 }) return;

		IEnumerable<PromoType> existingPromoTypes = await CreateMissingPromoTypes(apiCard);


		//Update realtions from the cards to the promotypes.
	}

	/// <summary>
	/// Compares all existing <see cref="PromoType"/> entities to those on the <paramref name="apiCard"/> and creates the ones missing.
	/// </summary>
	/// <returns>All <see cref="PromoType"/> entities with <see cref="SourceType.Scryfall"/> after adding any missing entries.</returns>
	private async Task<IEnumerable<PromoType>> CreateMissingPromoTypes(ApiCard apiCard)
	{
		if (apiCard.PromoTypes is not { Length: > 0 }) return new List<PromoType>();

		IEnumerable<PromoType> existingPromoTypes = await _cardService.GetPromoTypes(SourceType.Scryfall);

		IEnumerable<PromoType> missingPromoTypes = apiCard.PromoTypes
			.Where(promoTypeName => !existingPromoTypes.ExistsWithName(promoTypeName, SourceType.Scryfall))
			.Select(promoTypeName => new PromoType
			{
				Name = promoTypeName,
				SourceId = (int)SourceType.Scryfall
			});

		if (!missingPromoTypes.Any()) return existingPromoTypes;

		IEnumerable<PromoType> newPromoTypes = await _cardService.CreatePromoTypes(missingPromoTypes);

		return existingPromoTypes.Union(newPromoTypes);
	}
}
