using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
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

	/// <summary>
	/// Adds any missing <see cref="PrintFinishType"/> from <paramref name="apiCard"/> to it's corresponding <see cref="Card"/> entities.
	/// </summary>
	/// <returns>
	/// All <see cref="CardPrintFinish"/> associated with the <see cref="Card"/> entities found from <paramref name="apiCard"/> after updating.<br/>
	/// Reponse is empty if no <see cref="Card"/> entities are associated with the <paramref name="apiCard"/>.
	/// </returns>
	Task<IEnumerable<CardPrintFinish>> UpdatePrintFinishes(ApiCard apiCard);

	/// <summary>
	/// Adds any missing <see cref="CardGameType"/> from <paramref name="apiCard"/> to it's corresponding <see cref="Card"/> entities.
	/// </summary>
	/// <returns>All <see cref="CardGameType"/> associated with the <see cref="Card"/> entities found from <paramref name="apiCard"/> after updating.</returns>
	Task<IEnumerable<CardGameType>> UpdateGameTypes(ApiCard apiCard);

	/// <summary>
	/// Upserts card legality information from <paramref name="apiCard"/> to it's corresponding <see cref="Card"/> entities.
	/// </summary>
	/// <returns>All created or updated <see cref="CardLegality"/> entities.</returns>
	Task<IEnumerable<CardLegality>> UpsertLegality(ApiCard apiCard);

	/// <summary>
	/// Creates or updates the <see cref="Keyword"/> entities and their relation to the <see cref="Card"/> associated with the <paramref name="apiCard"/>.
	/// </summary>
	/// <returns>The <see cref="Keyword"/> entities associated with the <see cref="Card"/>.</returns>
	Task<IEnumerable<Keyword>> UpsertKeywords(ApiCard apiCard);

	/// <summary>
	/// Creates or updates <see cref="PromoType"/> entities associated with the <paramref name="apiCard"/>.<br/>
	/// Only updates if the <paramref name="apiCard"/> has new promo types not already associated with the <see cref="Card"/> (may need to be updated).
	/// </summary>
	/// <returns>All <see cref="PromoType"/> entities associated with the <paramref name="apiCard"/>.</returns>
	Task<IEnumerable<PromoType>> UpsertPromoTypes(ApiCard apiCard);
}