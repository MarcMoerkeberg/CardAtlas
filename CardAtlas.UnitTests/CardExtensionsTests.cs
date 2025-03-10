using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.UnitTests.DataHelpers;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.UnitTests;

class CardExtensionsTests
{
	[Test]
	public void FindMatchingCard_ByCardFace_ExpectsNull_WhenCollectionIsEmpty()
	{
		IEnumerable<Card> targetCollection = new List<Card>();
		CardFace cardFace = CardDataHelper.CreateCardFace();
		
		Card? result = targetCollection.FindMatchingCard(cardFace);
		
		Assert.That(result, Is.Null);
	}
	
	[Test]
	public void FindMatchingCard_ByCardFace_ExpectsSingleCard_WhenCollectionIsPopulated()
	{
		string expectedName = "First card";

		IEnumerable<Card> targetCollection = new List<Card>
		{
			CardDataHelper.CreateCard(name: expectedName),
			CardDataHelper.CreateCard(name: "Second card"),
		};

		CardFace cardFace = CardDataHelper.CreateCardFace(name: expectedName);

		Card? result = targetCollection.FindMatchingCard(cardFace);
		
		Assert.That(result, Is.Not.Null);
		Assert.That(result?.Name, Is.EqualTo(expectedName));
	}
	
	[Test]
	public void FindMatchingCard_ByCardFace_ExpectsFirstCard_WhenCollectionContainsMultipleMatches()
	{
		string expectedName = "First card";

		IEnumerable<Card> targetCollection = new List<Card>
		{
			CardDataHelper.CreateCard(name: expectedName),
			CardDataHelper.CreateCard(name: expectedName),
		};

		CardFace cardFace = CardDataHelper.CreateCardFace(name: expectedName);

		Card? result = targetCollection.FindMatchingCard(cardFace);
		
		Assert.That(result, Is.Not.Null);
		Assert.That(result?.Name, Is.EqualTo(expectedName));
		Assert.That(targetCollection.First(), Is.EqualTo(result));
	}
	
	[Test]
	public void FindMatchingCard_ByCardFace_ExpectsNull_WhenCollectionContainsNoMatches()
	{
		IEnumerable<Card> targetCollection = new List<Card>
		{
			CardDataHelper.CreateCard(),
			CardDataHelper.CreateCard(),
		};

		CardFace cardFace = CardDataHelper.CreateCardFace(name: "Name with no matches");

		Card? result = targetCollection.FindMatchingCard(cardFace);

		Assert.That(result, Is.Null);
	}
}
