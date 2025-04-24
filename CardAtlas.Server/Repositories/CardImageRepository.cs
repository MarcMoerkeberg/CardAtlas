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
	private readonly ApplicationDbContext _dbContext;
	public CardImageRepository(
		IEqualityComparer<CardImage> cardImageComparer, 
		ApplicationDbContext dbContext)
	{
		_cardImageComparer = cardImageComparer;
		_dbContext = dbContext;
	}

	public async Task<CardImage> Create(CardImage cardImage)
	{
		EntityEntry<CardImage> addedCardImage = await _dbContext.CardImages.AddAsync(cardImage);
		await _dbContext.SaveChangesAsync();

		return addedCardImage.Entity;
	}

	public async Task<CardImage> Get(long cardImageId)
	{
		return await _dbContext.CardImages.SingleAsync(cardImage => cardImage.Id == cardImageId);
	}

	public async Task<IEnumerable<CardImage>> GetFromCardId(long cardId)
	{
		return await _dbContext.CardImages
			.Include(ci => ci.ImageType)
			.Include(ci => ci.ImageFormat)
			.Include(ci => ci.ImageStatus)
			.Include(ci => ci.Source)
			.Where(cardImage => cardImage.CardId == cardId)
			.ToListAsync();
	}

	public async Task<CardImage> Update(CardImage cardImageWithChanges)
	{
		CardImage imageToUpdate = await Get(cardImageWithChanges.Id);
		CardImageMapper.MergeProperties(imageToUpdate, cardImageWithChanges);

		await _dbContext.SaveChangesAsync();
		return imageToUpdate;
	}

	public async Task<CardImage> UpdateIfChanged(CardImage cardImageWithChanges)
	{
		CardImage imageToUpdate = await Get(cardImageWithChanges.Id);

		if (!_cardImageComparer.Equals(imageToUpdate, cardImageWithChanges))
		{
			CardImageMapper.MergeProperties(imageToUpdate, cardImageWithChanges);
			await _dbContext.SaveChangesAsync();
		}

		return imageToUpdate;
	}
}
