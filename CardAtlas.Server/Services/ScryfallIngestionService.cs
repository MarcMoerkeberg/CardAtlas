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

		await foreach (ApiCard apiCard in _scryfallApi.GetBulkCardDataAsync(BulkDataType.AllCards))
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
			Card? existingCard = await _cardService.GetFromScryfallId(apiCard.Id);

			Card mappedCard = CardMapper.MapCard(apiCard, set, artist, cardFace);
			mappedCard.ParentCardId = parentId;

			if (existingCard is null)
			{
				cards.Add(await _cardService.Create(mappedCard));
			}
			else
			{
				cards.Add(await _cardService.Update(mappedCard));
			}

			if (isFirstCardFace)
			{
				parentId = cards.First().Id;
				isFirstCardFace = false;
			}
		}

		return cards;
	}

	private async Task UpsertSets()
	{
		throw new NotImplementedException();
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
		Card? existingCard = await _cardService.GetFromScryfallId(apiCard.Id);

		return existingCard is null
			? await _cardService.Create(mappedCard)
			: await _cardService.Update(mappedCard);
	}

	/// <summary>
	/// Gets the <see cref="Set""/> or creates a new if no matching set is found.<br/>
	/// Uses <see cref="IScryfallApi"/> to fetch set data if no match is found.
	/// </summary>
	/// <returns>A <see cref="Set"/> from th database.</returns>
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
	/// <returns>An <see cref="Artist"/> from the database.</returns>
	private async Task<Artist> GetOrCreateArtist(ApiCard apiCard, CardFace? cardFace = null)
	{
		Artist artistFromCard = cardFace is not null
			? ArtistMapper.MapArtist(cardFace)
			: ArtistMapper.MapArtist(apiCard);

		return artistFromCard.ScryfallId.HasValue
			? await _artistService.GetFromScryfallId(artistFromCard.ScryfallId.Value) ?? await _artistService.Create(artistFromCard)
			: await _artistService.Get(Artist.DefaultArtistId);
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
