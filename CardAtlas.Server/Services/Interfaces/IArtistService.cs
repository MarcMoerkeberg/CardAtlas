using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces
{
	public interface IArtistService
	{
		Task<Artist> Create(Artist artist);
		Task<Artist> Get(int artistId);
		Task<Artist?> GetFromScryfallId(Guid scryfallId);
	}
}