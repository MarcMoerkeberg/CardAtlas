using CardAtlas.Server.DAL;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
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

	public async Task<IEnumerable<CardGameTypeAvailability>> CreateCardGameTypes(IEnumerable<CardGameTypeAvailability> cardGameTypes)
	{
		var addedCardGameTypes = new List<CardGameTypeAvailability>();
		if (!cardGameTypes.Any()) return addedCardGameTypes;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (CardGameTypeAvailability cardGameType in cardGameTypes)
		{
			EntityEntry<CardGameTypeAvailability> addedCardPrintFinish = await dbContext.CardGameTypes.AddAsync(cardGameType);
			addedCardGameTypes.Add(addedCardPrintFinish.Entity);
		}

		await dbContext.SaveChangesAsync();

		return addedCardGameTypes;
	}

	public async Task<GameFormat> CreateFormat(GameFormat format)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<GameFormat> addedGameType = await dbContext.GameFormats.AddAsync(format);
		await dbContext.SaveChangesAsync();

		return addedGameType.Entity;
	}

	public async Task<IEnumerable<GameFormat>> CreateFormats(IEnumerable<GameFormat> formats)
	{
		var addedFormats = new List<GameFormat>();
		if (!formats.Any()) return addedFormats;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var format in formats)
		{
			EntityEntry<GameFormat> addedGameType = await dbContext.GameFormats.AddAsync(format);
			addedFormats.Add(addedGameType.Entity);
		}

		await dbContext.SaveChangesAsync();

		return addedFormats;
	}

	public async Task<IEnumerable<GameFormat>> GetFormats()
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.GameFormats
			.AsNoTracking()
			.ToHashSetAsync();
	}

	public async Task<HashSet<GameFormat>> GetFormats(SourceType source)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.GameFormats
			.AsNoTracking()
			.Where(format => format.SourceId == (int)source)
			.ToHashSetAsync();
	}

	public async Task<GameFormat> GetFormat(int formatId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.GameFormats
			.AsNoTracking()
			.SingleAsync(gameFormat => gameFormat.Id == formatId);
	}
}
