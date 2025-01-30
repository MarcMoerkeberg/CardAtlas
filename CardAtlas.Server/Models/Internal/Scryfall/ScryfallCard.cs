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

	public ScryfallLayoutType LayoutType => Layout.ParseAsScryfallEnum<ScryfallLayoutType>();
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

	//CONTINUE ...
}

public class ScryfallRelatedCard
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }
	
	[JsonPropertyName("object")]
	public required string ObjectType { get; set; }

	[JsonPropertyName("component")]
	public required string ComponentType { get; set; }

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("type_line")]
	public required string TypeLine { get; set; }
	
	[JsonPropertyName("uri")]
	public required Uri Uri { get; set; }

	public ScryfallObjectType ScryfallObjectType => ObjectType.ParseAsScryfallEnum<ScryfallObjectType>();
	public ScryfallComponentType ScryfallComponentType => ComponentType.ParseAsScryfallEnum<ScryfallComponentType>();
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

	public ScryfallObjectType ScryfallObjectType => ObjectType.ParseAsScryfallEnum<ScryfallObjectType>();
}