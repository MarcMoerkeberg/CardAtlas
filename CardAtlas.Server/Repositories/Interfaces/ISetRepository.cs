using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Repositories.Interfaces;

public interface ISetRepository
{
	/// <summary>
	/// Returns the <see cref="Set"/> from the db with the specified <paramref name="scryfallId"/>.
	/// </summary>
	/// <returns>The <see cref="Set"/> with the specified <paramref name="scryfallId"/> or null if no match is found.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
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
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Set> Update(Set setWithChanges);

	/// <summary>
	/// Returns the <see cref="Set"/> from the db with the specified <paramref name="setId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="Set"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Set> Get(int setId);

	/// <summary>
	/// Returns the <see cref="Set"/> entities from the db with the specified <paramref name="source"/>.<br/>
	/// </summary>
	Task<Set> Get(SourceType source);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="setWithChanges"/>.<br/>
	/// Does not update the if there are no changes.
	/// </summary>
	/// <returns>The updated <see cref="Set"/>.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Set> UpdateIfChanged(Set setWithChanges);

	/// <summary>
	/// Returns the <see cref="Set"/> entities from the db with the which has a matching in <paramref name="scryfallIds"/>.
	/// </summary>
	/// <returns>The <see cref="Set"/> entities which has a match in <paramref name="scryfallIds"/> or empty if no matches is found or inpput is empty.</returns>
	Task<IEnumerable<Set>> GetFromScryfallIds(IEnumerable<Guid> scryfallIds);

	/// <summary>
	/// Adds the provided <paramref name="sets"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="Set"/> entities with identity.</returns>
	Task<IEnumerable<Set>> Create(IEnumerable<Set> sets);

	/// <summary>
	/// Updates existing entities with changes from <paramref name="setsWithChanges"/>.<br/>
	/// Only updates the entities with an Id.
	/// </summary>
	/// <returns>The updated <see cref="Set"/> entities.</returns>
	Task<IEnumerable<Set>> Update(IEnumerable<Set> setsWithChanges);

	/// <summary>
	/// Creates and updates <see cref="Set"/> entities, based on the provided <paramref name="upsertionData"/>.
	/// </summary>
	/// <returns>The number of affected effected entities.</returns>
	Task<int> Upsert(UpsertContainer<Set> upsertionData);
}