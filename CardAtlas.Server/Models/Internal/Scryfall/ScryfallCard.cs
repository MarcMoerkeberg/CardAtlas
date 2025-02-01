using CardAtlas.Server.Helpers;
using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Internal.Scryfall;

public class ScryfallCard
{
}

public class ScryfallCoreCard
{
	[JsonPropertyName("arena_id")]
	public int? ArenaId { get; set; }

	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("lang")]
	public required string Language { get; set; }

	[JsonPropertyName("mtgo_id")]
	public int? MtgoId { get; set; }

	[JsonPropertyName("mtgo_foil_id")]
	public int? MtgoFoilId { get; set; }

	[JsonPropertyName("multiverse_ids")]
	public int[]? GathererMultiverseIds { get; set; }

	[JsonPropertyName("tcgplayer_id")]
	public int? TcgPlayerId { get; set; }

	[JsonPropertyName("tcgplayer_etched_id")]
	public int? TcgPlayerEtchedId { get; set; }

	[JsonPropertyName("cardmarket_id")]
	public int? CardMarketId { get; set; }

	[JsonPropertyName("object")]
	public required string ObjectType { get; set; }

	[JsonPropertyName("layout")]
	public required string Layout { get; set; }
	public ScryfallLayoutType LayoutType => Layout.ParseAsScryfallEnum<ScryfallLayoutType>();

	[JsonPropertyName("oracle_id")]
	public Guid? OracleId { get; set; }

	[JsonPropertyName("prints_search_uri")]
	public required Uri PrintsSearchUri { get; set; }

	[JsonPropertyName("rulings_uri")]
	public required Uri RulingsUri { get; set; }

	[JsonPropertyName("scryfall_uri")]
	public required Uri ScryfallUri { get; set; }

	[JsonPropertyName("uri")]
	public required Uri Uri { get; set; }
}

public class ScryfallGameplay
{
	[JsonPropertyName("all_parts")]
	public ScryfallRelatedCard[]? AllParts { get; set; }

	[JsonPropertyName("card_faces")]
	public ScryfallCardFace[]? CardFaces { get; set; }

	[JsonPropertyName("color_identity")]
	public required string[] ColorIdentity { get; set; }

	[JsonPropertyName("color_indicator")]
	public string[]? ColorIndicator { get; set; }

	[JsonPropertyName("colors")]
	public string[]? Colors { get; set; }

	[JsonPropertyName("defense")]
	public string? Defense { get; set; }

	[JsonPropertyName("edhrec_rank")]
	public int? EdhRecRank { get; set; }

	[JsonPropertyName("hand_modifier")]
	public string? HandModifier { get; set; }

	[JsonPropertyName("keywords")]
	public required string[] Keywords { get; set; }

	[JsonPropertyName("legalities")]
	public required Dictionary<string, string> Legalities { get; set; }
	public Dictionary<ScryfallFormat, ScryfallLegalFormat> FormatLegalities
	{
		get
		{
			var formatsAndLegalities = new Dictionary<ScryfallFormat, ScryfallLegalFormat>();

			foreach (var format in Legalities)
			{
				formatsAndLegalities.Add(format.Key.ParseAsScryfallEnum<ScryfallFormat>(), format.Value.ParseAsScryfallEnum<ScryfallLegalFormat>());
			}

			return formatsAndLegalities;
		}
	}

	[JsonPropertyName("life_modifier")]
	public string? VanguardLifeModifier { get; set; }

	[JsonPropertyName("loyalty")]
	public string? Loyalty { get; set; }

	[JsonPropertyName("mana_cost")]
	public string? ManaCost { get; set; }

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("oracle_text")]
	public string? OracleText { get; set; }

	[JsonPropertyName("penny_rank")]
	public int? PennyDreadfulRank { get; set; }

	[JsonPropertyName("power")]
	public string? Power { get; set; }

	[JsonPropertyName("produced_mana")]
	public string[]? ProducesManaColor { get; set; }

	[JsonPropertyName("reserved")]
	public bool IsOnReservedList { get; set; }

	[JsonPropertyName("toughness")]
	public string? Toughness { get; set; }

	[JsonPropertyName("type_line")]
	public required string TypeLine { get; set; }
}

public class ScryfallRelatedCard
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("object")]
	public required string ObjectType { get; set; }
	public ScryfallObjectType ScryfallObjectType => ObjectType.ParseAsScryfallEnum<ScryfallObjectType>();

	[JsonPropertyName("component")]
	public required string ComponentType { get; set; }
	public ScryfallComponentType ScryfallComponentType => ComponentType.ParseAsScryfallEnum<ScryfallComponentType>();

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("type_line")]
	public required string TypeLine { get; set; }

	[JsonPropertyName("uri")]
	public required Uri Uri { get; set; }

}

public class ScryfallCardFace
{
	[JsonPropertyName("artist")]
	public string? Artist { get; set; }

	[JsonPropertyName("artist_id")]
	public Guid? ArtistId { get; set; }

	[JsonPropertyName("cmc")]
	public decimal? ConvertedManaCost { get; set; }

	[JsonPropertyName("color_indicator")]
	public string[]? ColorIndicator { get; set; }

	[JsonPropertyName("colors")]
	public string[]? Colors { get; set; }

	[JsonPropertyName("defense")]
	public string? Defense { get; set; }

	[JsonPropertyName("flavor_text")]
	public string? FlavorText { get; set; }

	[JsonPropertyName("illustration_id")]
	public Guid? IllustrationId { get; set; }

	[JsonPropertyName("image_uris")]
	public Dictionary<string, Uri>? ImageUris { get; set; }
	public Dictionary<ScryfallImageFormat, Uri>? ScryfallImageUris
	{
		get
		{
			if (ImageUris == null) return null;

			var imageFormatsAndUris = new Dictionary<ScryfallImageFormat, Uri>();

			foreach (var format in ImageUris)
			{
				imageFormatsAndUris.Add(format.Key.ParseAsScryfallEnum<ScryfallImageFormat>(), format.Value);
			}

			return imageFormatsAndUris;
		}
	}

	[JsonPropertyName("layout")]
	public string? Layout { get; set; }

	[JsonPropertyName("loyalty")]
	public string? Loyalty { get; set; }

	[JsonPropertyName("mana_cost")]
	public required string ManaCost { get; set; }

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("object")]
	public required string ObjectType { get; set; }
	public ScryfallObjectType ScryfallObjectType => ObjectType.ParseAsScryfallEnum<ScryfallObjectType>();

	[JsonPropertyName("oracle_id")]
	public Guid? OracleId { get; set; }

	[JsonPropertyName("oracle_text")]
	public string? OracleText { get; set; }

	[JsonPropertyName("power")]
	public string? Power { get; set; }

	[JsonPropertyName("printed_name")]
	public string? LocalizedName { get; set; }

	[JsonPropertyName("printed_text")]
	public string? LocalizedText { get; set; }

	[JsonPropertyName("printed_type_line")]
	public string? LocalizedTypeLine { get; set; }

	[JsonPropertyName("toughness")]
	public string? Toughness { get; set; }

	[JsonPropertyName("type_line")]
	public string? TypeLine { get; set; }

	[JsonPropertyName("watermark")]
	public string? Watermark { get; set; }
}

public class ScryfallPrintInformation
{
	[JsonPropertyName("artist")]
	public string? ArtistName { get; set; }

	[JsonPropertyName("artist_ids")]
	public Guid[]? ArtistIds { get; set; }

	[JsonPropertyName("attraction_lights")]
	public int[]? UnfinityAttractionLights { get; set; }

	[JsonPropertyName("booster")]
	public bool CanBeFoundInBoosterPacks { get; set; }

	[JsonPropertyName("border_color")]
	public required string BorderColor { get; set; }
	public ScryfallBorderColor ScryfallBorderColor => BorderColor.ParseAsScryfallEnum<ScryfallBorderColor>();

	[JsonPropertyName("card_back_id")]
	public Guid CardBackId { get; set; }

	[JsonPropertyName("collector_number")]
	public required string CollectorNumber { get; set; }

	[JsonPropertyName("content_warning")]
	public bool? HasContentWarning { get; set; }

	[JsonPropertyName("digital")]
	public bool IsOnlyDigitalPrint { get; set; }

	[JsonPropertyName("finishes")]
	public required string[] ComesInFinishes { get; set; }
	public ScryfallFinish[] ComesInScryfallFinishes => ComesInFinishes.ParseAsScryfallEnum<ScryfallFinish>().ToArray();

	[JsonPropertyName("flavor_name")]
	public string? FlavorName { get; set; }

	[JsonPropertyName("flavor_text")]
	public string? FlavorText { get; set; }

	[JsonPropertyName("frame_effects")]
	public string[]? FrameEffects { get; set; }
	public ScryfallFrameEffect[]? ScryfallFrameEffects
	{
		get
		{
			return FrameEffects == null
				? null
				: FrameEffects.ParseAsScryfallEnum<ScryfallFrameEffect>().ToArray();
		}
	}

	[JsonPropertyName("frame")]
	public required string FrameType { get; set; }

	[JsonPropertyName("full_art")]
	public bool IsFullArt { get; set; }

	[JsonPropertyName("games")]
	public required string[] PrintIsAvailableInGameModes { get; set; }
	public ScryfallGameMode[] PrintIsAvailableInScryfallGameModes => PrintIsAvailableInGameModes.ParseAsScryfallEnum<ScryfallGameMode>().ToArray();

	[JsonPropertyName("highres_image")]
	public bool ImageIsHighResolution { get; set; }

	[JsonPropertyName("illustration_id")]
	public Guid? IllustrationId { get; set; }

	[JsonPropertyName("image_status")]
	public required string ImageStatus { get; set; }
	public ScryfallImageStatus ScryfallImageStatus => ImageStatus.ParseAsScryfallEnum<ScryfallImageStatus>();

	[JsonPropertyName("image_uris")]
	public required Dictionary<string, Uri>? ImageUris { get; set; }
	public Dictionary<ScryfallImageFormat, Uri>? ScryfallImageUris
	{
		get
		{
			if (ImageUris == null) return null;

			var imageFormatsAndUris = new Dictionary<ScryfallImageFormat, Uri>();

			foreach (var format in ImageUris)
			{
				imageFormatsAndUris.Add(format.Key.ParseAsScryfallEnum<ScryfallImageFormat>(), format.Value);
			}

			return imageFormatsAndUris;
		}
	}

	[JsonPropertyName("oversized")]
	public bool CardIsOversized { get; set; }

	[JsonPropertyName("prices")]
	public required Dictionary<string, string> CardPrices { get; set; }
	public Dictionary<ScryfallCardPriceType, string> ScryfallCardPrices
	{
		get
		{

		}
	}
}