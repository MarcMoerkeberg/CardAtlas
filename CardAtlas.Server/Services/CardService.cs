using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Services;

public class CardService : ICardService
{
	private readonly IEqualityComparer<Card> _cardComparer;
	private readonly ApplicationDbContext _dbContext;
	public CardService(
		IEqualityComparer<Card> comparer,
		ApplicationDbContext dbContext)
	{
		_cardComparer = comparer;
		_dbContext = dbContext;
	}

	public async Task<Card> Get(long cardId)
	{
		return await _dbContext.Cards.SingleAsync(card => card.Id == cardId);
	}
	public async Task<Card> Create(Card card)
	{
		EntityEntry<Card> addedCard = await _dbContext.Cards.AddAsync(card);
		await _dbContext.SaveChangesAsync();

		return addedCard.Entity;
	}

	public async Task<Card> Update(Card cardWithChanges)
	{
		Card cardToUpdate = await Get(cardWithChanges.Id);
		CardMapper.MergeProperties(cardToUpdate, cardWithChanges);

		await _dbContext.SaveChangesAsync();

		return cardToUpdate;
	}

	public async Task<Card> UpdateIfChanged(Card cardWithChanges)
	{
		Card existingCard = await Get(cardWithChanges.Id);

		if (!_cardComparer.Equals(existingCard, cardWithChanges))
		{
			CardMapper.MergeProperties(existingCard, cardWithChanges);
			await _dbContext.SaveChangesAsync();
		}

		return existingCard;
	}

	public async Task<IEnumerable<Card>> GetFromScryfallId(Guid scryfallId)
	{
		return await _dbContext.Cards
			.Where(card => card.ScryfallId == scryfallId)
			.ToListAsync();
	}
}
