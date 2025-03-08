using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces;

public interface ISetService
{
	Task<Set?> GetFromScryfallId(Guid scryfallId);
	Task<Set> Create(Set set);
	Task<Set> Update(Set set);
	Task<Set> Get(int setId);

	/// <summary>
	/// Updates <paramref name="oldSet"/> with changes from <paramref name="newSet"/>.<br/>
	/// Does not update the <paramref name="oldSet"/> if there are no changes.
	/// </summary>
	/// <returns><paramref name="oldSet"/> updated with changes from <paramref name="newSet"/>.</returns>
	/// <exception cref="Exception">Is thrown if the ids on both cards do not match.</exception>
	Task<Set> Merge(Set oldSet, Set newSet);
}