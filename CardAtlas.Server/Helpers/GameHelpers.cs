using CardAtlas.Server.Extensions;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using ApiCard = ScryfallApi.Models.Card;

namespace CardAtlas.Server.Helpers;

public static class GameHelpers
{
	/// <summary>
	/// Gets all the missing game types from comparing <paramref name="apiCard"/> to the <paramref name="card"/>.
	/// </summary>
	/// <param name="card">Should have the associated <see cref="Card.GameTypes"/> materialized when calling this method.</param>
	/// <returns>A new <see cref="CardGameTypeAvailability"/> for each gametype missing from the <paramref name="card"/> when compared to the <paramref name="apiCard"/>.</returns>
	public static IEnumerable<CardGameTypeAvailability> GetMissingGameTypes(Card card, ApiCard apiCard)
	{
		HashSet<GameKind> apiGameTypes = GameMapper.MapGameTypes(apiCard);
		IEnumerable<CardGameTypeAvailability> missingGameTypes = apiGameTypes
			.Where(apiGameKind => !card.PrintedInGameTypes.Contains(apiGameKind))
			.Select(apiGameKind =>
				new CardGameTypeAvailability
				{
					CardId = card.Id,
					GameTypeId = (int)apiGameKind
				}
			);

		return missingGameTypes;
	}

	/// <summary>
	/// Gets all missing game formats from comparing the legalities of <paramref name="apiCard"/> to the provided <paramref name="formats"/>
	/// </summary>
	/// <returns>A new <see cref="GameFormat"/> for each missing format when comparing those on the <paramref name="apiCard"/> to the provided <paramref name="formats"/>.</returns>
	public static HashSet<GameFormat> GetMissingGameFormats(ApiCard apiCard, HashSet<GameFormat> formats)
	{
		var missingFormats = new HashSet<GameFormat>();

		foreach (string scryfallFormatName in apiCard.ScryfallLegalities.Keys)
		{
			if (formats.ExistsWithName(scryfallFormatName, SourceType.Scryfall)) continue;

			missingFormats.Add(new GameFormat
			{
				Name = scryfallFormatName.CapitalizeFirstLetter(),
				SourceId = (int)SourceType.Scryfall
			});
		}

		return missingFormats;
	}
}
