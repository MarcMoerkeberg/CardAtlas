using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Repositories;

public class CardRepository : ICardRepository
{
	private readonly IEqualityComparer<Card> _cardComparer;
	private readonly IEqualityComparer<CardLegality> _cardLegalityComparer;
	private readonly IEqualityComparer<CardPrice> _cardPriceComparer;
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
	public CardRepository(
		IEqualityComparer<Card> comparer,
		IEqualityComparer<CardLegality> cardLegalityComparer,
		IEqualityComparer<CardPrice> cardPriceComparer,
		IDbContextFactory<ApplicationDbContext> dbContextFactory)
	{
		_cardComparer = comparer;
		_cardLegalityComparer = cardLegalityComparer;
		_cardPriceComparer = cardPriceComparer;
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Card> Get(long cardId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();
		return await dbContext.Cards.SingleAsync(card => card.Id == cardId);
	}

	public async Task<Card> Create(Card card)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<Card> addedCard = await dbContext.Cards.AddAsync(card);
		await dbContext.SaveChangesAsync();

		return addedCard.Entity;
	}

	public async Task<Card> Update(Card cardWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		Card cardToUpdate = await dbContext.Cards.SingleAsync(card => card.Id == cardWithChanges.Id);
		CardMapper.MergeProperties(cardToUpdate, cardWithChanges);

		await dbContext.SaveChangesAsync();

		return cardToUpdate;
	}

	public async Task<Card> UpdateIfChanged(Card cardWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		Card existingCard = await dbContext.Cards.SingleAsync(card => card.Id == cardWithChanges.Id);

		if (!_cardComparer.Equals(existingCard, cardWithChanges))
		{
			CardMapper.MergeProperties(existingCard, cardWithChanges);
			await dbContext.SaveChangesAsync();
		}

		return existingCard;
	}

	public async Task<IEnumerable<Card>> GetFromScryfallId(Guid scryfallId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Cards
			.Include(card => card.Images)
			.Include(card => card.Set)
			.Include(card => card.Prices)
			.Include(card => card.CardPrintFinishes)
			.Include(card => card.Legalities)
			.Include(card => card.CardKeywords)
			.Include(card => card.CardPromoTypes)
			.Where(card => card.ScryfallId == scryfallId)
			.ToListAsync();
	}

	public async Task<CardPrice> GetPrice(long priceId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrices.SingleAsync(price => price.Id == priceId);
	}

	public async Task<IEnumerable<CardPrice>> GetPrices(long cardId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrices
			.Where(price => price.CardId == cardId)
			.ToListAsync();
	}

	public async Task<CardPrice> CreatePrice(CardPrice cardPrice)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<CardPrice> addedPrice = await dbContext.CardPrices.AddAsync(cardPrice);
		await dbContext.SaveChangesAsync();

		return addedPrice.Entity;
	}

	public async Task<CardPrice> UpdatePrice(CardPrice priceToUpdate)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardPrice existingPrice = await dbContext.CardPrices.SingleAsync(price => price.Id == priceToUpdate.Id);
		CardPriceMapper.MergeProperties(existingPrice, priceToUpdate);
		await dbContext.SaveChangesAsync();

		return existingPrice;
	}

	public async Task<CardPrice> UpdatePriceIfChanged(CardPrice priceToUpdate)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardPrice existingPrice = await dbContext.CardPrices.SingleAsync(price => price.Id == priceToUpdate.Id);

		if (!_cardPriceComparer.Equals(existingPrice, priceToUpdate))
		{
			CardPriceMapper.MergeProperties(existingPrice, priceToUpdate);
			await dbContext.SaveChangesAsync();
		}

		return existingPrice;
	}

	public async Task<IEnumerable<CardPrintFinish>> CreateCardPrintFinishes(IEnumerable<CardPrintFinish> cardPrintFinishes)
	{
		var addedCardPrintFinishes = new List<CardPrintFinish>();
		if (!cardPrintFinishes.Any()) return addedCardPrintFinishes;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (CardPrintFinish cardPrintFinish in cardPrintFinishes)
		{
			EntityEntry<CardPrintFinish> addedCardPrintFinish = await dbContext.CardPrintFinishes.AddAsync(cardPrintFinish);
			addedCardPrintFinishes.Add(addedCardPrintFinish.Entity);
		}

		await dbContext.SaveChangesAsync();

		return addedCardPrintFinishes;
	}

	public async Task<CardLegality> GetCardLegality(long cardLegalityId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardLegalities.SingleAsync(cardLegality => cardLegality.Id == cardLegalityId);
	}

	public async Task<CardLegality> CreateCardLegality(CardLegality legality)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<CardLegality> addedLegality = await dbContext.CardLegalities.AddAsync(legality);
		await dbContext.SaveChangesAsync();

		return addedLegality.Entity;
	}

	public async Task<IEnumerable<CardLegality>> CreateCardLegalities(IEnumerable<CardLegality> legalities)
	{
		var addedLegalities = new List<CardLegality>();
		if (!legalities.Any()) return addedLegalities;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var legality in legalities)
		{
			EntityEntry<CardLegality> addedLegality = await dbContext.CardLegalities.AddAsync(legality);
			addedLegalities.Add(addedLegality.Entity);
		}

		await dbContext.SaveChangesAsync();

		return addedLegalities;
	}

	public async Task<CardLegality> UpdateCardLegality(CardLegality legalityWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardLegality legalityToUpdate = await dbContext.CardLegalities.SingleAsync(cardLegality => cardLegality.Id == legalityWithChanges.Id);
		CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);

		await dbContext.SaveChangesAsync();

		return legalityToUpdate;
	}

	public async Task<IEnumerable<CardLegality>> UpdateCardLegalities(IEnumerable<CardLegality> legalitiesWithChanges)
	{
		var updatedCardLegalities = new List<CardLegality>();
		if (!legalitiesWithChanges.Any()) return updatedCardLegalities;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var legalityWithChanges in legalitiesWithChanges)
		{
			CardLegality legalityToUpdate = await dbContext.CardLegalities.SingleAsync(cardLegality => cardLegality.Id == legalityWithChanges.Id);

			CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);
			updatedCardLegalities.Add(legalityToUpdate);
		}

		await dbContext.SaveChangesAsync();

		return updatedCardLegalities;
	}

	public async Task<CardLegality> UpdateCardLegalityIfChanged(CardLegality legalityWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardLegality legalityToUpdate = await dbContext.CardLegalities.SingleAsync(cardLegality => cardLegality.Id == legalityWithChanges.Id);

		if (_cardLegalityComparer.Equals(legalityToUpdate, legalityWithChanges))
		{
			CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);
			await dbContext.SaveChangesAsync();
		}

		return legalityToUpdate;
	}

	public async Task<IEnumerable<CardLegality>> UpdateCardLegalitiesIfChanged(IEnumerable<CardLegality> legalitiesWithChanges)
	{
		var updatedCardLegalities = new List<CardLegality>();
		if (!legalitiesWithChanges.Any()) return updatedCardLegalities;
		
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var legalityWithChanges in legalitiesWithChanges)
		{
			CardLegality legalityToUpdate = await dbContext.CardLegalities.SingleAsync(cardLegality => cardLegality.Id == legalityWithChanges.Id);

			if (_cardLegalityComparer.Equals(legalityToUpdate, legalityWithChanges))
			{
				CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);
			}

			updatedCardLegalities.Add(legalityToUpdate);
		}

		await dbContext.SaveChangesAsync();

		return updatedCardLegalities;
	}

	public async Task<IEnumerable<Keyword>> GetKeywords()
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Keywords.ToListAsync();
	}

	public async Task<IEnumerable<Keyword>> GetKeywords(SourceType source)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Keywords
			.Where(keyword => keyword.SourceId == (int)source)
			.ToListAsync();
	}

	public async Task<Keyword> CreateKeyword(Keyword keyword)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<Keyword> addedKeyword = await dbContext.Keywords.AddAsync(keyword);
		await dbContext.SaveChangesAsync();

		return addedKeyword.Entity;
	}

	public async Task<IEnumerable<Keyword>> CreateKeywords(IEnumerable<Keyword> keywords)
	{
		var addedKeywords = new List<Keyword>();
		if (!keywords.Any()) return addedKeywords;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (Keyword keywordToAdd in keywords)
		{
			EntityEntry<Keyword> addedKeyword = await dbContext.Keywords.AddAsync(keywordToAdd);
			addedKeywords.Add(addedKeyword.Entity);
		}
		await dbContext.SaveChangesAsync();

		return addedKeywords;
	}

	public async Task<CardKeyword> GetCardKeyword(long cardKeywordId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardKeywords.SingleAsync(cardKeyword => cardKeyword.Id == cardKeywordId);
	}

	public async Task<CardKeyword> CreateCardKeyword(CardKeyword cardKeyword)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<CardKeyword> addedCardKeyword = await dbContext.CardKeywords.AddAsync(cardKeyword);
		await dbContext.SaveChangesAsync();

		return addedCardKeyword.Entity;
	}

	public async Task<IEnumerable<CardKeyword>> CreateCardKeywords(IEnumerable<CardKeyword> cardKeywords)
	{
		var addedCardKeywords = new List<CardKeyword>();
		if (!cardKeywords.Any()) return addedCardKeywords;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (CardKeyword cardKeyword in cardKeywords)
		{
			EntityEntry<CardKeyword> addedCardKeyword = await dbContext.CardKeywords.AddAsync(cardKeyword);
			addedCardKeywords.Add(addedCardKeyword.Entity);
		}
		await dbContext.SaveChangesAsync();

		return addedCardKeywords;
	}

	public async Task<CardKeyword> UpdateCardKeyword(CardKeyword cardKeywordWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardKeyword cardKeywordToUpdate = await dbContext.CardKeywords.SingleAsync(cardKeyword => cardKeyword.Id == cardKeywordWithChanges.Id);
		CardMapper.MergeProperties(cardKeywordToUpdate, cardKeywordWithChanges);

		await dbContext.SaveChangesAsync();

		return cardKeywordToUpdate;
	}

	public async Task<IEnumerable<CardKeyword>> UpdateCardKeywords(IEnumerable<CardKeyword> cardKeywordsWithChanges)
	{
		var updatedCardKeywords = new List<CardKeyword>();
		if (!cardKeywordsWithChanges.Any()) return updatedCardKeywords;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var cardKeywordWithChanges in cardKeywordsWithChanges)
		{
			CardKeyword cardKeywordToUpdate = await dbContext.CardKeywords.SingleAsync(cardKeyword => cardKeyword.Id == cardKeywordWithChanges.Id);

			CardMapper.MergeProperties(cardKeywordToUpdate, cardKeywordWithChanges);
			updatedCardKeywords.Add(cardKeywordWithChanges);
		}

		await dbContext.SaveChangesAsync();

		return updatedCardKeywords;
	}

	public async Task<IEnumerable<PromoType>> GetPromoTypes()
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.PromoTypes.ToListAsync();
	}

	public async Task<IEnumerable<PromoType>> GetPromoTypes(SourceType source)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.PromoTypes
			.Where(promoType => promoType.SourceId == (int)source)
			.ToListAsync();
	}

	public async Task<PromoType> CreatePromoType(PromoType promoType)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<PromoType> addedPromoType = await dbContext.PromoTypes.AddAsync(promoType);
		await dbContext.SaveChangesAsync();

		return addedPromoType.Entity;
	}

	public async Task<IEnumerable<PromoType>> CreatePromoTypes(IEnumerable<PromoType> promoTypes)
	{
		List<PromoType> addedPromoTypes = new();
		if (!promoTypes.Any()) return addedPromoTypes;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var promoType in promoTypes)
		{
			EntityEntry<PromoType> addedPromoType = await dbContext.AddAsync(promoType);
			addedPromoTypes.Add(addedPromoType.Entity);
		}
		await dbContext.SaveChangesAsync();

		return addedPromoTypes;
	}

	public async Task<IEnumerable<CardPromoType>> CreateCardPromoTypes(IEnumerable<CardPromoType> cardPromoTypes)
	{
		List<CardPromoType> addedCardPromoTypes = new();
		if (!cardPromoTypes.Any()) return addedCardPromoTypes;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var cardPromoType in cardPromoTypes)
		{
			EntityEntry<CardPromoType> addedCardPromoType = await dbContext.CardPromoTypes.AddAsync(cardPromoType);
			addedCardPromoTypes.Add(addedCardPromoType.Entity);
		}
		await dbContext.SaveChangesAsync();

		return addedCardPromoTypes;
	}
	
	public async Task<CardPromoType> GetCardPromoType(long cardPromoTypeId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPromoTypes.SingleAsync(cardPromoType => cardPromoType.Id == cardPromoTypeId);
	}

	public async Task<IEnumerable<CardPromoType>> UpdateCardPromoTypes(IEnumerable<CardPromoType> cardPromoTypes)
	{
		List<CardPromoType> updatedCardPromoTypes = new();
		if (!cardPromoTypes.Any()) return updatedCardPromoTypes;

		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var cardPromoType in cardPromoTypes)
		{
			CardPromoType cardPromoTypeToUpdate = await dbContext.CardPromoTypes.SingleAsync(cardPromoType => cardPromoType.Id == cardPromoType.Id);

			CardMapper.MergeProperties(cardPromoTypeToUpdate, cardPromoType);
			updatedCardPromoTypes.Add(cardPromoTypeToUpdate);
		}
		await dbContext.SaveChangesAsync();

		return updatedCardPromoTypes;
	}
}
