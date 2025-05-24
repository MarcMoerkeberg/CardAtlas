using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Repositories.Interfaces;

public interface ICardImageRepository
{
	/// <summary>
	/// Returns the <see cref="CardImage"/> from the db with the specified <paramref name="cardImageId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or more than one <see cref="CardImage"/> is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	public Task<CardImage> Get(long cardImageId);

	/// <summary>
	/// Returns the <see cref="CardImage"/> entries from the db with the specified <paramref name="cardId"/>.
	/// </summary>
	/// <returns>All <see cref="CardImage"/> entries which references <paramref name="cardId"/>. Enumerable is empty if no matches are found.</returns>
	public Task<IEnumerable<CardImage>> GetFromCardId(long cardId);

	/// <summary>
	/// Returns all <see cref="CardImage"/> entities from the db with the specified <paramref name="source"/>.
	/// </summary>
	/// <returns>All <see cref="CardImage"/> entities with the specified <paramref name="source"/>. Enumerable is empty if no matches are found.</returns>
	public Task<IEnumerable<CardImage>> Get(SourceType source);

	/// <summary>
	/// Returns all <see cref="CardImage"/> entities from the db associated with the specified <paramref name="cardIds"/>.
	/// </summary>
	/// <returns>All <see cref="CardImage"/> entities associated with the specified <paramref name="cardIds"/>. Enumerable is empty if no matches are found.</returns>
	public Task<IEnumerable<CardImage>> GetFromCardIds(IEnumerable<long> cardIds);

	/// <summary>
	/// Adds the provided <paramref name="cardImage"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="CardImage"/> with identity.</returns>
	public Task<CardImage> Create(CardImage cardImage);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="cardImageWithChanges"/>.<br/>
	/// </summary>
	/// <returns>The updated <see cref="CardImage"/>.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	public Task<CardImage> Update(CardImage cardImageWithChanges);

	/// <summary>
	/// Creates and updates <see cref="CardImage"/> entities, based on the provided <paramref name="upsertionData"/>.
	/// </summary>
	/// <returns>The number of affected effected entities.</returns>
	public Task<int> Upsert(UpsertContainer<CardImage> upsertionData);
}