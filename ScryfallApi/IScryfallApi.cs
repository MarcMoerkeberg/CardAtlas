using ScryfallApi.Scryfall;

namespace ScryfallApi
{
	public interface IScryfallApi
	{
		public Task<ScryfallCard[]> GetAllCardsAsync();
	}
}
