using CardAtlas.Server.Models.Data;

namespace CardAtlas.UnitTests.DataHelpers;

public static class SetDataHelper
{
	public static Set CreateSet()
	{
		return new Set
		{
			Id = 1,
			ScryfallId = Guid.Empty,
			Name = "Set name",
			Code = "The set code",
			MtgoCode = string.Empty,
			ArenaCode = string.Empty,
			ParentSetCode = string.Empty,
			Block = "Full block name.",
			BlockCode = "Some block code",
			SetTypeId = (int)SetTypeKind.NotImplemented,
			NumberOfCardsInSet = 0,
			IsDigitalOnly = false,
			IsFoilOnly = false,
			IsNonFoilOnly = false,
			ReleaseDate = new DateOnly(2025, 1, 1),
			SourceId = (int)SourceType.Scryfall,
		};
	}
}
