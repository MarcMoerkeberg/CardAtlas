using CardAtlas.Server.Comparers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.UnitTests.DataHelpers;

namespace CardAtlas.UnitTests;

class ComparerTests
{
	private readonly CardComparer _cardComparer = new CardComparer();
	private readonly SetComparer _setComparer = new SetComparer();
	private readonly CardImageComparer _cardImageComparer = new CardImageComparer();

	[Test]
	public void CardEqualityComparer_Equals_ExpectsMatchingObjectsTrue()
	{
		Card card1 = CardDataHelper.CreateCard();
		Card card2 = CardDataHelper.CreateCard();

		Assert.That(_cardComparer.Equals(card1, card2), Is.True);
	}

	[Test]
	public void CardEqualityComparer_Equals_ExpectsMatchingObjectsFalse()
	{
		Card card1 = CardDataHelper.CreateCard();
		Card card2 = CardDataHelper.CreateCard();
		card2.Name = "Different name";

		Assert.That(_cardComparer.Equals(card1, card2), Is.False);
	}

	[Test]
	public void SetEqualityComparer_Equals_ExpectsMatchingObjectsTrue()
	{
		Set set1 = SetDataHelper.CreateSet();
		Set set2 = SetDataHelper.CreateSet();

		Assert.That(_setComparer.Equals(set1, set2), Is.True);
	}

	[Test]
	public void SetEqualityComparer_Equals_ExpectsMatchingObjectsFalse()
	{
		Set set1 = SetDataHelper.CreateSet();
		Set set2 = SetDataHelper.CreateSet();
		set2.Name = "Different name";

		Assert.That(_setComparer.Equals(set1, set2), Is.False);
	}

	[Test]
	public void ImageEqualityComparer_Equals_ExpectsMatchingObjectsTrue()
	{
		CardImage cardImage1 = CardImageDataHelper.CreateCardImage();
		CardImage cardImage2 = CardImageDataHelper.CreateCardImage();

		Assert.That(_cardImageComparer.Equals(cardImage1, cardImage2), Is.True);
	}
	
	[Test]
	public void ImageEqualityComparer_Equals_ExpectsMatchingObjectsFalse()
	{
		CardImage cardImage1 = CardImageDataHelper.CreateCardImage();
		CardImage cardImage2 = CardImageDataHelper.CreateCardImage();
		cardImage2.Uri = new Uri("https://differentUri.com");

		Assert.That(_cardImageComparer.Equals(cardImage1, cardImage2), Is.False);
	}
}
