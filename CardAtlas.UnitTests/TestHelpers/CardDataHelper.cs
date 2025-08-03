using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardFace = ScryfallApi.Models.CardFace;
using ImageUris = ScryfallApi.Models.ImageUris;

namespace CardAtlas.UnitTests.DataHelpers;

public static class CardDataHelper
{
	public static List<Card> CreateCards(int count)
	{
		List<Card> cards = new();

		for (int i = 0; i < count; i++)
		{
			cards.Add(CreateCard(cardId: i + 1));
		}

		return cards;
	}

	public static Card CreateCard(long cardId = 1, Guid? scryfallId = null, string name = "Card name") => new Card
	{
		Id = cardId,
		Name = name,
		OracleText = "Oracle text.",
		TypeLine = "Creature - Creature subtype",
		FlavorText = "Flavor text.",
		ManaCost = "{1}{W}",
		ConvertedManaCost = 2,
		Power = "2",
		Toughness = "2",
		Loyalty = null,
		ScryfallId = scryfallId,
		CollectorNumber = "1",
		ReleaseDate = new DateOnly(2025, 1, 1),
		IsOnReservedList = false,
		CanBeFoundInBoosters = true,
		IsDigitalOnly = false,
		IsFullArt = false,
		IsOversized = false,
		SetId = 1,
		ColorIdentity = "W",
		FrameLayoutId = (int)FrameType.NotImplemented,
		LanguageId = (int)LanguageType.NotImplemented,
		RarityId = (int)RarityType.NotImplemented,
		IsPromo = false,
		IsReprint = false,
		IsTextless = false,
		IsWotcOfficial = true,
		CreatedDate = DateTime.UtcNow
	};

	public static CardFace CreateCardFace(string name = "Card name") => new CardFace
	{
		ScryfallObjectType = "card_face",
		Name = name,
		OracleText = "Oracle text.",
		TypeLine = "Creature - Creature subtype",
		FlavorText = "Flavor text.",
		ManaCost = "{1}{W}",
		ConvertedManaCost = 2,
		Power = "2",
		Toughness = "2",
		Loyalty = null,
		ArtistName = "Artist name",
		ArtistId = Guid.Empty,
		ColorIndicator = ["W"],
		Colors = ["W"],
		Defense = null,
		IllustrationId = Guid.Empty,
		ImageUris = CreateImageUris(),
		Layout = "flip",
		LocalizedName = null,
		LocalizedText = null,
		LocalizedTypeLine = null,
		OracleId = Guid.Empty,
		Watermark = null,
	};

	public static ImageUris CreateImageUris() => new ImageUris
	{
		Small = new Uri("https://example.com/small.jpg"),
		Normal = new Uri("https://example.com/normal.jpg"),
		Large = new Uri("https://example.com/large.jpg"),
		Png = new Uri("https://example.com/png.png"),
		ArtCrop = new Uri("https://example.com/art_crop.jpg"),
		BorderCrop = new Uri("https://example.com/border_crop.jpg"),
	};

	public static CardPrice CreateCardPrice(
		long cardId = 1,
		VendorType vendor = VendorType.NotImplemented,
		CurrencyType currency = CurrencyType.NotImplemented)
	{
		return new CardPrice
		{
			Price = 1m,
			FoilPrice = 1m,
			PurchaseUri = new Uri("https://example.com/purchase"),
			VendorId = (int)vendor,
			CurrencyId = (int)currency,
			CardId = cardId,
		};
	}

	public static CardLegality CreateCardLegality()
	{
		return new CardLegality
		{
			CardId = 1,
			GameFormatId = 1,
			LegalityId = (int)LegalityType.NotImplemented
		};
	}

	public static CardKeyword CreateCardKeyword(long cardId = 0, int keywordId = 0)
	{
		return new CardKeyword
		{
			Id = 1,
			CardId = cardId,
			KeywordId = keywordId,
		};
	}

	public static Keyword CreateKeyword(int id = 1, string name = "Flying")
	{
		return new Keyword
		{
			Id = id,
			Name = name,
			SourceId = (int)SourceType.NotImplemented,
		};
	}
}
