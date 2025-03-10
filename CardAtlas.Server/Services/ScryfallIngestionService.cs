using CardAtlas.Server.Extensions;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;
using ScryfallApi;
using ScryfallApi.Models.Types;
using ApiCard = ScryfallApi.Models.Card;
using ApiSet = ScryfallApi.Models.Set;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Services;

public class ScryfallIngestionService : IScryfallIngestionService
{
	private readonly IScryfallApi _scryfallApi;
	private readonly IArtistService _artistService;
	private readonly ISetService _setService;
	private readonly ICardService _cardService;

	public ScryfallIngestionService(
		IScryfallApi scryfallApi,
		IArtistService artistService,
		ISetService setService,
		ICardService cardService)
	{
		_scryfallApi = scryfallApi;
		_artistService = artistService;
		_setService = setService;
		_cardService = cardService;
	}

	public async Task UpsertCardCollection()
	{
		await UpsertSets();

		foreach (ApiCard apiCard in await _scryfallApi.GetBulkCardData(BulkDataType.AllCards))
		{
			await UpsertCard(apiCard);

			UpsertImages(apiCard);
			UpsertPrices(apiCard);
			UpsertPrintFinishes(apiCard);
			UpsertGameTypes(apiCard);
			UpsertKeywords(apiCard);
			UpsertPromoTypes(apiCard);
			UpsertLegality(apiCard);
		}
	}

	public async Task UpsertSets()
	{
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
		}
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
		IEnumerable<Card> existingCard = await _cardService.GetFromScryfallId(apiCard.Id);

		return existingCard.Any() 
			? existingCard.FindMatchingCard(cardFace) 
			: null;
	}

	private void UpsertLegality(ApiCard card)
	{
		throw new NotImplementedException();
	}

	private void UpsertPromoTypes(ApiCard card)
	{
		throw new NotImplementedException();
	}

	private void UpsertKeywords(ApiCard card)
	{
		throw new NotImplementedException();
	}

	private static void UpsertGameTypes(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertPrintFinishes(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertPrices(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertImages(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}
}
