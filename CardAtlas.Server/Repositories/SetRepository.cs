using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Internal;
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

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Sets
			.AsNoTracking()
			.SingleOrDefaultAsync(set => set.ScryfallId == scryfallId);
	}

	public async Task<IEnumerable<Set>> GetFromScryfallIds(IEnumerable<Guid> scryfallIds)
	{
		if (!scryfallIds.Any()) return Enumerable.Empty<Set>();

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Sets
			.AsNoTracking()
			.Where(set => set.ScryfallId.HasValue && scryfallIds.Contains(set.ScryfallId.Value))
			.ToListAsync();
	}

	public async Task<Set> Create(Set set)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<Set> savedSet = await dbContext.Sets.AddAsync(set);
		await dbContext.SaveChangesAsync();

		return savedSet.Entity;
	}

	public async Task<IEnumerable<Set>> Create(IEnumerable<Set> sets)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		await dbContext.Sets.AddRangeAsync(sets);
		await dbContext.SaveChangesAsync();

		return sets;
	}

	public async Task<Set> Get(int setId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Sets
			.AsNoTracking()
			.SingleAsync(set => set.Id == setId);
	}

	public async Task<IEnumerable<Set>> Get(SourceType source)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Sets
			.Include(set => set.Source)
			.AsNoTracking()
			.Where(set => set.SourceType == source)
			.ToListAsync();
	}

	public async Task<Set> Update(Set setWithChanges)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		Set setToUpdate = await dbContext.Sets.SingleAsync(set => set.Id == setWithChanges.Id);
		SetMapper.MergeProperties(setToUpdate, setWithChanges);

		await dbContext.SaveChangesAsync();

		return setToUpdate;
	}

	public async Task<IEnumerable<Set>> Update(IEnumerable<Set> setsWithChanges)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		dbContext.Sets.UpdateRange(setsWithChanges);
		await dbContext.SaveChangesAsync();

		return setsWithChanges.Where(set => set.Id != 0);
	}

	public async Task<Set> UpdateIfChanged(Set setWithChanges)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		Set existingSet = await dbContext.Sets.SingleAsync(set => set.Id == setWithChanges.Id);

		if (!_setComparer.Equals(existingSet, setWithChanges))
		{
			SetMapper.MergeProperties(existingSet, setWithChanges);
			await dbContext.SaveChangesAsync();
		}

		return existingSet;
	}

	public async Task<int> Upsert(UpsertContainer<Set> upsertionData)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		if (upsertionData.ToInsert is { Count: > 0 }) dbContext.Sets.AddRange(upsertionData.ToInsert);
		if (upsertionData.ToUpdate is { Count: > 0 }) dbContext.Sets.UpdateRange(upsertionData.ToUpdate);

		int numberOfAffectedRows = await dbContext.SaveChangesAsync();

		return numberOfAffectedRows;
	}
}
