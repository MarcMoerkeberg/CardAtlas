using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Resources.Errors;
using CardAtlas.Server.Services.Interfaces;

namespace CardAtlas.Server.Services;

public class SetService : ISetService
{
	private readonly IEqualityComparer<Set> _setComparer;
	public SetService(IEqualityComparer<Set> equalityComparer)
	{
		_setComparer = equalityComparer;
	}


	public Task<Set?> GetFromScryfallId(Guid scryfallId)
	{
		throw new NotImplementedException();
	}

	public Task<Set> Create(Set set)
	{
		throw new NotImplementedException();
	}

	public Task<Set> Get(int setId)
	{
		throw new NotImplementedException();
	}

	public Task<Set> Update(Set set)
	{
		throw new NotImplementedException();
	}

	public async Task<Set> Merge(Set oldSet, Set newSet)
	{
		if (oldSet.Id != newSet.Id)
		{
			throw new Exception(Errors.MergingIdsAreNotEqual);
		}

		if (!_setComparer.Equals(oldSet, newSet))
		{
			SetMapper.MergeProperties(oldSet, newSet);
			return await Update(oldSet);
		}

		return oldSet;
	}
}
