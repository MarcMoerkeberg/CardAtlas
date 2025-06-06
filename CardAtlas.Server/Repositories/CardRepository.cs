﻿using CardAtlas.Server.DAL;
using CardAtlas.Server.Extensions;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Repositories;

public class CardRepository : ICardRepository
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
	public CardRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Card> Get(long cardId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();
		return await dbContext.Cards
			.AsNoTracking()
			.SingleAsync(card => card.Id == cardId);
	}

	public async Task<Card> Create(Card card)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<Card> addedCard = await dbContext.Cards.AddAsync(card);
		await dbContext.SaveChangesAsync();

		return addedCard.Entity;
	}

	public async Task<Card> Update(Card cardWithChanges)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		Card cardToUpdate = await dbContext.Cards.SingleAsync(card => card.Id == cardWithChanges.Id);
		CardMapper.MergeProperties(cardToUpdate, cardWithChanges);

		await dbContext.SaveChangesAsync();

		return cardToUpdate;
	}

	public async Task<IEnumerable<Card>> Get(Guid scryfallId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Cards
			.AsNoTracking()
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

	public async Task<IEnumerable<Card>> Get(IEnumerable<Guid> scryfallIds)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Cards
			.AsNoTracking()
			.Include(card => card.ParentCard)
			.Where(card => card.ScryfallId.HasValue &&
				scryfallIds.Contains(card.ScryfallId.Value))
			.ToListAsync();
	}

	public async Task<CardPrice> GetPrice(long priceId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrices
			.AsNoTracking()
			.SingleAsync(price => price.Id == priceId);
	}

	public async Task<IEnumerable<CardPrice>> GetPrices(long cardId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrices
			.AsNoTracking()
			.Where(price => price.CardId == cardId)
			.ToListAsync();
	}

	public async Task<CardPrice> Create(CardPrice cardPrice)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<CardPrice> addedPrice = await dbContext.CardPrices.AddAsync(cardPrice);
		await dbContext.SaveChangesAsync();

		return addedPrice.Entity;
	}

	public async Task<CardPrice> Update(CardPrice priceToUpdate)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardPrice existingPrice = await dbContext.CardPrices.SingleAsync(price => price.Id == priceToUpdate.Id);
		CardPriceMapper.MergeProperties(existingPrice, priceToUpdate);
		await dbContext.SaveChangesAsync();

		return existingPrice;
	}

	public async Task<IEnumerable<CardPrintFinish>> Create(IEnumerable<CardPrintFinish> cardPrintFinishes)
	{
		var addedCardPrintFinishes = new List<CardPrintFinish>();
		if (!cardPrintFinishes.Any()) return addedCardPrintFinishes;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

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
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardLegalities
			.AsNoTracking()
			.SingleAsync(cardLegality => cardLegality.Id == cardLegalityId);
	}

	public async Task<CardLegality> Create(CardLegality legality)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<CardLegality> addedLegality = await dbContext.CardLegalities.AddAsync(legality);
		await dbContext.SaveChangesAsync();

		return addedLegality.Entity;
	}

	public async Task<IEnumerable<CardLegality>> Create(IEnumerable<CardLegality> legalities)
	{
		var addedLegalities = new List<CardLegality>();
		if (!legalities.Any()) return addedLegalities;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var legality in legalities)
		{
			EntityEntry<CardLegality> addedLegality = await dbContext.CardLegalities.AddAsync(legality);
			addedLegalities.Add(addedLegality.Entity);
		}

		await dbContext.SaveChangesAsync();

		return addedLegalities;
	}

	public async Task<CardLegality> Update(CardLegality legalityWithChanges)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardLegality legalityToUpdate = await dbContext.CardLegalities.SingleAsync(cardLegality => cardLegality.Id == legalityWithChanges.Id);
		CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);

		await dbContext.SaveChangesAsync();

		return legalityToUpdate;
	}

	public async Task<IEnumerable<CardLegality>> Update(IEnumerable<CardLegality> legalitiesWithChanges)
	{
		var updatedCardLegalities = new List<CardLegality>();
		if (!legalitiesWithChanges.Any()) return updatedCardLegalities;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var legalityWithChanges in legalitiesWithChanges)
		{
			CardLegality legalityToUpdate = await dbContext.CardLegalities.SingleAsync(cardLegality => cardLegality.Id == legalityWithChanges.Id);

			CardMapper.MergeProperties(legalityToUpdate, legalityWithChanges);
			updatedCardLegalities.Add(legalityToUpdate);
		}

		await dbContext.SaveChangesAsync();

		return updatedCardLegalities;
	}

	public async Task<IEnumerable<Keyword>> GetKeywords()
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Keywords
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<List<Keyword>> GetKeywords(SourceType source)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.Keywords
			.AsNoTracking()
			.Where(keyword => keyword.SourceId == (int)source)
			.ToListAsync();
	}

	public async Task<Keyword> Create(Keyword keyword)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<Keyword> addedKeyword = await dbContext.Keywords.AddAsync(keyword);
		await dbContext.SaveChangesAsync();

		return addedKeyword.Entity;
	}

	public async Task<IEnumerable<Keyword>> Create(IEnumerable<Keyword> keywords)
	{
		var addedKeywords = new List<Keyword>();
		if (!keywords.Any()) return addedKeywords;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

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
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardKeywords
			.AsNoTracking()
			.SingleAsync(cardKeyword => cardKeyword.Id == cardKeywordId);
	}

	public async Task<CardKeyword> Create(CardKeyword cardKeyword)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<CardKeyword> addedCardKeyword = await dbContext.CardKeywords.AddAsync(cardKeyword);
		await dbContext.SaveChangesAsync();

		return addedCardKeyword.Entity;
	}

	public async Task<IEnumerable<CardKeyword>> Create(IEnumerable<CardKeyword> cardKeywords)
	{
		var addedCardKeywords = new List<CardKeyword>();
		if (!cardKeywords.Any()) return addedCardKeywords;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (CardKeyword cardKeyword in cardKeywords)
		{
			EntityEntry<CardKeyword> addedCardKeyword = await dbContext.CardKeywords.AddAsync(cardKeyword);
			addedCardKeywords.Add(addedCardKeyword.Entity);
		}
		await dbContext.SaveChangesAsync();

		return addedCardKeywords;
	}

	public async Task<CardKeyword> Update(CardKeyword cardKeywordWithChanges)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardKeyword cardKeywordToUpdate = await dbContext.CardKeywords.SingleAsync(cardKeyword => cardKeyword.Id == cardKeywordWithChanges.Id);
		CardMapper.MergeProperties(cardKeywordToUpdate, cardKeywordWithChanges);

		await dbContext.SaveChangesAsync();

		return cardKeywordToUpdate;
	}

	public async Task<IEnumerable<CardKeyword>> Update(IEnumerable<CardKeyword> cardKeywordsWithChanges)
	{
		var updatedCardKeywords = new List<CardKeyword>();
		if (!cardKeywordsWithChanges.Any()) return updatedCardKeywords;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

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
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.PromoTypes
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<List<PromoType>> GetPromoTypes(SourceType source)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.PromoTypes
			.AsNoTracking()
			.Where(promoType => promoType.SourceId == (int)source)
			.ToListAsync();
	}

	public async Task<PromoType> Create(PromoType promoType)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<PromoType> addedPromoType = await dbContext.PromoTypes.AddAsync(promoType);
		await dbContext.SaveChangesAsync();

		return addedPromoType.Entity;
	}

	public async Task<IEnumerable<PromoType>> Create(IEnumerable<PromoType> promoTypes)
	{
		List<PromoType> addedPromoTypes = new();
		if (!promoTypes.Any()) return addedPromoTypes;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var promoType in promoTypes)
		{
			EntityEntry<PromoType> addedPromoType = await dbContext.AddAsync(promoType);
			addedPromoTypes.Add(addedPromoType.Entity);
		}
		await dbContext.SaveChangesAsync();

		return addedPromoTypes;
	}

	public async Task<IEnumerable<CardPromoType>> Create(IEnumerable<CardPromoType> cardPromoTypes)
	{
		List<CardPromoType> addedCardPromoTypes = new();
		if (!cardPromoTypes.Any()) return addedCardPromoTypes;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

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
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPromoTypes
			.AsNoTracking()
			.SingleAsync(cardPromoType => cardPromoType.Id == cardPromoTypeId);
	}

	public async Task<IEnumerable<CardPromoType>> Update(IEnumerable<CardPromoType> cardPromoTypes)
	{
		List<CardPromoType> updatedCardPromoTypes = new();
		if (!cardPromoTypes.Any()) return updatedCardPromoTypes;

		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		foreach (var cardPromoType in cardPromoTypes)
		{
			CardPromoType cardPromoTypeToUpdate = await dbContext.CardPromoTypes.SingleAsync(cardPromoType => cardPromoType.Id == cardPromoType.Id);

			CardMapper.MergeProperties(cardPromoTypeToUpdate, cardPromoType);
			updatedCardPromoTypes.Add(cardPromoTypeToUpdate);
		}
		await dbContext.SaveChangesAsync();

		return updatedCardPromoTypes;
	}

	public async Task<int> Upsert(UpsertContainer<Card> upsertionData)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.UpsertAsync(upsertionData);
	}

	public async Task<int> Upsert(UpsertContainer<Keyword> upsertionData)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.UpsertAsync(upsertionData);
	}

	public async Task<IEnumerable<CardPrice>> GetCardPrices()
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrices
			.AsNoTracking()
			.Include(cp => cp.Currency)
			.Include(cp => cp.Vendor)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardPrice>> GetCardPrices(IEnumerable<long> cardIds)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrices
			.AsNoTracking()
			.Include(cp => cp.Currency)
			.Include(cp => cp.Vendor)
			.Where(cardPrice => cardIds.Contains(cardPrice.CardId))
			.ToListAsync();
	}

	public async Task<int> Upsert(UpsertContainer<CardPrice> upsertionData)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.UpsertAsync(upsertionData);
	}

	public async Task<IEnumerable<CardGamePlatform>> GetCardGamePlatforms()
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardGamePlatforms
			.AsNoTracking()
			.Include(cgp => cgp.GamePlatform)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardGamePlatform>> GetCardGamePlatforms(long cardId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardGamePlatforms
			.AsNoTracking()
			.Include(cgp => cgp.GamePlatform)
			.Where(cardGamePlatform => cardGamePlatform.CardId == cardId)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardGamePlatform>> GetCardGamePlatforms(IEnumerable<long> cardIds)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardGamePlatforms
			.AsNoTracking()
			.Include(cgp => cgp.GamePlatform)
			.Where(cardGamePlatform => cardIds.Contains(cardGamePlatform.CardId))
			.ToListAsync();
	}

	public async Task<IEnumerable<CardGamePlatform>> Create(IEnumerable<CardGamePlatform> platforms)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();
		List<CardGamePlatform> addedPlatforms = new();

		foreach (var platform in platforms)
		{
			EntityEntry<CardGamePlatform> addedPlatform = await dbContext.CardGamePlatforms.AddAsync(platform);
			addedPlatforms.Add(addedPlatform.Entity);
		}
		await dbContext.SaveChangesAsync();

		return addedPlatforms;
	}

	public async Task<IEnumerable<CardPrintFinish>> GetCardPrintFinishes()
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrintFinishes
			.AsNoTracking()
			.Include(cpf => cpf.PrintFinish)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardPrintFinish>> GetCardPrintFinishes(long cardId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrintFinishes
			.AsNoTracking()
			.Include(cpf => cpf.PrintFinish)
			.Where(cardPrintFinish => cardPrintFinish.CardId == cardId)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardPrintFinish>> GetCardPrintFinishes(IEnumerable<long> cardIds)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPrintFinishes
			.AsNoTracking()
			.Include(cpf => cpf.PrintFinish)
			.Where(cardPrintFinish => cardIds.Contains(cardPrintFinish.CardId))
			.ToListAsync();
	}

	public async Task<IEnumerable<CardLegality>> GetCardLegalities(long cardId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardLegalities
			.AsNoTracking()
			.Include(cl => cl.Legality)
			.Include(cl => cl.GameFormat)
			.Where(cardLegality => cardLegality.CardId == cardId)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardLegality>> GetCardLegalities(IEnumerable<long> cardIds)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardLegalities
			.AsNoTracking()
			.Include(cl => cl.Legality)
			.Include(cl => cl.GameFormat)
			.Where(cardLegality => cardIds.Contains(cardLegality.CardId))
			.ToListAsync();
	}

	public async Task<int> Upsert(UpsertContainer<CardLegality> upsertionData)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.UpsertAsync(upsertionData);
	}

	public async Task<IEnumerable<CardKeyword>> GetCardKeywords(long cardId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardKeywords
			.AsNoTracking()
			.Include(ck => ck.Keyword)
			.Where(cardKeyword => cardKeyword.CardId == cardId)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardKeyword>> GetCardKeywords(IEnumerable<long> cardIds)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardKeywords
			.AsNoTracking()
			.Include(ck => ck.Keyword)
			.Where(cardKeyword => cardIds.Contains(cardKeyword.CardId))
			.ToListAsync();
	}

	public async Task<IEnumerable<CardPromoType>> GetCardPromoTypes(long cardId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPromoTypes
			.AsNoTracking()
			.Include(cpt => cpt.PromoType)
			.Where(cardPromoType => cardPromoType.CardId == cardId)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardPromoType>> GetCardPromoTypes(IEnumerable<long> cardIds)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardPromoTypes
			.AsNoTracking()
			.Include(cpt => cpt.PromoType)
			.Where(cardPromoType => cardIds.Contains(cardPromoType.CardId))
			.ToListAsync();
	}
}
