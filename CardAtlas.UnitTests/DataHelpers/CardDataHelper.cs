using CardAtlas.Server.Models.Data;
using CardFace = ScryfallApi.Models.CardFace;
using ImageUris = ScryfallApi.Models.ImageUris;

namespace CardAtlas.UnitTests.DataHelpers;

public static class CardDataHelper
{
	public static Card CreateCard(string name = "Card name") => new Card
	{
		Name = name,
		OracleText = "Oracle text.",
		TypeLine = "Creature - Creature subtype",
		FlavorText = "Flavor text.",
		ManaCost = "{1}{W}",
		ConvertedManaCost = 2,
		Power = "2",
		Toughness = "2",
		Loyalty = null,
		ScryfallId = Guid.Empty,
		CollectorNumber = "1",
		ReleaseDate = new DateOnly(2025, 1, 1),
		IsOnReservedList = false,
		CanBeFoundInBoosters = true,
		IsDigitalOnly = false,
		IsFullArt = false,
		IsOversized = false,
		ArtistId = Artist.DefaultArtistId,
		SetId = 1,
		ColorIdentity = "W",
		FrameLayoutId = (int)FrameType.NotImplemented,
		LanguageId = (int)LanguageType.NotImplemented,
		RarityId = (int)RarityType.NotImplemented,
		IsPromo = false,
		IsReprint = false,
		IsTextless = false,
		IsWotcOfficial = true,
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
		Artist = "Artist name",
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
}
