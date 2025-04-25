using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Repositories;

public class SetRepository : ISetRepository
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
	private readonly IEqualityComparer<Set> _setComparer;
	public SetRepository(
		IDbContextFactory<ApplicationDbContext> dbContextFactory,
		IEqualityComparer<Set> equalityComparer)
	{
		_dbContextFactory = dbContextFactory;
		_setComparer = equalityComparer;
	}

	public async Task<Set?> GetFromScryfallId(Guid scryfallId)
	{
		if (scryfallId == Guid.Empty)
		{
			return null;
		}

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Sets
			.AsNoTracking()
			.SingleOrDefaultAsync(set => set.ScryfallId == scryfallId);
	}

	public async Task<Set> Create(Set set)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<Set> savedSet = await dbContext.Sets.AddAsync(set);
		await dbContext.SaveChangesAsync();

		return savedSet.Entity;
	}

	public async Task<Set> Get(int setId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Sets
			.AsNoTracking()
			.SingleAsync(set => set.Id == setId);
	}

	public async Task<Set> Update(Set setWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		Set setToUpdate = await dbContext.Sets.SingleAsync(set => set.Id == setWithChanges.Id);
		SetMapper.MergeProperties(setToUpdate, setWithChanges);

		await dbContext.SaveChangesAsync();

		return setToUpdate;
	}

	public async Task<Set> UpdateIfChanged(Set setWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		Set existingSet = await dbContext.Sets.SingleAsync(set => set.Id == setWithChanges.Id);

		if (!_setComparer.Equals(existingSet, setWithChanges))
		{
			SetMapper.MergeProperties(existingSet, setWithChanges);
			await dbContext.SaveChangesAsync();
		}

		return existingSet;
	}
}
