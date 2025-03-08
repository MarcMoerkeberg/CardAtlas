using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces;

public interface ISetService
{
	public Task<Set?> GetFromScryfallId(Guid scryfallId);
	public Task<Set> Create(Set set);
	public Task<Set> Update(Set set);
	public Task<Set> Get(int setId);
}