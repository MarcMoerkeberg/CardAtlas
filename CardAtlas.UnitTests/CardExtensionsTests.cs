using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Cards;
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
		
		Card? result = targetCollection.FindMatchByName(cardFace);
		
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

		Card? result = targetCollection.FindMatchByName(cardFace);
		
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

		Card? result = targetCollection.FindMatchByName(cardFace);
		
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

		Card? result = targetCollection.FindMatchByName(cardFace);

		Assert.That(result, Is.Null);
	}

	[Test]
	public void FindMatchByVendorAndCurrency_ExpectsNull_WhenCollectionIsEmpty()
	{
		IEnumerable<CardPrice> targetCollection = new List<CardPrice>();
		CardPrice cardPrice = CardDataHelper.CreateCardPrice();

		CardPrice? result = targetCollection.FindMatchByVendorAndCurrency(cardPrice);

		Assert.That(result, Is.Null);
	}

	[Test]
	[TestCase(2, VendorType.CardMarket, CurrencyType.Eur)]
	[TestCase(1, VendorType.TcgPlayer, CurrencyType.Eur)]
	[TestCase(1, VendorType.CardMarket, CurrencyType.Usd)]
	public void FindMatchByTypeAndSource_ExpectsNull_WhenMatchPropertyDiffers(long cardId, VendorType vendor, CurrencyType currency)
	{
		IEnumerable<CardPrice> targetCollection = new List<CardPrice>
		{
			CardDataHelper.CreateCardPrice(cardId, vendor, currency)
		};

		CardPrice searchPrice = CardDataHelper.CreateCardPrice(cardId: 1, vendor: VendorType.CardMarket, currency: CurrencyType.Eur);

		CardPrice? result = targetCollection.FindMatchByVendorAndCurrency(searchPrice);

		Assert.That(result, Is.Null);
	}

	[Test]
	public void FindMatchByTypeAndSource_ExpectsSingleCard_WhenCollectionIsPopulated()
	{
		IEnumerable<CardPrice> targetCollection = new List<CardPrice>
		{
			CardDataHelper.CreateCardPrice(),
			CardDataHelper.CreateCardPrice(cardId: 2),
		};

		CardPrice searchPrice = CardDataHelper.CreateCardPrice();

		CardPrice? result = targetCollection.FindMatchByVendorAndCurrency(searchPrice);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.VendorId, Is.EqualTo(searchPrice.VendorId));
		Assert.That(result.CurrencyId, Is.EqualTo(searchPrice.CurrencyId));
		Assert.That(result.CardId, Is.EqualTo(searchPrice.CardId));
	}

	[Test]
	public void FindMatchByTypeAndSource_ExpectsFirstInstance_WhenCollectionHasMulitpleMatches()
	{
		IEnumerable<CardPrice> targetCollection = new List<CardPrice>
		{
			CardDataHelper.CreateCardPrice(cardId: 1, vendor: VendorType.CardMarket, currency: CurrencyType.Eur),
			CardDataHelper.CreateCardPrice(),
			CardDataHelper.CreateCardPrice(cardId: 1, vendor: VendorType.CardMarket, currency: CurrencyType.Eur),
		};

		CardPrice searchPrice = CardDataHelper.CreateCardPrice(cardId: 1, vendor: VendorType.CardMarket, currency: CurrencyType.Eur);

		CardPrice? result = targetCollection.FindMatchByVendorAndCurrency(searchPrice);

		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.SameAs(targetCollection.First()));
	}
}
