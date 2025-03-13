using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Cards;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Services;

public class CardService : ICardService
{
	private readonly IEqualityComparer<Card> _cardComparer;
	private readonly IEqualityComparer<CardPrice> _cardPriceComparer;
	private readonly ApplicationDbContext _dbContext;
	public CardService(
		IEqualityComparer<Card> comparer,
		IEqualityComparer<CardPrice> cardPriceComparer,
		ApplicationDbContext dbContext)
	{
		_cardComparer = comparer;
		_cardPriceComparer = cardPriceComparer;
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
			.Include(card => card.Images)
			.Include(card => card.Set)
			.Include(card => card.Prices)
			.Include(card => card.CardPrintFinishes)
			.Where(card => card.ScryfallId == scryfallId)
			.ToListAsync();
	}

	public async Task<CardPrice> GetPrice(long priceId)
	{
		return await _dbContext.CardPrices.SingleAsync(price => price.Id == priceId);
	}

	public async Task<IEnumerable<CardPrice>> GetPrices(long cardId)
	{
		return await _dbContext.CardPrices
			.Where(price => price.CardId == cardId)
			.ToListAsync();
	}

	public async Task<CardPrice> CreatePrice(CardPrice priceToUpsert)
	{
		EntityEntry<CardPrice> addedPrice = await _dbContext.CardPrices.AddAsync(priceToUpsert);
		await _dbContext.SaveChangesAsync();

		return addedPrice.Entity;
	}

	public async Task<CardPrice> UpdatePrice(CardPrice priceToUpdate)
	{
		CardPrice existingPrice = await GetPrice(priceToUpdate.Id);
		CardPriceMapper.MergeProperties(existingPrice, priceToUpdate);
		await _dbContext.SaveChangesAsync();

		return existingPrice;
	}

	public async Task<CardPrice> UpdatePriceIfChanged(CardPrice priceToUpdate)
	{
		CardPrice existingPrice = await GetPrice(priceToUpdate.Id);

		if(!_cardPriceComparer.Equals(existingPrice, priceToUpdate))
		{
			CardPriceMapper.MergeProperties(existingPrice, priceToUpdate);
			await _dbContext.SaveChangesAsync();
		}

		return existingPrice;
	}

	public async Task<IEnumerable<CardPrintFinish>> CreateCardPrintFinishes(IEnumerable<CardPrintFinish> cardPrintFinishes)
	{
		var addedCardPrintFinishes = new List<CardPrintFinish>();

		foreach (CardPrintFinish cardPrintFinish in cardPrintFinishes)
		{
			EntityEntry<CardPrintFinish> addedCardPrintFinish = await _dbContext.CardPrintFinishes.AddAsync(cardPrintFinish);
			addedCardPrintFinishes.Add(addedCardPrintFinish.Entity);
		}

		await _dbContext.SaveChangesAsync();

		return addedCardPrintFinishes;
	}

	public async Task<IEnumerable<CardGameType>> CreateCardGameTypes(IEnumerable<CardGameType> cardGameTypes)
	{
		var addedCardGameTypes = new List<CardGameType>();

		foreach (CardGameType cardGameType in cardGameTypes)
		{
			EntityEntry<CardGameType> addedCardPrintFinish = await _dbContext.CardGameTypes.AddAsync(cardGameType);
			addedCardGameTypes.Add(addedCardPrintFinish.Entity);
		}

		await _dbContext.SaveChangesAsync();

		return addedCardGameTypes;
	}
}
