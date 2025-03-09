using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces;

public interface ISetService
{
	/// <summary>
	/// Returns the <see cref="Set"/> from the db with the specified <paramref name="scryfallId"/>.
	/// </summary>
	/// <returns>The <see cref="Set"/> with the specified <paramref name="scryfallId"/> or null if no match is found.</returns>
	Task<Set?> GetFromScryfallId(Guid scryfallId);

	/// <summary>
	/// Adds the provided <paramref name="set"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="Set"/> with identity.</returns>
	Task<Set> Create(Set set);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="setWithChanges"/>.<br/>
	/// </summary>
	/// <returns>The updated <see cref="Set"/>.</returns>
	Task<Set> Update(Set setWithChanges);

	/// <summary>
	/// Returns the <see cref="Set"/> from the db with the specified <paramref name="setId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="Set"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Set> Get(int setId);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="setWithChanges"/>.<br/>
	/// Does not update the if there are no changes.
	/// </summary>
	/// <returns>The updated <see cref="Set"/>.</returns>
	Task<Set> UpdateIfChanged(Set setWithChanges);
}