using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;

namespace CardAtlas.Server.Services;

public class CardService : ICardService
{
	private readonly IEqualityComparer<Card> _cardComparer;
	public CardService(IEqualityComparer<Card> comparer)
	{
		_cardComparer = comparer;
	}

	public Task<Card> Get(long cardId)
	{
		throw new NotImplementedException();
	}
	public Task<Card> Create(Card card)
	{
		throw new NotImplementedException();
	}

	public Task<Card> Update(Card card)
	{
		throw new NotImplementedException();
	}

	public async Task<Card> Merge(Card oldCard, Card newCard)
	{
		if(_cardComparer.Equals(oldCard, newCard))
		{
			return await Update(newCard);
		}
		
		return oldCard;
	}

	public Task<Card?> GetFromScryfallId(Guid scryfallId)
	{
		throw new NotImplementedException();
	}
}
