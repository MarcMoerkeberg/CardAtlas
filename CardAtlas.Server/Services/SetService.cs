using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;

namespace CardAtlas.Server.Services;

public class SetService : ISetService
{
	public SetService()
	{
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
}
