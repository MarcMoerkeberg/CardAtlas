using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data.Image;
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
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		EntityEntry<CardImage> addedCardImage = await dbContext.CardImages.AddAsync(cardImage);
		await dbContext.SaveChangesAsync();

		return addedCardImage.Entity;
	}

	public async Task<CardImage> Get(long cardImageId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardImages.SingleAsync(cardImage => cardImage.Id == cardImageId);
	}

	public async Task<IEnumerable<CardImage>> GetFromCardId(long cardId)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		return await dbContext.CardImages
			.Include(ci => ci.ImageType)
			.Include(ci => ci.ImageFormat)
			.Include(ci => ci.ImageStatus)
			.Include(ci => ci.Source)
			.Where(cardImage => cardImage.CardId == cardId)
			.ToListAsync();
	}

	public async Task<CardImage> Update(CardImage cardImageWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

		CardImage imageToUpdate = await dbContext.CardImages.SingleAsync(cardImage => cardImage.Id == cardImageWithChanges.Id);
		CardImageMapper.MergeProperties(imageToUpdate, cardImageWithChanges);

		await dbContext.SaveChangesAsync();
		return imageToUpdate;
	}

	public async Task<CardImage> UpdateIfChanged(CardImage cardImageWithChanges)
	{
		ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();
		CardImage imageToUpdate = await dbContext.CardImages.SingleAsync(cardImage => cardImage.Id == cardImageWithChanges.Id);

		if (!_cardImageComparer.Equals(imageToUpdate, cardImageWithChanges))
		{
			CardImageMapper.MergeProperties(imageToUpdate, cardImageWithChanges);
			await dbContext.SaveChangesAsync();
		}

		return imageToUpdate;
	}
}
