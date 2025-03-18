using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.UnitTests.DataHelpers;

namespace CardAtlas.UnitTests.ExtensionTests;

class IEnumerableExtensionsTests
{
	[Test]
	public void ExistsWithName_ExpectsFalse_WhenCollectionIsEmpty()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>();

		bool result = targetCollection.ExistsWithName("Some format name");

		Assert.That(result, Is.False);
	}

	[Test]
	[TestCase(null)]
	[TestCase("")]
	[TestCase(" ")]
	public void ExistsWithName_ExpectsFalse_WhenSearchNameIsNullEmptyOrWhitespace(string? searchName)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat()
		};

		bool result = targetCollection.ExistsWithName(searchName);

		Assert.That(result, Is.False);
	}

	[Test]
	public void ExistsWithName_ExpectsTrue_WhenCollectionIsPopulated()
	{
		string searchName = "Format name";
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: searchName)
		};

		bool result = targetCollection.ExistsWithName(searchName);

		Assert.That(result, Is.True);
	}

	[Test]
	[TestCase("Format name")]
	[TestCase("format name")]
	public void ExistsWithName_ExpectsTrue_WhenNameOnlyDiffersInCasing(string searchName)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name")
		};

		bool result = targetCollection.ExistsWithName(searchName);

		Assert.That(result, Is.True);
	}

	[Test]
	public void ExistsWithName_ExpectsFalse_WhenNoFormatWithMatchingNameExists()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name 1"),
			GameDataHelper.CreateGameFormat(formatName: "Format name 2"),
			GameDataHelper.CreateGameFormat(formatName: "Format name 3"),
			GameDataHelper.CreateGameFormat(formatName: "Format name 4")
		};

		bool result = targetCollection.ExistsWithName("Format name");

		Assert.That(result, Is.False);
	}

	[Test]
	public void ExistsWithName_ExpectsFalse_WhenCollectionIsEmpty_UsingOverloadWithSource()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>();

		bool result = targetCollection.ExistsWithName("Some format name", SourceType.Scryfall);

		Assert.That(result, Is.False);
	}

	[Test]
	[TestCase(null)]
	[TestCase("")]
	[TestCase(" ")]
	public void ExistsWithName_ExpectsFalse_WhenSearchNameIsNullEmptyOrWhitespace_UsingOverloadWithSource(string? searchName)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(source: SourceType.Scryfall)
		};

		bool result = targetCollection.ExistsWithName(searchName, SourceType.Scryfall);

		Assert.That(result, Is.False);
	}

	[Test]
	public void ExistsWithName_ExpectsTrue_WhenCollectionIsPopulated_UsingOverloadWithSource()
	{
		string searchName = "Format name";
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: searchName, SourceType.Scryfall)
		};

		bool result = targetCollection.ExistsWithName(searchName, SourceType.Scryfall);

		Assert.That(result, Is.True);
	}

	[Test]
	[TestCase("Format name")]
	[TestCase("format name")]
	public void ExistsWithName_ExpectsTrue_WhenNameOnlyDiffersInCasing_UsingOverloadWithSource(string searchName)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name", SourceType.Scryfall)
		};

		bool result = targetCollection.ExistsWithName(searchName, SourceType.Scryfall);

		Assert.That(result, Is.True);
	}

	[Test]
	[TestCase("Format name", SourceType.User)]
	[TestCase("Formatname", SourceType.Scryfall)]
	public void ExistsWithName_ExpectsFalse_WhenNoFormatMatchExists_UsingOverloadWithSource(string formatName, SourceType source)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName, source),
			GameDataHelper.CreateGameFormat(formatName: "Format name 1", source),
			GameDataHelper.CreateGameFormat(formatName: "Format name 2", source),
		};

		bool result = targetCollection.ExistsWithName("Format name", SourceType.Scryfall);

		Assert.That(result, Is.False);
	}

	[Test]
	public void FirstWithNameOrDefault_ExpectsNull_WhenCollectionIsEmpty()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>();

		GameFormat? result = targetCollection.FirstWithNameOrDefault("Some format name");

		Assert.That(result, Is.Null);
	}

	[Test]
	[TestCase(null)]
	[TestCase("")]
	[TestCase(" ")]
	public void FirstWithNameOrDefault_ExpectsNull_WhenSearchNameIsNullEmptyOrWhitespace(string? searchName)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat()
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault(searchName);

		Assert.That(result, Is.Null);
	}

	[Test]
	public void FirstWithNameOrDefault_ExpectsGameFormat_WhenCollectionIsPopulated()
	{
		string searchName = "Format name";
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: searchName)
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault(searchName);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Name, Is.EqualTo(searchName));
	}

	[Test]
	[TestCase("Format name")]
	[TestCase("format name")]
	public void FirstWithNameOrDefault_ExpectsGameFormat_WhenNameOnlyDiffersInCasing(string searchName)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name")
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault(searchName);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Name, Is.EqualTo(targetCollection.First().Name));
	}

	[Test]
	public void FirstWithNameOrDefault_ExpectsNull_WhenNoFormatWithMatchingNameExists()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name 1"),
			GameDataHelper.CreateGameFormat(formatName: "Format name 2"),
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault("Format name");

		Assert.That(result, Is.Null);
	}

	[Test]
	public void FirstWithNameOrDefault_ExpectsFirstGameFormat_WhenMultipleMatchesExists()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name"),
			GameDataHelper.CreateGameFormat(formatName: "Format name"),
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault("Format name");

		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(targetCollection.First()));
	}

	[Test]
	public void FirstWithNameOrDefault_ExpectsNull_WhenCollectionIsEmpty_UsingOverloadWithSource()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>();

		GameFormat? result = targetCollection.FirstWithNameOrDefault("Some format name", SourceType.Scryfall);

		Assert.That(result, Is.Null);
	}

	[Test]
	[TestCase(null)]
	[TestCase("")]
	[TestCase(" ")]
	public void FirstWithNameOrDefault_ExpectsNull_WhenSearchNameIsNullEmptyOrWhitespace_UsingOverloadWithSource(string? searchName)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(source: SourceType.Scryfall)
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault(searchName, SourceType.Scryfall);

		Assert.That(result, Is.Null);
	}

	[Test]
	public void FirstWithNameOrDefault_ExpectsGameFormat_WhenCollectionIsPopulated_UsingOverloadWithSource()
	{
		string searchName = "Format name";
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: searchName, SourceType.Scryfall)
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault(searchName, SourceType.Scryfall);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Name, Is.EqualTo(searchName));
	}

	[Test]
	[TestCase("Format name")]
	[TestCase("format name")]
	public void FirstWithNameOrDefault_ExpectsGameFormat_WhenNameOnlyDiffersInCasing_UsingOverloadWithSource(string searchName)
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name", SourceType.Scryfall)
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault(searchName, SourceType.Scryfall);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Name, Is.EqualTo(targetCollection.First().Name));
		Assert.That(result.SourceId, Is.EqualTo(targetCollection.First().SourceId));
	}

	[Test]
	public void FirstWithNameOrDefault_ExpectsNull_WhenNoFormatWithMatchingNameOrSourceExists_UsingOverloadWithSource()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name", SourceType.User),
			GameDataHelper.CreateGameFormat(formatName: "Format name 1", SourceType.Scryfall),
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault("Format name", SourceType.Scryfall);

		Assert.That(result, Is.Null);
	}

	[Test]
	public void FirstWithNameOrDefault_ExpectsFirstGameFormat_WhenMultipleMatchesExists_UsingOverloadWithSource()
	{
		IEnumerable<GameFormat> targetCollection = new List<GameFormat>
		{
			GameDataHelper.CreateGameFormat(formatName: "Format name", SourceType.Scryfall),
			GameDataHelper.CreateGameFormat(formatName: "Format name", SourceType.Scryfall),
		};

		GameFormat? result = targetCollection.FirstWithNameOrDefault("Format name", SourceType.Scryfall);

		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(targetCollection.First()));
	}
}
