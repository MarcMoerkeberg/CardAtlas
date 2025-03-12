using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Cards;
using CardAtlas.Server.Models.Data.Image;
using ApiCard = ScryfallApi.Models.Card;

namespace CardAtlas.Server.Services.Interfaces;

public interface IScryfallIngestionService
{
	Task UpsertCardCollection();

	Task<int> UpsertSets();

	/// <summary>
	/// Upserts data from <paramref name="apiCard"/> into the database.<br/>
	/// If <paramref name="apiCard"/> has multiple <see cref="ScryfallApi.Models.CardFace"/>, each face will be upserted as a separate card with a parent/child relationship.
	/// </summary>
	/// <param name="apiCard"></param>
	/// <returns>Each <see cref="Card"/> that has been created or updated.</returns>
	Task<IEnumerable<Card>> UpsertCard(ApiCard apiCard);

	/// <summary>
	/// Upserts the imagery from <paramref name="apiCard"/> to it's corresponding <see cref="Card"/>.<br/>
	/// If <paramref name="apiCard"/> has multiple <see cref="ScryfallApi.Models.CardFace"/>, each face's corresponding <see cref="Card"/> will updated.
	/// </summary>
	/// <returns>All <see cref="CardImage"/> entries which was created or updated.</returns>
	Task<IEnumerable<CardImage>> UpsertCardImages(ApiCard apiCard);

	/// <summary>
	/// Upserts the pricing data from <paramref name="apiCard"/> to it's corresponding <see cref="Card"/>.<br/>
	/// If <paramref name="apiCard"/> has multiple <see cref="ScryfallApi.Models.CardFace"/>, each face's corresponding <see cref="Card"/> will updated.
	/// </summary>
	/// <returns>All <see cref="CardPrice"/> entries which was created or updated.</returns>
	Task<IEnumerable<CardPrice>> UpsertCardPrices(ApiCard apiCard);
}