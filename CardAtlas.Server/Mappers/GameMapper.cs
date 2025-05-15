using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using ApiCard = ScryfallApi.Models.Card;


namespace CardAtlas.Server.Mappers;

public static class GameMapper
{
	/// <summary>
	/// Maps the print availability from <paramref name="apiCard"/> to a collection of <see cref="GameKind"/>
	/// </summary>
	public static List<GameKind> MapGameTypes(ApiCard apiCard)
	{
		var gameTypes = new List<GameKind>();

		if (apiCard.PrintAvailability.Paper) gameTypes.Add(GameKind.Paper);
		if (apiCard.PrintAvailability.Arena) gameTypes.Add(GameKind.Arena);
		if (apiCard.PrintAvailability.Mtgo) gameTypes.Add(GameKind.Mtgo);

		return gameTypes;
	}

	/// <summary>
	/// Maps the print availability from <paramref name="apiCard"/> to a collection of <see cref="CardGameTypeAvailability"/>
	/// </summary>
	public static List<CardGameTypeAvailability> MapGameTypeAvailability(ApiCard apiCard)
	{
		var gameTypeAvailability = new List<CardGameTypeAvailability>();

		if (apiCard.PrintAvailability.Paper)
		{
			gameTypeAvailability.Add(MapCardGameTypeAvailability(GameKind.Paper));
		}

		if (apiCard.PrintAvailability.Arena)
		{
			gameTypeAvailability.Add(MapCardGameTypeAvailability(GameKind.Arena));

		}

		if (apiCard.PrintAvailability.Mtgo)
		{
			gameTypeAvailability.Add(MapCardGameTypeAvailability(GameKind.Mtgo));
		}

		return gameTypeAvailability;
	}

	private static CardGameTypeAvailability MapCardGameTypeAvailability(GameKind gameKind)
	{
		return new CardGameTypeAvailability
		{
			CardId = default,
			GameTypeId = (int)gameKind
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
