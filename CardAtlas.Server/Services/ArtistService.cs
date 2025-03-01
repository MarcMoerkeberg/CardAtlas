using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;

namespace CardAtlas.Server.Services;

public class ArtistService : IArtistService
{
	public ArtistService()
	{
	}

	public Task<Artist?> GetFromScryfallId(Guid scryfallId)
	{
		throw new NotImplementedException();
	}

	public Task<Artist> Create(Artist artist)
	{
		throw new NotImplementedException();
	}
	
	public Task<Artist> Get(int artistId)
	{
		throw new NotImplementedException();
	}
}
