using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Services;

public class CardService : ICardService
{
	private readonly IEqualityComparer<Card> _cardComparer;
	private readonly IEqualityComparer<CardLegality> _cardLegalityComparer;
	private readonly IEqualityComparer<CardPrice> _cardPriceComparer;
	private readonly ApplicationDbContext _dbContext;
	public CardService(
		IEqualityComparer<Card> comparer,
		IEqualityComparer<CardLegality> cardLegalityComparer,
		IEqualityComparer<CardPrice> cardPriceComparer,
		ApplicationDbContext dbContext)
	{
		_cardComparer = comparer;
		_cardLegalityComparer = cardLegalityComparer;
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
			.Include(card => card.Legalities)
			.Include(card => card.CardKeywords)
			.Include(card => card.CardPromoTypes)
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

	public async Task<CardPrice> CreatePrice(CardPrice cardPrice)
	{
		EntityEntry<CardPrice> addedPrice = await _dbContext.CardPrices.AddAsync(cardPrice);
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

		if (!_cardPriceComparer.Equals(existingPrice, priceToUpdate))
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

	public async Task<CardLegality> GetCardLegality(long cardLegalityId)
	{
		return await _dbContext.CardLegalities.SingleAsync(cardLegality => cardLegality.Id == cardLegalityId);
	}

	public async Task<CardLegality> CreateCardLegality(CardLegality legality)
	{
		EntityEntry<CardLegality> addedLegality = await _dbContext.CardLegalities.AddAsync(legality);
		await _dbContext.SaveChangesAsync();

		return addedLegality.Entity;
	}

	public async Task<IEnumerable<CardLegality>> CreateCardLegalities(IEnumerable<CardLegality> legalities)
	{
		var addedLegalities = new List<CardLegality>();
		if (!legalities.Any()) return addedLegalities;

		foreach (var legality in legalities)
		{
			EntityEntry<CardLegality> addedLegality = await _dbContext.CardLegalities.AddAsync(legality);
			addedLegalities.Add(addedLegality.Entity);
		}

		await _dbContext.SaveChangesAsync();

		return addedLegalities;
	}

	public async Task<CardLegality> UpdateCardLegality(CardLegality legalityWithChanges)
	{
		CardLegality legalityToUpdate = await GetCardLegality(legalityWithChanges.Id);
		CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);

		await _dbContext.SaveChangesAsync();

		return legalityToUpdate;
	}

	public async Task<IEnumerable<CardLegality>> UpdateCardLegalities(IEnumerable<CardLegality> legalitiesWithChanges)
	{
		var updatedCardLegalities = new List<CardLegality>();
		if (!legalitiesWithChanges.Any()) return updatedCardLegalities;

		foreach (var legalityWithChanges in legalitiesWithChanges)
		{
			CardLegality legalityToUpdate = await GetCardLegality(legalityWithChanges.Id);

			CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);
			updatedCardLegalities.Add(legalityToUpdate);
		}

		await _dbContext.SaveChangesAsync();

		return updatedCardLegalities;
	}

	public async Task<CardLegality> UpdateCardLegalityIfChanged(CardLegality legalityWithChanges)
	{
		CardLegality legalityToUpdate = await GetCardLegality(legalityWithChanges.Id);

		if (_cardLegalityComparer.Equals(legalityToUpdate, legalityWithChanges))
		{
			CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);
			await _dbContext.SaveChangesAsync();
		}

		return legalityToUpdate;
	}

	public async Task<IEnumerable<CardLegality>> UpdateCardLegalitiesIfChanged(IEnumerable<CardLegality> legalitiesWithChanges)
	{
		var updatedCardLegalities = new List<CardLegality>();
		if (!legalitiesWithChanges.Any()) return updatedCardLegalities;

		foreach (var legalityWithChanges in legalitiesWithChanges)
		{
			CardLegality legalityToUpdate = await GetCardLegality(legalityWithChanges.Id);

			if (_cardLegalityComparer.Equals(legalityToUpdate, legalityWithChanges))
			{
				CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);
			}

			updatedCardLegalities.Add(legalityToUpdate);
		}

		await _dbContext.SaveChangesAsync();

		return updatedCardLegalities;
	}

	public async Task<IEnumerable<Keyword>> GetKeywords()
	{
		return await _dbContext.Keywords.ToListAsync();
	}

	public async Task<IEnumerable<Keyword>> GetKeywords(SourceType source)
	{
		return await _dbContext.Keywords
			.Where(keyword => keyword.SourceId == (int)source)
			.ToListAsync();
	}

	public async Task<Keyword> CreateKeyword(Keyword keyword)
	{
		EntityEntry<Keyword> addedKeyword = await _dbContext.Keywords.AddAsync(keyword);
		await _dbContext.SaveChangesAsync();

		return addedKeyword.Entity;
	}

	public async Task<IEnumerable<Keyword>> CreateKeywords(IEnumerable<Keyword> keywords)
	{
		var addedKeywords = new List<Keyword>();

		foreach (Keyword keywordToAdd in keywords)
		{
			EntityEntry<Keyword> addedKeyword = await _dbContext.Keywords.AddAsync(keywordToAdd);
			addedKeywords.Add(addedKeyword.Entity);
		}
		await _dbContext.SaveChangesAsync();

		return addedKeywords;
	}

	public async Task<CardKeyword> GetCardKeyword(long cardKeywordId)
	{
		return await _dbContext.CardKeywords.SingleAsync(cardKeyword => cardKeyword.Id == cardKeywordId);
	}

	public async Task<CardKeyword> CreateCardKeyword(CardKeyword cardKeyword)
	{
		EntityEntry<CardKeyword> addedCardKeyword = await _dbContext.CardKeywords.AddAsync(cardKeyword);
		await _dbContext.SaveChangesAsync();

		return addedCardKeyword.Entity;
	}

	public async Task<IEnumerable<CardKeyword>> CreateCardKeywords(IEnumerable<CardKeyword> cardKeywords)
	{
		var addedCardKeywords = new List<CardKeyword>();

		foreach (CardKeyword cardKeyword in cardKeywords)
		{
			EntityEntry<CardKeyword> addedCardKeyword = await _dbContext.CardKeywords.AddAsync(cardKeyword);
			addedCardKeywords.Add(addedCardKeyword.Entity);
		}
		await _dbContext.SaveChangesAsync();

		return addedCardKeywords;
	}

	public async Task<CardKeyword> UpdateCardKeyword(CardKeyword cardKeywordWithChanges)
	{
		CardKeyword cardKeywordToUpdate = await GetCardKeyword(cardKeywordWithChanges.Id);
		CardMapper.MergeProperties(cardKeywordToUpdate, cardKeywordWithChanges);

		await _dbContext.SaveChangesAsync();

		return cardKeywordToUpdate;
	}

	public async Task<IEnumerable<CardKeyword>> UpdateCardKeywords(IEnumerable<CardKeyword> cardKeywordsWithChanges)
	{
		var updatedCardKeywords = new List<CardKeyword>();
		if (!cardKeywordsWithChanges.Any()) return updatedCardKeywords;

		foreach (var cardKeywordWithChanges in cardKeywordsWithChanges)
		{
			CardKeyword cardKeywordToUpdate = await GetCardKeyword(cardKeywordWithChanges.Id);

			CardMapper.MergeProperties(cardKeywordToUpdate, cardKeywordWithChanges);
			updatedCardKeywords.Add(cardKeywordWithChanges);
		}

		await _dbContext.SaveChangesAsync();

		return updatedCardKeywords;
	}

	public async Task<IEnumerable<PromoType>> GetPromoTypes()
	{
		return await _dbContext.PromoTypes.ToListAsync();
	}

	public async Task<IEnumerable<PromoType>> GetPromoTypes(SourceType source)
	{
		return await _dbContext.PromoTypes
			.Where(promoType => promoType.SourceId == (int)source)
			.ToListAsync();
	}

	public async Task<PromoType> CreatePromoType(PromoType promoType)
	{
		EntityEntry<PromoType> addedPromoType = await _dbContext.PromoTypes.AddAsync(promoType);
		await _dbContext.SaveChangesAsync();

		return addedPromoType.Entity;
	}

	public async Task<IEnumerable<PromoType>> CreatePromoTypes(IEnumerable<PromoType> promoTypes)
	{
		List<PromoType> addedPromoTypes = new();
		if (!promoTypes.Any()) return addedPromoTypes;

		foreach (var promoType in promoTypes)
		{
			EntityEntry<PromoType> addedPromoType = await _dbContext.AddAsync(promoType);
			addedPromoTypes.Add(addedPromoType.Entity);
		}
		await _dbContext.SaveChangesAsync();

		return addedPromoTypes;
	}

	public async Task<IEnumerable<CardPromoType>> CreateCardPromoTypes(IEnumerable<CardPromoType> cardPromoTypes)
	{
		List<CardPromoType> addedCardPromoTypes = new();
		if (!cardPromoTypes.Any()) return addedCardPromoTypes;

		foreach (var cardPromoType in cardPromoTypes)
		{
			EntityEntry<CardPromoType> addedCardPromoType = await _dbContext.CardPromoTypes.AddAsync(cardPromoType);
			addedCardPromoTypes.Add(addedCardPromoType.Entity);
		}
		await _dbContext.SaveChangesAsync();

		return addedCardPromoTypes;
	}
	
	public async Task<CardPromoType> GetCardPromoType(long cardPromoTypeId)
	{
		return await _dbContext.CardPromoTypes.SingleAsync(cardPromoType => cardPromoType.Id == cardPromoTypeId);
	}

	public async Task<IEnumerable<CardPromoType>> UpdateCardPromoTypes(IEnumerable<CardPromoType> cardPromoTypes)
	{
		List<CardPromoType> updatedCardPromoTypes = new();
		if (!cardPromoTypes.Any()) return updatedCardPromoTypes;

		foreach (var cardPromoType in cardPromoTypes)
		{
			CardPromoType cardPromoTypeToUpdate = await GetCardPromoType(cardPromoType.Id);

			CardMapper.MergeProperties(cardPromoTypeToUpdate, cardPromoType);
			updatedCardPromoTypes.Add(cardPromoTypeToUpdate);
		}
		await _dbContext.SaveChangesAsync();

		return updatedCardPromoTypes;
	}
}
