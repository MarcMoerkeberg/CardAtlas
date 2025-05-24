using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using ApiCard = ScryfallApi.Models.Card;


namespace CardAtlas.Server.Mappers;

public static class GameMapper
{
	/// <summary>
	/// Maps the print availability from <paramref name="apiCard"/> to a collection of <see cref="PlatformType"/>
	/// </summary>
	public static List<PlatformType> MapGameTypes(ApiCard apiCard)
	{
		var gameTypes = new List<PlatformType>();

		if (apiCard.PrintAvailability.Paper) gameTypes.Add(PlatformType.Paper);
		if (apiCard.PrintAvailability.Arena) gameTypes.Add(PlatformType.Arena);
		if (apiCard.PrintAvailability.Mtgo) gameTypes.Add(PlatformType.Mtgo);

		return gameTypes;
	}

	/// <summary>
	/// Maps the print availability from <paramref name="apiCard"/> to a collection of <see cref="CardGamePlatform"/>
	/// </summary>
	public static List<CardGamePlatform> MapCardGamePlatform(ApiCard apiCard)
	{
		var gameTypeAvailability = new List<CardGamePlatform>();

		if (apiCard.PrintAvailability.Paper)
		{
			gameTypeAvailability.Add(MapCardGamePlatform(PlatformType.Paper));
		}

		if (apiCard.PrintAvailability.Arena)
		{
			gameTypeAvailability.Add(MapCardGamePlatform(PlatformType.Arena));

		}

		if (apiCard.PrintAvailability.Mtgo)
		{
			gameTypeAvailability.Add(MapCardGamePlatform(PlatformType.Mtgo));
		}

		return gameTypeAvailability;
	}

	private static CardGamePlatform MapCardGamePlatform(PlatformType gamePlatform)
	{
		return new CardGamePlatform
		{
			CardId = default,
			GamePlatformId = (int)gamePlatform
		};
	}

	/// <summary>
	/// Creates a new list populated with <see cref="GameFormat"/> entities from the available data on the <paramref name="apiCard"/>.
	/// </summary>
	public static IEnumerable<GameFormat> MapGameFormat(ApiCard apiCard)
	{
		if (apiCard.ScryfallLegalities is not { Count: > 0 }) return Enumerable.Empty<GameFormat>();

		return apiCard.ScryfallLegalities.Keys
			.Select(format => new GameFormat
			{
				Name = format.CapitalizeFirstLetter(),
				SourceId = (int)SourceType.Scryfall,
			})
			.ToList();
	}
}
