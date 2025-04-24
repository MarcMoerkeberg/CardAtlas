using CardAtlas.Server.DAL;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Repositories;

public class ArtistRepository : IArtistRepository
{
	private readonly ApplicationDbContext _dbContext;
	public ArtistRepository(ApplicationDbContext context)
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
