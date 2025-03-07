using CardAtlas.Server.Models.Data;
using ApiCard = ScryfallApi.Models.Card;

namespace CardAtlas.Server.Services.Interfaces;

public interface IScryfallIngestionService
{
	Task UpsertCardCollection();
	
	/// <summary>
	/// Upserts data from <paramref name="apiCard"/> into the database.<br/>
	/// If <paramref name="apiCard"/> has multiple <see cref="ScryfallApi.Models.CardFace"/>, each face will be upserted as a separate card with a parent/child relationship.
	/// </summary>
	/// <param name="apiCard"></param>
	/// <returns>Each <see cref="Card"/> that has been created or updated.</returns>
	Task<IEnumerable<Card>> UpsertCard(ApiCard apiCard);
}