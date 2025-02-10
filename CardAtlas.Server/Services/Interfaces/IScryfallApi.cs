using CardAtlas.Server.Models.Scryfall;

namespace CardAtlas.Server.Services.Interfaces
{
	public interface IScryfallApi
	{
		public Task<ScryfallCard[]> GetAllCardsAsync();
	}
}
