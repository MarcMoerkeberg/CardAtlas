using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardAtlas.Server.Repositories;

public class CardImageRepository : ICardImageRepository
{
	private readonly IEqualityComparer<CardImage> _cardImageComparer;
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

	public CardImageRepository(
		IEqualityComparer<CardImage> cardImageComparer,
		IDbContextFactory<ApplicationDbContext> dbContextFactory)
	{
		_cardImageComparer = cardImageComparer;
		_dbContextFactory = dbContextFactory;
	}

	public async Task<CardImage> Create(CardImage cardImage)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<CardImage> addedCardImage = await dbContext.CardImages.AddAsync(cardImage);
		await dbContext.SaveChangesAsync();

		return addedCardImage.Entity;
	}

	public async Task<CardImage> Get(long cardImageId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardImages
			.AsNoTracking()
			.SingleAsync(cardImage => cardImage.Id == cardImageId);
	}

	public async Task<IEnumerable<CardImage>> Get(SourceType source)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardImages
			.Include(cardImage => cardImage.SourceType)
			.Include(cardImage => cardImage.ImageType)
			.AsNoTracking()
			.Where(cardImage => cardImage.SourceType == SourceType.Scryfall)
			.ToListAsync();
	}

	public async Task<IEnumerable<CardImage>> GetFromCardId(long cardId)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardImages
			.AsNoTracking()
			.Include(ci => ci.ImageType)
			.Include(ci => ci.ImageFormat)
			.Include(ci => ci.ImageStatus)
			.Include(ci => ci.Source)
			.Where(cardImage => cardImage.CardId == cardId)
			.ToListAsync();
	}

	public async Task<CardImage> Update(CardImage cardImageWithChanges)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardImage imageToUpdate = await dbContext.CardImages.SingleAsync(cardImage => cardImage.Id == cardImageWithChanges.Id);
		CardImageMapper.MergeProperties(imageToUpdate, cardImageWithChanges);

		await dbContext.SaveChangesAsync();
		return imageToUpdate;
	}

	public async Task<CardImage> UpdateIfChanged(CardImage cardImageWithChanges)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();
		CardImage imageToUpdate = await dbContext.CardImages.SingleAsync(cardImage => cardImage.Id == cardImageWithChanges.Id);

		if (!_cardImageComparer.Equals(imageToUpdate, cardImageWithChanges))
		{
			CardImageMapper.MergeProperties(imageToUpdate, cardImageWithChanges);
			await dbContext.SaveChangesAsync();
		}

		return imageToUpdate;
	}

	public async Task<int> Upsert(UpsertContainer<CardImage> upsertionData)
	{
		using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		if (upsertionData.ToInsert is { Count: > 0 }) dbContext.CardImages.AddRange(upsertionData.ToInsert);
		if (upsertionData.ToUpdate is { Count: > 0 }) dbContext.CardImages.UpdateRange(upsertionData.ToUpdate);

		int numberOfAffectedRows = await dbContext.SaveChangesAsync();

		return numberOfAffectedRows;
	}
}
