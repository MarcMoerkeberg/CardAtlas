using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Services;

public class SetService : ISetService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IEqualityComparer<Set> _setComparer;
	public SetService(
		ApplicationDbContext context,
		IEqualityComparer<Set> equalityComparer)
	{
		_dbContext = context;
		_setComparer = equalityComparer;
	}

	public async Task<Set?> GetFromScryfallId(Guid scryfallId)
	{
		if (scryfallId == Guid.Empty)
		{
			return null;
		}

		return await _dbContext.Sets
			.SingleOrDefaultAsync(set => set.ScryfallId == scryfallId);
	}

	public async Task<Set> Create(Set set)
	{
		EntityEntry<Set> savedSet = await _dbContext.Sets.AddAsync(set);
		await _dbContext.SaveChangesAsync();

		return savedSet.Entity;
	}

	public async Task<Set> Get(int setId)
	{
		return await _dbContext.Sets.SingleAsync(set => set.Id == setId);
	}

	public async Task<Set> Update(Set setWithChanges)
	{
		Set setToUpdate = await Get(setWithChanges.Id);
		SetMapper.MergeProperties(setToUpdate, setWithChanges);

		await _dbContext.SaveChangesAsync();

		return setToUpdate;
	}

	public async Task<Set> UpdateIfChanged(Set setWithChanges)
	{
		Set existingSet = await Get(setWithChanges.Id);

		if (!_setComparer.Equals(existingSet, setWithChanges))
		{
			SetMapper.MergeProperties(existingSet, setWithChanges);
			await _dbContext.SaveChangesAsync();
		}

		return existingSet;
	}
}
