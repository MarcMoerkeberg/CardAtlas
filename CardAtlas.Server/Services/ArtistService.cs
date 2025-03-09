using CardAtlas.Server.DAL;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Services;

public class ArtistService : IArtistService
{
	private readonly ApplicationDbContext _dbContext;
	public ArtistService(ApplicationDbContext context)
	{
		_dbContext = context;
	}

	public async Task<Artist?> GetFromScryfallId(Guid scryfallId)
	{
		if (scryfallId == Guid.Empty)
		{
			return null;
		}

		return await _dbContext.Artists
			.SingleOrDefaultAsync(artist => artist.ScryfallId == scryfallId);
	}

	public async Task<Artist> Create(Artist artist)
	{
		EntityEntry<Artist> addedArtist = await _dbContext.Artists.AddAsync(artist);
		await _dbContext.SaveChangesAsync();

		return addedArtist.Entity;
	}

	public async Task<Artist> Get(int artistId)
	{
		return await _dbContext.Artists.SingleAsync(artist => artist.Id == artistId);
	}
}
