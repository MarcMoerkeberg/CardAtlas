using CardAtlas.Server.Models.Data.Image;
using CardAtlas.Server.Services.Interfaces;

namespace CardAtlas.Server.Services;

public class CardImageService : ICardImageService
{
	public Task<CardImage> Create(CardImage cardImage)
	{
		throw new NotImplementedException();
	}

	public Task<CardImage> Get(long cardImageId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<CardImage>> GetFromCardId(long cardId)
	{
		throw new NotImplementedException();
	}

	public Task<CardImage> Update(CardImage cardImageWithChanges)
	{
		throw new NotImplementedException();
	}

	public Task<CardImage> UpdateIfChanged(CardImage cardImageWithChanges)
	{
		throw new NotImplementedException();
	}
}
