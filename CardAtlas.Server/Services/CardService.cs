using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Resources.Errors;
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
		if (oldCard.Id != newCard.Id)
		{
			throw new Exception(Errors.MergingIdsAreNotEqual);
		}

		if (!_cardComparer.Equals(oldCard, newCard))
		{
			CardMapper.MergeProperties(oldCard, newCard);
			return await Update(oldCard);
		}

		return oldCard;
	}

	public Task<Card?> GetFromScryfallId(Guid scryfallId)
	{
		throw new NotImplementedException();
	}
}
