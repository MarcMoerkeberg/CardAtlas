using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces;

public interface ICardService
{
	public Task<Card> Get(long cardId);
	public Task<Card> Create(Card card);
	public Task<Card> Update(Card card);
	public Task<Card> Merge(Card oldCard, Card newCard);
	public Task<Card?> GetFromScryfallId(Guid scryfallId);
}
