using CardAtlas.Server.Comparers;
using CardAtlas.Server.Models.Data;
using CardAtlas.UnitTests.DataHelpers;

namespace CardAtlas.UnitTests;

class ComparerTests
{
	private readonly CardEqualityComparer _comparer = new CardEqualityComparer();

	[Test]
	public void CardEqualityComparer_Equals_ExpectsMatchingObjectsTrue()
	{
		Card card1 = CardDataHelper.CreateCard();
		Card card2 = CardDataHelper.CreateCard();

		Assert.That(_comparer.Equals(card1, card2), Is.True);
	}
	
	[Test]
	public void CardEqualityComparer_Equals_ExpectsMatchingObjectsFalse()
	{
		Card card1 = CardDataHelper.CreateCard();
		Card card2 = CardDataHelper.CreateCard();
		card2.Name = "Different name";

		Assert.That(_comparer.Equals(card1, card2), Is.False);
	}
}
