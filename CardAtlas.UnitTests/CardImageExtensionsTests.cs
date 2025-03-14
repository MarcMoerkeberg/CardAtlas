using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.UnitTests.DataHelpers;

namespace CardAtlas.UnitTests;

class CardImageExtensionsTests
{
	[Test]
	public void FindMatchByTypeAndSource_ExpectsNull_WhenCollectionIsEmpty()
	{
		IEnumerable<CardImage> targetCollection = new List<CardImage>();
		CardImage searchImage = CardImageDataHelper.CreateCardImage();

		CardImage? result = targetCollection.FindMatchByTypeAndSource(searchImage);
		
		Assert.That(result, Is.Null);
	}

	[Test]
	[TestCase(2, SourceType.Scryfall, ImageTypeKind.Normal)]
	[TestCase(1, SourceType.User, ImageTypeKind.Normal)]
	[TestCase(1, SourceType.Scryfall, ImageTypeKind.Small)]
	public void FindMatchByTypeAndSource_ExpectsNull_WhenMatchPropertyDiffers(long cardId, SourceType source, ImageTypeKind imageType)
	{
		IEnumerable<CardImage> targetCollection = new List<CardImage>
		{
			CardImageDataHelper.CreateCardImage(cardId, source, imageType),
		};

		CardImage searchImage = CardImageDataHelper.CreateCardImage(cardId: 1, source: SourceType.Scryfall, imageType: ImageTypeKind.Normal);

		CardImage? result = targetCollection.FindMatchByTypeAndSource(searchImage);

		Assert.That(result, Is.Null);
	}

	[Test]
	public void FindMatchByTypeAndSource_ExpectsSingleCard_WhenCollectionIsPopulated()
	{
		var expectedImageType = ImageTypeKind.Normal;

		IEnumerable<CardImage> targetCollection = new List<CardImage>
		{
			CardImageDataHelper.CreateCardImage(imageType: ImageTypeKind.Small),
			CardImageDataHelper.CreateCardImage(imageType: expectedImageType),
			CardImageDataHelper.CreateCardImage(imageType: ImageTypeKind.Large),
		};

		CardImage searchImage = CardImageDataHelper.CreateCardImage(imageType: expectedImageType);

		CardImage? result = targetCollection.FindMatchByTypeAndSource(searchImage);

		Assert.That(result, Is.Not.Null);
		Assert.That(result?.ImageTypeId, Is.EqualTo(searchImage.ImageTypeId));
	}
	
	[Test]
	public void FindMatchByTypeAndSource_ExpectsFirstInstance_WhenCollectionHasMulitpleMatches()
	{
		var expectedImageType = ImageTypeKind.Normal;

		IEnumerable<CardImage> targetCollection = new List<CardImage>
		{
			CardImageDataHelper.CreateCardImage(imageType: expectedImageType),
			CardImageDataHelper.CreateCardImage(imageType: expectedImageType),
		};

		CardImage searchImage = CardImageDataHelper.CreateCardImage(imageType: expectedImageType);

		CardImage? result = targetCollection.FindMatchByTypeAndSource(searchImage);

		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(targetCollection.First()));
	}
}
