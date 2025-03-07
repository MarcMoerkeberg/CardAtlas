using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;

namespace CardAtlas.Server.Services;

public class CardService : ICardService
{
	public Task<Card> Create(Card card)
	{
		throw new NotImplementedException();
	}
	
	public Task<Card> Update(Card card)
	{
		throw new NotImplementedException();
	}
	
	public Task<Card> Get(long cardId)
	{
		throw new NotImplementedException();
	}
	
	public Task<Card?> GetFromScryfallId(Guid scryfallId)
	{
		throw new NotImplementedException();
	}
}
