using CardAtlas.Server.DAL;
using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Repositories;

public class ArtistRepository : IArtistRepository
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

	public ArtistRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Artist?> Get(Guid scryfallId)
	{
		if (scryfallId == Guid.Empty)
		{
			return null;
		}

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Artists
			.AsNoTracking()
			.SingleOrDefaultAsync(artist => artist.ScryfallId == scryfallId);
	}

	public async Task<Artist> Create(Artist artist)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<Artist> addedArtist = await dbContext.Artists.AddAsync(artist);
		await dbContext.SaveChangesAsync();

		return addedArtist.Entity;
	}

	public async Task<Artist> Get(int artistId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Artists
			.AsNoTracking()
			.SingleAsync(artist => artist.Id == artistId);
	}

	public async Task<IEnumerable<Artist>> Get(IEnumerable<Guid> scryfallIds)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Artists
			.AsNoTracking()
			.Where(artist => artist.ScryfallId.HasValue &&
				scryfallIds.Contains(artist.ScryfallId.Value))
			.ToListAsync();
	}

	public async Task<int> Upsert(UpsertContainer<Artist> upsertionData)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.UpsertAsync(upsertionData);
	}
}
