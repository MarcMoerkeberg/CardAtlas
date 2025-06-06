﻿using CardAtlas.Server.Comparers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.UnitTests.DataHelpers;

namespace CardAtlas.UnitTests;

class ComparerTests
{
	private readonly CardComparer _cardComparer = new CardComparer();
	private readonly SetComparer _setComparer = new SetComparer();
	private readonly CardImageComparer _cardImageComparer = new CardImageComparer();
	private readonly CardPriceComparer _cardPriceComparer = new CardPriceComparer();
	private readonly CardLegalityComparer _cardLegalityComparer = new CardLegalityComparer();

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

	[Test]
	public void CardPriceComparer_Equals_ExpectsMatchingObjectsTrue()
	{
		CardPrice cardPrice1 = CardDataHelper.CreateCardPrice();
		CardPrice cardPrice2 = CardDataHelper.CreateCardPrice();

		Assert.That(_cardPriceComparer.Equals(cardPrice1, cardPrice2), Is.True);
	}

	[Test]
	public void CardPriceComparer_Equals_ExpectsMatchingObjectsFalse()
	{
		CardPrice cardPrice1 = CardDataHelper.CreateCardPrice(cardId: 1);
		CardPrice cardPrice2 = CardDataHelper.CreateCardPrice(cardId: 2);

		Assert.That(_cardPriceComparer.Equals(cardPrice1, cardPrice2), Is.False);
	}

	[Test]
	public void CardLegalityComparer_Equals_ExpectsMatchingObjectsTrue()
	{
		CardLegality cardLegality1 = CardDataHelper.CreateCardLegality();
		CardLegality cardLegality2 = CardDataHelper.CreateCardLegality();

		Assert.That(_cardLegalityComparer.Equals(cardLegality1, cardLegality2), Is.True);
	}

	[Test]
	public void CardLegalityComparer_Equals_ExpectsMatchingObjectsFalse()
	{
		CardLegality cardLegality1 = CardDataHelper.CreateCardLegality();
		CardLegality cardLegality2 = CardDataHelper.CreateCardLegality();
		cardLegality2.Id = 2;

		Assert.That(_cardLegalityComparer.Equals(cardLegality1, cardLegality2), Is.False);
	}
}
