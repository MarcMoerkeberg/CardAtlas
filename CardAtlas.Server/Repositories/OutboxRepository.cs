using CardAtlas.Server.DAL;
using CardAtlas.Server.Models.Entities;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Repositories;

public class OutboxRepository : IOutboxRepository
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

	public OutboxRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public async Task<List<OutboxMessage>> GetAsync()
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.OutboxMessages
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<List<OutboxMessage>> GetAsync(bool isProcessed, int maxRetries = 5)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.OutboxMessages
			.AsNoTracking()
			.Where(message =>
				message.IsProcessed == isProcessed &&
				message.RetryCount <= maxRetries
			)
			.OrderBy(message => message.CreatedDate)
			.ToListAsync();
	}

	public async Task<List<OutboxMessage>> GetAsync(bool isProcessed, int batchSize, int maxRetries = 5)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.OutboxMessages
			.AsNoTracking()
			.Where(message =>
				message.IsProcessed == isProcessed &&
				message.RetryCount <= maxRetries
			)
			.OrderBy(message => message.CreatedDate)
			.Take(batchSize)
			.ToListAsync();
	}

	public async Task<OutboxMessage> GetAsync(Guid id)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.OutboxMessages
			.AsNoTracking()
			.SingleAsync(message => message.Id == id);
	}

	public async Task<List<OutboxMessage>> GetAsync(IEnumerable<Guid> ids)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.OutboxMessages
			.AsNoTracking()
			.Where(message => ids.Contains(message.Id))
			.ToListAsync();
	}

	public async Task<OutboxMessage> CreateAsync(OutboxMessage outboxMessage)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<OutboxMessage> addedMessage = dbContext.OutboxMessages.Add(outboxMessage);
		await dbContext.SaveChangesAsync();

		return addedMessage.Entity;
	}

	public async Task<OutboxMessage> UpdateAsync(OutboxMessage outboxMessage)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<OutboxMessage> updatedMessage = dbContext.OutboxMessages.Update(outboxMessage);
		await dbContext.SaveChangesAsync();

		return updatedMessage.Entity;
	}

	public async Task<int> UpdateAsync(IEnumerable<OutboxMessage> outboxMessages)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.OutboxMessages.UpdateRange(outboxMessages);

		return await dbContext.SaveChangesAsync();
	}
}
