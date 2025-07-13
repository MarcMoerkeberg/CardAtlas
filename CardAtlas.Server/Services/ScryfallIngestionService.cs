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
using System.Threading.Channels;
using ApiCard = ScryfallApi.Models.Card;
using ApiSet = ScryfallApi.Models.Set;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Services;

public class ScryfallIngestionService : IScryfallIngestionService
{
	private const int _batchCapacity = 2000;
	private Dictionary<Guid, Set> _setLookup = new();

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

	public async Task<int> UpsertSetsAsync()
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

	public async Task UpsertCardCollectionAsync()
	{
		//TODO: Add logging (maybe a db table entry for history) in controller to see that this method was called.
		await UpsertAndCacheSetEntitiesAsync();

		var (workerChannel, persisterChannel) = CreateChannels(_batchCapacity);

		Task producer = ProduceCardsAsync(workerChannel.Writer);
		Task persister = PersistBatchesAsync(persisterChannel.Reader);
		Task[] consumers = ConsumeCardsAsync(
			reader: workerChannel.Reader,
			batchWriter: persisterChannel.Writer,
			workerCount: Math.Max(1, Environment.ProcessorCount - 1),
			_batchCapacity
		);

		try
		{
			await Task.WhenAll(producer, Task.WhenAll(consumers));
		}
		finally
		{
			persisterChannel.Writer.Complete();
		}

		await persister;
	}

	/// <summary>
	/// Creates producing, consuming and persisting data.
	/// </summary>
	/// <param name="batchCapacity">The number of <see cref="ApiCard"/> entities parsed in the worker channel, before batch is comitted to the persisting channel.</param>
	/// <returns>2 channels; One for parsing data and one for persisting data after it has been batched.</returns>
	private static (Channel<ApiCard> workerChannel, Channel<IngestionBatch> persisterChannel) CreateChannels(int batchCapacity = _batchCapacity)
	{
		var workerChannel = Channel.CreateBounded<ApiCard>(
			new BoundedChannelOptions(batchCapacity)
			{
				FullMode = BoundedChannelFullMode.Wait,
				SingleReader = false,
				SingleWriter = true,
			}
		);

		var persistingChannel = Channel.CreateUnbounded<IngestionBatch>(
			new UnboundedChannelOptions
			{
				SingleReader = true,
				SingleWriter = false,
			}
		);

		return (workerChannel, persistingChannel);
	}

	/// <summary>
	/// Adds a single task to the writer, which get's all <see cref="ApiCard"/> entities from Scryfall.<br/>
	/// Sets <paramref name="writer"/> as complete, when no more cards is available.
	/// </summary>
	/// <param name="writer">The channelwriter that should get all the card data.</param>
	/// <returns>A task which writes all available <see cref="ApiCard"/> entities to the provided channel.</returns>
	private Task ProduceCardsAsync(ChannelWriter<ApiCard> writer)
	{
		return Task.Run(async () =>
		{
			await foreach (ApiCard card in _scryfallApi.GetBulkCardDataAsync(BulkDataType.AllCards))
			{
				await writer.WriteAsync(card);
			}

			writer.Complete();
		});
	}

	/// <summary>
	/// Creaes a <see cref="Task"/> for parsing cards in the <paramref name="reader"/> channel and batching them into the <paramref name="batchWriter"/> channel.
	/// </summary>
	/// <param name="reader">Reads <see cref="ApiCard"/> entities from this channel.</param>
	/// <param name="batchWriter">Writes the parsed and batched <see cref="ApiCard"/> to the channel.</param>
	/// <param name="workerCount">Creates a task for the number of workers.</param>
	/// <param name="batchCapacity">The number of batching cycles, before writing them to <paramref name="batchWriter"/>.</param>
	/// <returns>A task which batches <see cref="ApiCard"/> entities for each <paramref name="workerCount"/>.</returns>
	private Task[] ConsumeCardsAsync(
		ChannelReader<ApiCard> reader,
		ChannelWriter<IngestionBatch> batchWriter,
		int workerCount,
		int batchCapacity = _batchCapacity)
	{
		return Enumerable.Range(0, workerCount)
			.Select(_ => Task.Run(async () =>
			{
				var batch = new IngestionBatch(_artistComparer, _gameFormatComparer, _keywordComparer, _promoTypeComparer);
				int batchCount = 0;

				await foreach (ApiCard apiCard in reader.ReadAllAsync())
				{
					IngestionBatch batchedDataOnCard = BatchCardData(apiCard);

					batch.Merge(batchedDataOnCard);
					batchCount++;

					if (batchCount >= batchCapacity)
					{
						await batchWriter.WriteAsync(batch);
						batch = new IngestionBatch(_artistComparer, _gameFormatComparer, _keywordComparer, _promoTypeComparer);
						batchCount = 0;
					}
				}

				if (batchCount > 0)
				{
					await batchWriter.WriteAsync(batch);
				}
			}))
			.ToArray();
	}

	/// <summary>
	/// Persists batched <see cref="ApiCard"/> entites to the database.
	/// </summary>
	/// <param name="reader"></param>
	/// <returns>A task that consumes all <see cref="IngestionBatch"/> entities in the provided <paramref name="reader"/> channel.</returns>
	private Task PersistBatchesAsync(ChannelReader<IngestionBatch> reader)
	{
		return Task.Run(async () =>
		{
			await foreach (IngestionBatch batch in reader.ReadAllAsync())
			{
				await PersistBatchedData(batch);
			}
		});
	}

	/// <summary>
	/// Upserts <see cref="Set"/> entities from Scryfall API and caches them in the <see cref="_setLookup"/> for batching <see cref="Card"/> entities.<br/>
	/// Should be called once before upserting <see cref="Card"/> entities.
	/// </summary>
	private async Task UpsertAndCacheSetEntitiesAsync()
	{
		await UpsertSetsAsync();
		IEnumerable<Set> allScryfallSets = await _setRepository.Get(SourceType.Scryfall);
		_setLookup = allScryfallSets
			.Where(set => set.ScryfallId.HasValue)
			.ToDictionary(set => set.ScryfallId!.Value);
	}

	/// <summary>
	/// Batches all the data from <paramref name="apiCard"/> into their respective batching dictionaries.<br/>
	/// </summary>
	private IngestionBatch BatchCardData(ApiCard apiCard)
	{
		IngestionBatch batch = new(_artistComparer, _gameFormatComparer, _keywordComparer, _promoTypeComparer);

		batch.Cards.UnionWith(BatchCards(apiCard));
		batch.Images.Union(BatchCardImages(apiCard));
		batch.CardPrices.Union(BatchCardPrices(apiCard));
		batch.CardGamePlatformRelations.Union(BatchCardGamePlatform(apiCard));
		batch.CardPrintFinishRelations.Union(BatchPrintFinishes(apiCard));

		var (artists, cardArtistRelations) = BatchArtistsAndCardRelations(apiCard);
		batch.Artists.UnionWith(artists);
		batch.CardArtistRelations.Union(cardArtistRelations);

		var (gameFormats, cardLegalities) = BatchGameFormatsAndLegalities(apiCard);
		batch.GameFormats.UnionWith(gameFormats);
		batch.CardLegalityRelations.Union(cardLegalities);

		var (keywords, cardKeywords) = BatchKeywordsAndCardRelations(apiCard);
		batch.Keywords.UnionWith(keywords);
		batch.CardKeywordRelations.Union(cardKeywords);

		var (promoTypes, cardPromoTypes) = BatchPromoTypesAndCardRelations(apiCard);
		batch.PromoTypes.UnionWith(promoTypes);
		batch.CardPromoTypeRelations.Union(cardPromoTypes);

		return batch;
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

		return mappedCards;
	}

	/// <summary>
	/// Batches <see cref="Artist"/> entities from artist information on <paramref name="apiCard"/>.<br/><br/>
	/// <see cref="Artist"/> entities are batched based with composite key of <see cref="Card.Id"/> and <see cref="Card.Name"/>.
	/// </summary>
	/// <returns>A read-only list of <see cref="Artist"/>. Returns an empty list if the <paramref name="apiCard"/> has no images yet.</returns>
	private (
		IReadOnlyList<Artist> artists,
		IReadOnlyDictionary<(Guid cardScryfallId, string cardName), List<(Guid artistScryfallId, CardArtist cardArtist)>> cardArtistRelations
		) BatchArtistsAndCardRelations(ApiCard apiCard)
	{
		List<Artist> batchedArtists = new();
		Dictionary<(Guid cardScryfallId, string cardName), List<(Guid artistScryfallId, CardArtist cardArtist)>> batchedRelations = new();

		if (apiCard.CardFaces is { Length: > 0 })
		{
			foreach (CardFace cardFace in apiCard.CardFaces)
			{
				Artist? artist = ArtistMapper.MapArtist(cardFace);
				if (artist is null) continue;

				CardArtist cardArtist = ArtistMapper.MapCardArtist(artist);

				batchedRelations[(apiCard.Id, cardFace.Name)] = new List<(Guid artistScryfallId, CardArtist cardArtist)>
				{
					(artist.ScryfallId!.Value, cardArtist)
				};

				batchedArtists.Add(artist);
			}
		}
		else
		{
			batchedArtists = ArtistMapper.MapArtists(apiCard);
			if (batchedArtists is { Count: 0 }) return (batchedArtists, batchedRelations);

			batchedRelations[(apiCard.Id, apiCard.Name)] = batchedArtists
				.Select(artist => (artist.ScryfallId!.Value, ArtistMapper.MapCardArtist(artist)))
				.ToList();
		}

		return (batchedArtists, batchedRelations);
	}

	/// <summary>
	/// Batches <see cref="CardImage"/> entities from image information on <paramref name="apiCard"/>.<br/><br/>
	/// <see cref="CardImage"/> entities are batched based with composite key of <see cref="Card.Id"/> and <see cref="Card.Name"/>.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardImage"/>. Returns an empty list if the <paramref name="apiCard"/> has no images yet.</returns>
	private IReadOnlyDictionary<(Guid cardScryfallId, string cardName), List<CardImage>> BatchCardImages(ApiCard apiCard)
	{
		Dictionary<(Guid cardScryfallId, string cardName), List<CardImage>> batchedImages = new();

		if (apiCard.CardFaces is { Length: > 0 })
		{
			foreach (CardFace cardFace in apiCard.CardFaces)
			{
				batchedImages[(apiCard.Id, cardFace.Name)] = CardImageMapper.MapCardImages(apiCard, cardFace);
			}
		}
		else
		{
			batchedImages[(apiCard.Id, apiCard.Name)] = CardImageMapper.MapCardImages(apiCard);
		}

		return batchedImages;
	}

	/// <summary>
	/// Batches <see cref="CardPrice"/> entities from price information on <paramref name="apiCard"/>.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardPrice"/>. Returns an empty list if the <paramref name="apiCard"/> has no pricing information.</returns>
	private IReadOnlyDictionary<Guid, List<CardPrice>> BatchCardPrices(ApiCard apiCard)
	{
		Dictionary<Guid, List<CardPrice>> batchedPrices = new();
		List<CardPrice> mappedPrices = CardPriceMapper.MapCardPrices(apiCard);

		if (mappedPrices is { Count: > 0 })
		{
			batchedPrices[apiCard.Id] = mappedPrices;
		}

		return batchedPrices;
	}

	/// <summary>
	/// Batches <see cref="CardGamePlatform"/> entities from game information on <paramref name="apiCard"/>.<br/><br/>
	/// <see cref="CardGamePlatform"/> represents relations between <see cref="Card"/> and <see cref="GamePlatform"/> entities.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardGamePlatform"/>. Returns an empty list if the <paramref name="apiCard"/> has no "game" information.</returns>
	private IReadOnlyDictionary<Guid, List<CardGamePlatform>> BatchCardGamePlatform(ApiCard apiCard)
	{
		Dictionary<Guid, List<CardGamePlatform>> batchedGamePlatforms = new();
		List<CardGamePlatform> mappedGamePlatforms = GameMapper.MapCardGamePlatform(apiCard);

		if (mappedGamePlatforms is { Count: > 0 })
		{
			batchedGamePlatforms[apiCard.Id] = mappedGamePlatforms;
		}

		return batchedGamePlatforms;
	}

	/// <summary>
	/// Batches <see cref="CardPrintFinish"/> entities from print information on <paramref name="apiCard"/>.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardPrintFinish"/>. Returns an empty list if the <paramref name="apiCard"/> has no print finish entries.</returns>
	private IReadOnlyDictionary<Guid, List<CardPrintFinish>> BatchPrintFinishes(ApiCard apiCard)
	{
		Dictionary<Guid, List<CardPrintFinish>> batchedPrintFinishes = new();
		List<CardPrintFinish> apiPrintFinishes = CardMapper.MapCardPrintFinishes(apiCard);

		if (apiPrintFinishes is { Count: > 0 })
		{
			batchedPrintFinishes[apiCard.Id] = apiPrintFinishes;
		}

		return batchedPrintFinishes;
	}

	/// <summary>
	/// Batches <see cref="GameFormat"/> entities and <see cref="CardLegality"/> entities.<br/><br/>
	/// <see cref="CardLegality"/> represents relations between <see cref="Card"/> and <see cref="GameFormat"/> entities."/>
	/// </summary>
	/// <param name="apiCard"></param>
	/// <returns>A read-only list of <see cref="CardLegality"/>. Returns an empty list if the <paramref name="apiCard"/> has no logality information.</returns>
	private (
		IReadOnlySet<GameFormat> gameFormats,
		IReadOnlyDictionary<Guid, List<(string formatName, CardLegality legality)>> cardLegalities
		) BatchGameFormatsAndLegalities(ApiCard apiCard)
	{
		HashSet<GameFormat> batchedGameFormats = GameMapper.MapGameFormat(apiCard).ToHashSet();
		Dictionary<Guid, List<(string formatName, CardLegality legality)>> batchedCardLegalities = new();

		List<(string formatName, CardLegality legality)> cardLegalities = CardMapper.MapCardLegalities(apiCard, batchedGameFormats);

		if (cardLegalities is { Count: > 0 })
		{
			batchedCardLegalities[apiCard.Id] = cardLegalities;
		}

		return (batchedGameFormats, batchedCardLegalities);
	}

	/// <summary>
	/// Batches <see cref="Keyword"/> entities and <see cref="CardKeyword"/> entities.<br/><br/>
	/// <see cref="CardKeyword"/> represents relations between <see cref="Card"/> and <see cref="Keyword"/> entities.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardKeyword"/>. Returns an empty list if the <paramref name="apiCard"/> has no keywords.</returns>
	private (
		IReadOnlySet<Keyword> keywords,
		IReadOnlyDictionary<Guid, List<(string keywordName, CardKeyword cardKeyword)>> cardKeywords
		) BatchKeywordsAndCardRelations(ApiCard apiCard)
	{
		HashSet<Keyword> batchedKeywords = new();
		Dictionary<Guid, List<(string keywordName, CardKeyword cardKeyword)>> batchedCardKeywords = new();
		if (apiCard.Keywords is not { Length: > 0 }) return (batchedKeywords, batchedCardKeywords);

		batchedKeywords = CardMapper.MapKeywords(apiCard).ToHashSet();
		List<(string keywordName, CardKeyword cardKeyword)> mappedCardKeywords = CardMapper.MapCardKeywords(apiCard, batchedKeywords);

		if (mappedCardKeywords is { Count: > 0 })
		{
			batchedCardKeywords[apiCard.Id] = mappedCardKeywords;
		}

		return (batchedKeywords, batchedCardKeywords);
	}

	/// <summary>
	/// Batches <see cref="PromoType"/> entities and <see cref="CardPromoType"/> entities.<br/><br/>
	/// <see cref="CardPromoType"/> represents relations between <see cref="Card"/> and <see cref="PromoType"/> entities.
	/// </summary>
	/// <returns>A read-only list of <see cref="CardPromoType"/>. Returns an empty list if the <paramref name="apiCard"/> has no promo types.</returns>
	private (
		IReadOnlySet<PromoType> promoTypes,
		IReadOnlyDictionary<Guid, List<(string promoTypeName, CardPromoType cardPromoType)>> cardPromoTypes
		) BatchPromoTypesAndCardRelations(ApiCard apiCard)
	{
		HashSet<PromoType> batchedPromoTypes = new();
		Dictionary<Guid, List<(string promoTypeName, CardPromoType cardPromoType)>> batchedCardPromoTypes = new();
		if (apiCard.PromoTypes is not { Length: > 0 }) return (batchedPromoTypes, batchedCardPromoTypes);

		batchedPromoTypes = CardMapper.MapPromoTypes(apiCard).ToHashSet();
		List<(string promoTypeName, CardPromoType cardPromoType)> promoTypesOnCard = CardMapper.MapCardPromoTypes(apiCard, batchedPromoTypes);

		if (promoTypesOnCard is { Count: > 0 })
		{
			batchedCardPromoTypes[apiCard.Id] = promoTypesOnCard;
		}

		return (batchedPromoTypes, batchedCardPromoTypes);
	}

	/// <summary>
	/// Commits all batched entities to the database.
	/// </summary>
	private async Task PersistBatchedData(IngestionBatch batch)
	{
		await Task.WhenAll(
			UpsertCards(batch),
			CreateMissingGameFormats(batch),
			CreateMissingKeywords(batch),
			CreateMissingPromoTypes(batch),
			UpsertArtists(batch)
		);

		await Task.WhenAll(
			UpsertImages(batch),
			UpsertCardPrices(batch),
			CreateMissingCardGamePlatforms(batch),
			CreateMissingCardPrintFinishes(batch),
			UpsertCardLegalities(batch),
			CreateMissingCardKeywords(batch),
			CreateMissingCardPromoTypes(batch),
			CreateMissingCardArtists(batch)
		);
	}

	/// <summary>
	/// Inserts or updates <see cref="Card"/> entities based on the current <see cref="_cardBatch"/>.<br/>
	/// After upserting, it assigns the <see cref="Card.Id"/> to the <see cref="CardImage.CardId"/> property of the current <see cref="_imageBatch"/> entities.
	/// </summary>
	/// <returns>All upserted <see cref="Card"/> entities.</returns>
	private async Task<List<Card>> UpsertCards(IngestionBatch batch)
	{
		List<Card> existingCards = await _cardRepository.Get(batch.CardScryfallIds);
		UpsertContainer<Card> upsertionData = new();
		Dictionary<(Guid scryfallId, string cardName, string? parentCardName), Card> cardLookup = existingCards.ToDictionary(card => (card.ScryfallId!.Value, card.Name, card.ParentCard?.Name));//ParentCardName is required due to cardnames not being unique as art card dfcs have the same name as the actual card.

		foreach (Card batchedCard in batch.Cards)
		{
			if (cardLookup.TryGetValue((batchedCard.ScryfallId!.Value, batchedCard.Name, batchedCard.ParentCard?.Name), out Card? existingCard))
			{
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

		List<Card> updatedCards = await _cardRepository.Get(batch.CardScryfallIds);
		batch.AssignCardIdToEntities(updatedCards);

		return updatedCards;
	}

	/// <summary>
	/// Creates <see cref="GameFormat"/> entities from <see cref="_gameFormatsBatch"/> if they do not have an existing naming match in the db.
	/// </summary>
	/// <returns>The number of added <see cref="GameFormat"/> entities.</returns>
	private async Task<int> CreateMissingGameFormats(IngestionBatch batch)
	{
		int addedFormatsCount = 0;
		List<GameFormat> existingFormats = await _gameRepository.GetFormats(SourceType.Scryfall);
		List<GameFormat> missingGameFormats = batch.GameFormats.FindMissingEntities(existingFormats);

		if (missingGameFormats.Count > 0)
		{
			addedFormatsCount = await _gameRepository.Create(missingGameFormats);
			existingFormats = await _gameRepository.GetFormats(SourceType.Scryfall);
		}

		batch.AssignGameFormatIdToEntities(existingFormats);

		return addedFormatsCount;
	}

	/// <summary>
	/// Creates <see cref="Keyword"/> entities from <see cref="_keywordsBatch"/> if they do not have an existing naming match in the db.
	/// </summary>
	/// <returns>The number of added <see cref="Keyword"/> entities.</returns>
	private async Task<int> CreateMissingKeywords(IngestionBatch batch)
	{
		int addedKeywordsCount = 0;
		List<Keyword> existingKeywords = await _cardRepository.GetKeywords(SourceType.Scryfall);
		List<Keyword> missingKeywords = batch.Keywords.FindMissingEntities(existingKeywords);

		if (missingKeywords.Count > 0)
		{
			addedKeywordsCount = await _cardRepository.Create(missingKeywords);
			existingKeywords = await _cardRepository.GetKeywords(SourceType.Scryfall);
		}

		batch.AssignKeywordIdToEntities(existingKeywords);

		return addedKeywordsCount;
	}

	/// <summary>
	/// Creates <see cref="PromoType"/> entities from <see cref="_promoTypesBatch"/> if they do not have an existing naming match in the db.
	/// </summary>
	/// <returns>The number of added <see cref="PromoType"/> entities.</returns>
	private async Task<int> CreateMissingPromoTypes(IngestionBatch batch)
	{
		int addedPromoTypesCount = 0;
		List<PromoType> existingPromoTypes = await _cardRepository.GetPromoTypes(SourceType.Scryfall);
		List<PromoType> missingPromoTypes = batch.PromoTypes.FindMissingEntities(existingPromoTypes);

		if (missingPromoTypes.Count > 0)
		{
			addedPromoTypesCount = await _cardRepository.Create(missingPromoTypes);
			existingPromoTypes = await _cardRepository.GetPromoTypes(SourceType.Scryfall);
		}

		batch.AssignPromoTypesIdToEntities(existingPromoTypes);

		return addedPromoTypesCount;
	}

	/// <summary>
	/// Inserts or updates <see cref="Artist"/> entities based on the current <see cref="_artistBatch"/>.<br/>
	/// After upserting, it assigns the <see cref="Artist.Id"/> to the <see cref="Card.ArtistId"/> property of the current <see cref="_cardBatch"/> entities.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="Artist"/> entities.</returns>
	private async Task<int> UpsertArtists(IngestionBatch batch)
	{
		int numberOfAffectedRows = 0;
		List<Artist> existingArtists = await _artistRepository.Get(batch.ArtistScryfallIds);
		UpsertContainer<Artist> upsertionData = batch.Artists
			.DistinctBy(artist => artist.ScryfallId)
			.ToUpsertData(existingArtists, _artistComparer);


		if (upsertionData.ToInsert.Count > 0 || upsertionData.ToUpdate.Count > 0)
		{
			numberOfAffectedRows = await _artistRepository.Upsert(upsertionData);

			if (upsertionData.ToInsert.Count > 0)
			{
				existingArtists = await _artistRepository.Get(batch.ArtistScryfallIds);
			}
		}

		batch.AssignArtistIdToEntities(existingArtists);

		return numberOfAffectedRows;
	}

	/// <summary>
	/// Inserts or updates <see cref="CardImage"/> entities based on the current<see cref="_imageBatch"/>.<br/>
	/// Skips any images with a <see cref="CardImage.CardId"/> of 0.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="CardImage"/> entities.</returns>
	private async Task<int> UpsertImages(IngestionBatch batch)
	{
		List<CardImage> batchedImages = batch.Images
			.Values
			.SelectMany(imageList => imageList)
			.Where(image => image.CardId != 0)
			.ToList();

		IEnumerable<CardImage> existingImages = await _cardImageRepository.GetFromCardIds(batch.CardIds);
		UpsertContainer<CardImage> upsertionData = batchedImages.ToUpsertData(existingImages, _imageComparer);

		int upsertedCount = await _cardImageRepository.Upsert(upsertionData);

		return upsertedCount;
	}

	/// <summary>
	/// Inserts or updates <see cref="CardPrice"/> entities based on the current<see cref="_cardPriceBatch"/>.<br/>
	/// Skips any images with a <see cref="CardPrice.CardId"/> of 0.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="CardPrice"/> entities.</returns>
	private async Task<int> UpsertCardPrices(IngestionBatch batch)
	{
		List<CardPrice> batchedPrices = batch.CardPrices
			.Values
			.SelectMany(cardPriceList => cardPriceList)
			.Where(cardPrice => cardPrice.CardId != 0)
			.ToList();

		IEnumerable<CardPrice> existingCardPrices = await _cardRepository.GetCardPrices(batch.CardIds);
		UpsertContainer<CardPrice> upsertionData = batchedPrices.ToUpsertData(existingCardPrices, _priceComparer);

		int upsertedCount = await _cardRepository.Upsert(upsertionData);

		return upsertedCount;
	}

	/// <summary>
	/// Adds all missing <see cref="CardGamePlatform"/> entities to the database from <see cref="_cardGamePlatformBatch"/>.<br/>
	/// <see cref="CardGamePlatform"/> represents the relationship between <see cref="GamePlatform"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardGamePlatform"/> entities.</returns>
	private async Task<int> CreateMissingCardGamePlatforms(IngestionBatch batch)
	{
		IEnumerable<CardGamePlatform> existingPlatforms = await _cardRepository.GetCardGamePlatforms(batch.CardIds);
		List<CardGamePlatform> missingPlatforms = batch.CardGamePlatformRelations.FindMissingEntities(
			existingEntities: existingPlatforms,
			filterExistingEntities: cgp => (cgp.GamePlatformId, cgp.CardId)
		);

		return await _cardRepository.Create(missingPlatforms);
	}

	/// <summary>
	/// Adds all missing <see cref="CardPrintFinish"/> entities to the database from <see cref="_cardPrintFinishBatch"/>.<br/>
	/// <see cref="CardPrintFinish"/> represents the relationship between <see cref="PrintFinish"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardPrintFinish"/> entities.</returns>
	private async Task<int> CreateMissingCardPrintFinishes(IngestionBatch batch)
	{
		IEnumerable<CardPrintFinish> existingPrintFinishes = await _cardRepository.GetCardPrintFinishes(batch.CardIds);
		List<CardPrintFinish> missingPrintFinishes = batch.CardPrintFinishRelations.FindMissingEntities(
			existingEntities: existingPrintFinishes,
			filterExistingEntities: cpf => (cpf.PrintFinishId, cpf.CardId)
		);

		return await _cardRepository.Create(missingPrintFinishes);
	}

	/// <summary>
	/// Inserts or updates <see cref="CardLegality"/> entities based on the current <see cref="_cardLegalitiesBatch"/>.<br/>
	/// Skips any images with a <see cref="CardPrice.CardId"/> of 0.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="CardLegality"/> entities.</returns>
	private async Task<int> UpsertCardLegalities(IngestionBatch batch)
	{
		List<CardLegality> batchedCardLegalities = batch.CardLegalityRelations
			.Values
			.SelectMany(cardLegalityTupleList => cardLegalityTupleList)
			.Select(tuple => tuple.legality)
			.Where(cardLegality => cardLegality.CardId != 0 && cardLegality.GameFormatId != 0)
			.ToList();

		List<CardLegality> existingCardLegalities = await _cardRepository.GetCardLegalities(batch.CardIds);
		UpsertContainer<CardLegality> upsertionData = batchedCardLegalities.ToUpsertData(existingCardLegalities, _cardLegalityComparer);

		return await _cardRepository.Upsert(upsertionData); ;
	}

	/// <summary>
	/// Adds all missing <see cref="CardKeyword"/> entities to the database from <see cref="_cardKeywordsBatch"/>.<br/>
	/// <see cref="CardKeyword"/> represents the relationship between <see cref="Keyword"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardKeyword"/> entities.</returns>
	private async Task<int> CreateMissingCardKeywords(IngestionBatch batch)
	{
		List<CardKeyword> existingCardKeywords = await _cardRepository.GetCardKeywords(batch.CardIds);

		List<CardKeyword> missingCardKeywords = batch.CardKeywordRelations.FindMissingEntities(
			existingEntities: existingCardKeywords,
			omitDefaultValues: ck => ck.KeywordId != 0 && ck.CardId != 0,
			filterExistingEntities: ck => (ck.KeywordId, ck.CardId)
		);

		return await _cardRepository.Create(missingCardKeywords);
	}

	/// <summary>
	/// Adds all missing <see cref="CardPromoType"/> entities to the database from <see cref="_cardPromoTypesBatch"/>.<br/>
	/// <see cref="CardPromoType"/> represents the relationship between <see cref="PromoType"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardKeyword"/> entities.</returns>
	private async Task<int> CreateMissingCardPromoTypes(IngestionBatch batch)
	{
		List<CardPromoType> existingCardPromoTypes = await _cardRepository.GetCardPromoTypes(batch.CardIds);

		List<CardPromoType> missingCardPromoTypes = batch.CardPromoTypeRelations.FindMissingEntities(
			existingEntities: existingCardPromoTypes,
			omitDefaultValues: cpt => cpt.PromoTypeId != 0 && cpt.CardId != 0,
			filterExistingEntities: cpt => (cpt.PromoTypeId, cpt.CardId)
		);

		return await _cardRepository.Create(missingCardPromoTypes);
	}

	/// <summary>
	/// Adds all missing <see cref="CardArtist"/> entities to the database from <see cref="_cardArtistBatch"/>.<br/>
	/// <see cref="CardArtist"/> represents the relationship between <see cref="Artist"/> and <see cref="Card"/> entities.
	/// </summary>
	/// <returns>The number of added <see cref="CardArtist"/> entities.</returns>
	private async Task<int> CreateMissingCardArtists(IngestionBatch batch)
	{
		IEnumerable<CardArtist> existingCardArtists = await _cardRepository.GetCardArtists(batch.CardIds);
		IEnumerable<CardArtist> batchedCardArtists = batch.CardArtistRelations
			.SelectMany(kvp => kvp.Value)
			.Select(tuple => tuple.cardArtist);

		List<CardArtist> missingCardArtists = batchedCardArtists.FindMissingEntities(
			existingEntities: existingCardArtists,
			filterExistingEntities: cardArtist => (cardArtist.CardId, cardArtist.ArtistId)
		);

		return await _cardRepository.Create(missingCardArtists);
	}
}
