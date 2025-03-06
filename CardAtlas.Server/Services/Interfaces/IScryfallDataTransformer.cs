using CardAtlas.Server.Models.Data;
using ApiCard = ScryfallApi.Models.Card;

namespace CardAtlas.Server.Services.Interfaces
{
	public interface IScryfallDataTransformer
	{
		Task<Card> UpsertCard(ApiCard apiCard);
	}
}