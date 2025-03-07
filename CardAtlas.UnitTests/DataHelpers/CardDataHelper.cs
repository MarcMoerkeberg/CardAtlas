using CardAtlas.Server.Models.Data;

namespace CardAtlas.UnitTests.DataHelpers;

public static class CardDataHelper
{
	public static Card CreateCard()
	{
		return new Card
		{
			Name = "Card name",
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
	}
}
