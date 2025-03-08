using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces;

public interface ICardService
{
	public Task<Card> Get(long cardId);
	public Task<Card> Create(Card card);
	public Task<Card> Update(Card card);

	/// <summary>
	/// Updates <paramref name="oldCard"/> with changes from <paramref name="newCard"/>.<br/>
	/// Does not update the <paramref name="oldCard"/> if there are no changes.
	/// </summary>
	/// <returns><paramref name="oldCard"/> updated with changes from <paramref name="newCard"/>.</returns>
	/// <exception cref="Exception">Is thrown if the ids on both cards do not match.</exception>
	public Task<Card> Merge(Card oldCard, Card newCard);

	public Task<Card?> GetFromScryfallId(Guid scryfallId);
}
