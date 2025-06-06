using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.UnitTests.DataHelpers;

namespace CardAtlas.UnitTests.ExtensionTests;

internal class IngestionTests
{
	[Test]
	[TestCase(true)]
	[TestCase(false)]
	public void AssignCardIdToEntities_GuidList_WhenScryfallIdIsNullOrEmpty_ShouldNotAssignCardIdToAnyEntities(bool emptyScryfallId)
	{
		int cardId = 1;
		Guid? scryfallId = emptyScryfallId ? Guid.Empty : null;

		Card card = CardDataHelper.CreateCard(cardId, scryfallId);
		CardKeyword cardKeyword = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<Guid, List<CardKeyword>>
		{
			[Guid.Empty] = new List<CardKeyword> { cardKeyword }
		};

		batchedEntities.AssignCardIdToEntities([card]);
		CardKeyword itemAfterUpdate = batchedEntities[Guid.Empty].Single();

		Assert.That(
			itemAfterUpdate.CardId,
			Is.EqualTo(0),
			"CardId should remain 0, because ScryfallId was null or empty guid."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidList_WhenDictionaryIsEmpty_DoesNothing()
	{
		Card card = CardDataHelper.CreateCard();

		var batchedEntities = new Dictionary<Guid, List<CardKeyword>>();

		batchedEntities.AssignCardIdToEntities([card]);

		Assert.That(
			batchedEntities.Count,
			Is.EqualTo(0),
			"No data in batched entities, should still be able to call method, but have nothing happen."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidList_WhenScryfallIdHasNoMatch_ShouldNotAssignCardIdToAnyEntities()
	{
		int cardId = 1;
		Guid scryfallId = Guid.NewGuid();

		Card card = CardDataHelper.CreateCard(cardId, scryfallId);
		CardKeyword cardKeyword = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<Guid, List<CardKeyword>>
		{
			[Guid.Empty] = new List<CardKeyword> { cardKeyword }
		};

		batchedEntities.AssignCardIdToEntities([card]);
		CardKeyword itemAfterUpdate = batchedEntities[Guid.Empty].Single();

		Assert.That(
			itemAfterUpdate.CardId,
			Is.EqualTo(0),
			"CardId should remain 0, because batched entities had no key matching the card's ScryfallId."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidList_WhenScryfallIdHasMatch_ShouldAssignCardIdToAllMatchingEntities()
	{
		int cardId = 1;
		Guid scryfallId = Guid.NewGuid();
		Card card = CardDataHelper.CreateCard(cardId, scryfallId);

		CardKeyword cardKeyword1 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeyword2 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeywordNoMatch1 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeywordNoMatch2 = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<Guid, List<CardKeyword>>
		{
			[scryfallId] = new List<CardKeyword> { cardKeyword1, cardKeyword2 },
			[Guid.Empty] = new List<CardKeyword> { cardKeywordNoMatch1, cardKeywordNoMatch2 }
		};

		batchedEntities.AssignCardIdToEntities([card]);
		List<CardKeyword> entitiesWithUpdatedCardId = batchedEntities[scryfallId];
		List<CardKeyword> entitiesWithoutUpdatedCardId = batchedEntities[Guid.Empty];

		Assert.That(
			entitiesWithUpdatedCardId.Select(ck => ck.CardId),
			Is.All.EqualTo(cardId),
			"Every entity in this list should have assigned cardId, since they matched the card's ScryfallId."
		);
		Assert.That(
			entitiesWithoutUpdatedCardId.Select(ck => ck.CardId),
			Is.All.EqualTo(0),
			"No entities in this list should have altered the CardId, since the card's ScryfallId did not match."
		);
	}

	[Test]
	[TestCase(true)]
	[TestCase(false)]
	public void AssignCardIdToEntities_GuidStringList_WhenScryfallIdIsNullOrEmpty_ShouldNotAssignCardIdToAnyEntities(bool emptyScryfallId)
	{
		int cardId = 1;
		string cardName = "Card Name";
		Guid? scryfallId = emptyScryfallId ? Guid.Empty : null;

		Card card = CardDataHelper.CreateCard(cardId, scryfallId, cardName);
		CardKeyword cardKeyword = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<(Guid scryfallId, string cardName), List<CardKeyword>>
		{
			[(Guid.Empty, cardName)] = new List<CardKeyword> { cardKeyword },
		};

		batchedEntities.AssignCardIdToEntities([card]);
		CardKeyword itemAfterUpdate = batchedEntities[(Guid.Empty, cardName)].Single();

		Assert.That(
			itemAfterUpdate.CardId,
			Is.EqualTo(0),
			"CardId should remain 0, because ScryfallId was null or empty guid."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidStringList_WhenDictionaryIsEmpty_DoesNothing()
	{
		Card card = CardDataHelper.CreateCard();

		var batchedEntities = new Dictionary<(Guid scryfallId, string cardName), List<CardKeyword>>();

		batchedEntities.AssignCardIdToEntities([card]);

		Assert.That(
			batchedEntities.Count,
			Is.EqualTo(0),
			"No data in batched entities, should still be able to call method, but have nothing happen."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidStringList_WhenScryfallIdOrNameHasNoMatch_ShouldNotAssignCardIdToAnyEntities()
	{
		int cardId = 1;
		string cardName = "Card name";
		Guid scryfallId = Guid.NewGuid();

		Card card = CardDataHelper.CreateCard(cardId, scryfallId, cardName);
		CardKeyword cardKeyword = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<(Guid scryfallId, string cardName), List<CardKeyword>>
		{
			[(scryfallId, "Some other card name")] = new List<CardKeyword> { cardKeyword },
			[(Guid.Empty, cardName)] = new List<CardKeyword> { cardKeyword },
		};

		batchedEntities.AssignCardIdToEntities([card]);
		CardKeyword itemAfterUpdate1 = batchedEntities[(scryfallId, "Some other card name")].Single();
		CardKeyword itemAfterUpdate2 = batchedEntities[(Guid.Empty, cardName)].Single();

		Assert.That(
			itemAfterUpdate1.CardId,
			Is.EqualTo(0),
			"CardId should remain 0, because batched entities's key did not match the card's Name."
		);

		Assert.That(
			itemAfterUpdate2.CardId,
			Is.EqualTo(0),
			"CardId should remain 0, because the batched entities's key did not match the card's ScryfallId."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidStringList_WhenScryfallIdAndNameHasMatch_ShouldAssignCardIdToAllMatchingEntities()
	{
		int cardId = 1;
		string cardName = "Card name";
		Guid scryfallId = Guid.NewGuid();
		Card card = CardDataHelper.CreateCard(cardId, scryfallId, cardName);

		CardKeyword cardKeyword1 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeyword2 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeywordNoMatch1 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeywordNoMatch2 = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<(Guid scryfallId, string cardName), List<CardKeyword>>
		{
			[(scryfallId, cardName)] = new List<CardKeyword> { cardKeyword1, cardKeyword2 },
			[(Guid.Empty, cardName)] = new List<CardKeyword> { cardKeywordNoMatch1, cardKeywordNoMatch2 }
		};

		batchedEntities.AssignCardIdToEntities([card]);
		List<CardKeyword> entitiesWithUpdatedCardId = batchedEntities[(scryfallId, cardName)];
		List<CardKeyword> entitiesWithoutUpdatedCardId = batchedEntities[(Guid.Empty, cardName)];

		Assert.That(
			entitiesWithUpdatedCardId.Select(ck => ck.CardId),
			Is.All.EqualTo(cardId),
			"Every entity in this list should have assigned cardId, since they matched the card's ScryfallId and Name."
		);
		Assert.That(
			entitiesWithoutUpdatedCardId.Select(ck => ck.CardId),
			Is.All.EqualTo(0),
			"No entities in this list should have altered the CardId, since the card's ScryfallId or Name did not match."
		);
	}

	[Test]
	[TestCase(true)]
	[TestCase(false)]
	public void AssignCardIdToEntities_GuidListOfTuples_WhenScryfallIdIsNullOrEmpty_ShouldNotAssignCardIdToAnyEntities(bool emptyScryfallId)
	{
		int cardId = 1;
		Guid? scryfallId = emptyScryfallId ? Guid.Empty : null;
		string keywordName = "KeywordName";

		Card card = CardDataHelper.CreateCard(cardId, scryfallId);
		CardKeyword cardKeyword = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<Guid, List<(string cardName, CardKeyword relation)>>
		{
			[Guid.Empty] = new List<(string keywordName, CardKeyword relation)> { (keywordName, cardKeyword) },
		};

		batchedEntities.AssignCardIdToEntities([card]);
		(string cardName, CardKeyword relation) itemAfterUpdate = batchedEntities[Guid.Empty].Single();

		Assert.That(
			itemAfterUpdate.relation.CardId,
			Is.EqualTo(0),
			"CardId should remain 0, because ScryfallId was null or empty guid."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidListOfTuples_WhenDictionaryIsEmpty_DoesNothing()
	{
		Card card = CardDataHelper.CreateCard();

		var batchedEntities = new Dictionary<Guid, List<(string cardName, CardKeyword relation)>>();

		batchedEntities.AssignCardIdToEntities([card]);

		Assert.That(
			batchedEntities.Count,
			Is.EqualTo(0),
			"No data in batched entities, should still be able to call method, but have nothing happen."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidListOfTuples_WhenScryfallIdHasNoMatch_ShouldNotAssignCardIdToAnyEntities()
	{
		int cardId = 1;
		Guid scryfallId = Guid.NewGuid();
		string keywordName = "KeywordName";

		Card card = CardDataHelper.CreateCard(cardId, scryfallId);
		CardKeyword cardKeyword = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<Guid, List<(string cardName, CardKeyword relation)>>
		{
			[Guid.Empty] = new List<(string keywordName, CardKeyword relation)> { (keywordName, cardKeyword) },
		};

		batchedEntities.AssignCardIdToEntities([card]);
		(string cardName, CardKeyword relation) itemAfterUpdate1 = batchedEntities[Guid.Empty].Single();

		Assert.That(
			itemAfterUpdate1.relation.CardId,
			Is.EqualTo(0),
			"CardId should remain 0, because batched entities's key did not match the card's Name."
		);
	}

	[Test]
	public void AssignCardIdToEntities_GuidListOfTuples_WhenScryfallIdAndNameHasMatch_ShouldAssignCardIdToAllMatchingEntities()
	{
		int cardId = 1;
		Guid scryfallId = Guid.NewGuid();
		Card card = CardDataHelper.CreateCard(cardId, scryfallId);
		string keywordName1 = "KeywordName";
		string keywordName2 = "SomeKeywordOtherName";

		CardKeyword cardKeyword1 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeyword2 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeywordNoMatch1 = CardDataHelper.CreateCardKeyword(cardId: 0);
		CardKeyword cardKeywordNoMatch2 = CardDataHelper.CreateCardKeyword(cardId: 0);

		var batchedEntities = new Dictionary<Guid, List<(string cardName, CardKeyword relation)>>
		{
			[scryfallId] = new List<(string keywordName, CardKeyword relation)> { (keywordName1, cardKeyword1), (keywordName2, cardKeyword2) },
			[Guid.Empty] = new List<(string keywordName, CardKeyword relation)> { (keywordName2, cardKeywordNoMatch1), (keywordName2, cardKeywordNoMatch2) },
		};

		batchedEntities.AssignCardIdToEntities([card]);
		List<(string cardName, CardKeyword relation)> entitiesWithUpdatedCardId = batchedEntities[scryfallId];
		List<(string cardName, CardKeyword relation)> entitiesWithoutUpdatedCardId = batchedEntities[Guid.Empty];

		Assert.That(
			entitiesWithUpdatedCardId.Select(ck => ck.relation.CardId),
			Is.All.EqualTo(cardId),
			"Every entity in this list should have assigned cardId, since they matched the card's ScryfallId and Name."
		);
		Assert.That(
			entitiesWithoutUpdatedCardId.Select(ck => ck.relation.CardId),
			Is.All.EqualTo(0),
			"No entities in this list should have altered the CardId, since the card's ScryfallId or Name did not match."
		);
	}

	[Test]
	public void AssignRelationalIdToEntities_WhenDictionaryEmpty_DoesNothing()
	{
		Keyword keyword = CardDataHelper.CreateKeyword();
		var batchedEntities = new Dictionary<Guid, List<(string, CardKeyword)>>();

		batchedEntities.AssignRelationalIdToEntities(
			entitiesWithIds: [keyword],
			assignId: (cardKeyword, keywordId) => cardKeyword.KeywordId = keywordId
		);

		Assert.That(
			batchedEntities.Count,
			Is.EqualTo(0),
			"Should still be able to call method even though there is no entities batched, nothing should happen."
		);
	}

	[Test]
	public void AssignRelationalIdToEntities_WhenNoMatchingName_ShouldNotAssignRelationalId()
	{
		int keywordId = 1;
		Guid someScryfallId = Guid.NewGuid();

		Keyword keyword = CardDataHelper.CreateKeyword(keywordId, "Flying");
		CardKeyword cardKeyword = CardDataHelper.CreateCardKeyword(keywordId: 0);

		var batchedData = new Dictionary<Guid, List<(string, CardKeyword)>>
		{
			[someScryfallId] = new List<(string KeywordName, CardKeyword Relation)> { ("Haste", cardKeyword) }
		};

		batchedData.AssignRelationalIdToEntities(
			entitiesWithIds: [keyword],
			assignId: (cardKeyword, keywordID) => cardKeyword.KeywordId = keywordID
		);

		Assert.That(
			cardKeyword.KeywordId,
			Is.EqualTo(0),
			"KeywordId should remain 0, since there is not provided any entities with ids that match the tuple name."
		);
	}

	[Test]
	public void AssignRelationalIdToEntities_WhenMatchingName_ShouldAssignRelationalId()
	{
		Guid someScryfallId1 = Guid.NewGuid();
		Guid someScryfallId2 = Guid.NewGuid();

		Keyword keyword1 = CardDataHelper.CreateKeyword(id: 1, name: "Flying");
		Keyword keyword2 = CardDataHelper.CreateKeyword(id: 2, name: "Haste");

		CardKeyword cardKeyword1 = CardDataHelper.CreateCardKeyword(keywordId: 0);
		CardKeyword cardKeyword2 = CardDataHelper.CreateCardKeyword(keywordId: 0);
		CardKeyword cardKeyword3 = CardDataHelper.CreateCardKeyword(keywordId: 0);

		var batchedData = new Dictionary<Guid, List<(string, CardKeyword)>>
		{
			[someScryfallId1] = new List<(string KeywordName, CardKeyword Relation)>
			{
				("flying", cardKeyword1),
				("haste", cardKeyword2)
			},
			[someScryfallId2] = new List<(string KeywordName, CardKeyword Relation)>
			{
				("haste", cardKeyword3)
			}
		};

		batchedData.AssignRelationalIdToEntities(
			entitiesWithIds: [keyword1, keyword2],
			assignId: (cardKeyword, keywordId) => cardKeyword.KeywordId = keywordId
		);

		Assert.That(
			cardKeyword1.KeywordId,
			Is.EqualTo(1),
			"This CardKeyword should have KeywordId 1, since it's coupled with the name the keyword which name is Flying."
		);

		Assert.That(
			cardKeyword2.KeywordId,
			Is.EqualTo(2),
			"This CardKeyword should have KeywordId 2, since it's coupled with the name the keyword which name is Haste."
		);

		Assert.That(
			cardKeyword3.KeywordId,
			Is.EqualTo(2),
			"This CardKeyword should have KeywordId 1, since it's coupled with the name the keyword which name is Flying."
		);
	}
}
