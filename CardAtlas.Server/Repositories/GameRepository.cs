﻿using CardAtlas.Server.DAL;
using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Repositories;

public class GameRepository : IGameRepository
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

	public GameRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public async Task<GameFormat> Create(GameFormat format)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<GameFormat> addedGameType = await dbContext.GameFormats.AddAsync(format);
		await dbContext.SaveChangesAsync();

		return addedGameType.Entity;
	}

	public async Task<int> Create(IEnumerable<GameFormat> formats)
	{
		if (!formats.Any()) return 0;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();
		await dbContext.AddRangeAsync(formats);

		return await dbContext.SaveChangesAsync();
	}

	public async Task<IEnumerable<GameFormat>> GetFormats()
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.GameFormats
			.AsNoTracking()
			.ToHashSetAsync();
	}

	public async Task<List<GameFormat>> GetFormats(SourceType source)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.GameFormats
			.AsNoTracking()
			.Where(format => format.SourceId == (int)source)
			.ToListAsync();
	}

	public async Task<GameFormat> GetFormat(int formatId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.GameFormats
			.AsNoTracking()
			.SingleAsync(gameFormat => gameFormat.Id == formatId);
	}

	public async Task<int> Upsert(UpsertContainer<GameFormat> upsertionData)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.UpsertAsync(upsertionData);
	}
}
