using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces;

public interface ICardService
{
	/// <summary>
	/// Returns the <see cref="Card"/> from the db with the specified <paramref name="cardId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="Card"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	public Task<Card> Get(long cardId);

	/// <summary>
	/// Adds the provided <paramref name="card"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="Card"/> with identity.</returns>
	public Task<Card> Create(Card card);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="cardWithChanges"/>.<br/>
	/// </summary>
	/// <returns>The updated <see cref="Card"/>.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	public Task<Card> Update(Card cardWithChanges);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="cardWithChanges"/>.<br/>
	/// Does not update the if there are no changes.
	/// </summary>
	/// <returns>The updated <see cref="Card"/>.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	public Task<Card> UpdateIfChanged(Card cardWithChanges);

	/// <summary>
	/// Returns the <see cref="Card"/> entries from the db with the specified <paramref name="scryfallId"/>.<br/>
	/// Multiple cards may have the same scryfallId, since they are created as seperate card instances if they have multiple <see cref="ScryfallApi.Models.CardFace"/> (such as flip or split cards).
	/// </summary>
	/// <returns>The <see cref="Card"/> with the specified <paramref name="scryfallId"/> or null if no match is found.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	public Task<IEnumerable<Card>> GetFromScryfallId(Guid scryfallId);
}
