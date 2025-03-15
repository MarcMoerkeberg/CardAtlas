using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using ApiCard = ScryfallApi.Models.Card;


namespace CardAtlas.Server.Mappers;

public static class GameMapper
{
	/// <summary>
	/// Maps the print availability from <paramref name="apiCard"/> to a collection of <see cref="GameKind"/>
	/// </summary>
	public static HashSet<GameKind> MapGameTypes(ApiCard apiCard)
	{
		var gameTypes = new HashSet<GameKind>();

		if (apiCard.PrintAvailability.Paper) gameTypes.Add(GameKind.Paper);
		if (apiCard.PrintAvailability.Arena) gameTypes.Add(GameKind.Arena);
		if (apiCard.PrintAvailability.Mtgo) gameTypes.Add(GameKind.Mtgo);

		return gameTypes;
	}
}
