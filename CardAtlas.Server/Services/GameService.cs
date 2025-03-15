using CardAtlas.Server.DAL;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Services;

public class GameService : IGameService
{
	private readonly ApplicationDbContext _dbContext;
	public GameService(
		ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IEnumerable<CardGameType>> CreateCardGameTypes(IEnumerable<CardGameType> cardGameTypes)
	{
		var addedCardGameTypes = new List<CardGameType>();

		foreach (CardGameType cardGameType in cardGameTypes)
		{
			EntityEntry<CardGameType> addedCardPrintFinish = await _dbContext.CardGameTypes.AddAsync(cardGameType);
			addedCardGameTypes.Add(addedCardPrintFinish.Entity);
		}

		await _dbContext.SaveChangesAsync();

		return addedCardGameTypes;
	}

	public async Task<GameFormat> CreateFormat(GameFormat format)
	{
		EntityEntry<GameFormat> addedGameType = await _dbContext.GameFormats.AddAsync(format);
		await _dbContext.SaveChangesAsync();

		return addedGameType.Entity;
	}

	public async Task<IEnumerable<GameFormat>> CreateFormats(IEnumerable<GameFormat> formats)
	{
		var addedFormats = new List<GameFormat>();
		foreach (var format in formats)
		{
			EntityEntry<GameFormat> addedGameType = await _dbContext.GameFormats.AddAsync(format);
			addedFormats.Add(addedGameType.Entity);
		}

		await _dbContext.SaveChangesAsync();

		return addedFormats;
	}

	public async Task<IEnumerable<GameFormat>> GetFormats()
	{
		return await _dbContext.GameFormats.ToHashSetAsync();
	}

	public async Task<HashSet<GameFormat>> GetFormats(SourceType source)
	{
		return await _dbContext.GameFormats
			.Where(format => format.SourceId == (int)source)
			.ToHashSetAsync();
	}

	public async Task<GameFormat> GetFormat(int formatId)
	{
		return await _dbContext.GameFormats
			.SingleAsync(gameFormat => gameFormat.Id == formatId);
	}
}
