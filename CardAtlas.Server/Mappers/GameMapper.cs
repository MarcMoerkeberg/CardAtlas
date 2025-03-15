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
	public static HashSet<GameKind> MapGameTypes(ApiCard apiCard)
	{
		var gameTypes = new HashSet<GameKind>();

		if (apiCard.PrintAvailability.Paper) gameTypes.Add(GameKind.Paper);
		if (apiCard.PrintAvailability.Arena) gameTypes.Add(GameKind.Arena);
		if (apiCard.PrintAvailability.Mtgo) gameTypes.Add(GameKind.Mtgo);

		return gameTypes;
	}


	public static HashSet<CardLegality> MapCardLegalities(long cardId, ApiCard apiCard, HashSet<GameFormat> gameFormats)
	{
		var legalities = new HashSet<CardLegality>();
		if (apiCard.ScryfallLegalities is { Count: 0 }) return legalities;

		foreach (var (key, value) in apiCard.ScryfallLegalities)
		{
			GameFormat? gameFormat = gameFormats.GetWithName(key);
			if (gameFormat is null) continue;

			legalities.Add(new CardLegality
			{
				CardId = cardId,
				GameFormatId = gameFormat.Id,
				LegalityId = (int)GetLegalityType(key),
			});
		}

		return legalities;
	}

	private static LegalityType GetLegalityType(string scryfallLegality)
	{
		return scryfallLegality switch
		{
			"legal" => LegalityType.Legal,
			"not_legal" => LegalityType.NotLegal,
			"restricted" => LegalityType.Restricted,
			"banned" => LegalityType.Banned,
			_ => LegalityType.NotImplemented
		};
	}
}
