using ScryfallApi.Scryfall.Types;
using System.Text.Json.Serialization;

namespace ScryfallApi.Scryfall;

public class ScryfallCard
{
	//Core card properties
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
	public required string ScryfallObjectType { get; set; }
	[JsonIgnore]
	public ObjectType? _objectType { get; set; }
	[JsonIgnore]
	public ObjectType ObjectType
	{
		get
		{
			if (_objectType is null)
			{
				_objectType = ScryfallParser.ToEnumKey<ObjectType>(ScryfallObjectType);
			}

			return _objectType.Value;
		}
	}

	[JsonPropertyName("layout")]
	public required string ScryfallLayout { get; set; }
	[JsonIgnore]
	private LayoutType? _layout { get; set; }
	[JsonIgnore]
	public LayoutType Layout
	{
		get
		{
			if (_layout is null)
			{
				_layout = ScryfallParser.ToEnumKey<LayoutType>(ScryfallLayout);
			}
			return _layout.Value;
		}
	}

	[JsonPropertyName("oracle_id")]
	public Guid? OracleId { get; set; }

	[JsonPropertyName("prints_search_uri")]
	public required Uri PrintsSearchUri { get; set; }

	[JsonPropertyName("rulings_uri")]
	public required Uri RulingsUri { get; set; }

	[JsonPropertyName("scryfall_uri")]
	public required Uri ScryfallUri { get; set; }

	[JsonPropertyName("uri")]
	public required Uri ScryfallCardUri { get; set; }


	//Gameplay properties
	[JsonPropertyName("all_parts")]
	public RelatedCard[]? AllParts { get; set; }

	[JsonPropertyName("card_faces")]
	public CardFace[]? CardFaces { get; set; }

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
	public required Dictionary<string, string> ScryfallLegalities { get; set; }
	[JsonIgnore]
	private FormatLegalities? _formatLegalities { get; set; }
	[JsonIgnore]
	public FormatLegalities FormatLegalities
	{
		get
		{
			if (_formatLegalities == null)
			{
				FormatLegalities formatLegalities = ScryfallParser.BindDictionaryToModel<FormatLegalities, string>(
					ScryfallLegalities,
					dictionaryValue => ScryfallParser.ToEnumKey<Legality>(dictionaryValue));

				_formatLegalities = formatLegalities;
			}

			return _formatLegalities;
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


	//Print properties
	[JsonPropertyName("artist")]
	public string? ArtistName { get; set; }

	[JsonPropertyName("artist_ids")]
	public Guid[]? ArtistIds { get; set; }

	[JsonPropertyName("attraction_lights")]
	public int[]? UnfinityAttractionLights { get; set; }

	[JsonPropertyName("booster")]
	public bool CanBeFoundInBoosterPacks { get; set; }

	[JsonPropertyName("border_color")]
	public required string ScryfallBorderColor { get; set; }
	[JsonIgnore]
	public BorderColor? _borderColor { get; set; }
	[JsonIgnore]
	public BorderColor BorderColor
	{
		get
		{
			if (_borderColor is null)
			{
				_borderColor = ScryfallParser.ToEnumKey<BorderColor>(ScryfallBorderColor);
			}
			return _borderColor.Value;
		}
	}

	[JsonPropertyName("card_back_id")]
	public Guid CardBackId { get; set; }

	[JsonPropertyName("collector_number")]
	public required string CollectorNumber { get; set; }

	[JsonPropertyName("content_warning")]
	public bool? HasContentWarning { get; set; }

	[JsonPropertyName("digital")]
	public bool IsOnlyDigitalPrint { get; set; }

	[JsonPropertyName("finishes")]
	public required string[] ScryfallComesInFinishes { get; set; }
	[JsonIgnore]
	private IEnumerable<Finish>? _comesInFinishes { get; set; }
	[JsonIgnore]
	public IEnumerable<Finish> ComesInFinishes
	{
		get
		{
			if (_comesInFinishes == null)
			{
				_comesInFinishes = ScryfallParser.ToEnumKey<Finish>(ScryfallComesInFinishes);
			}

			return _comesInFinishes;
		}
	}

	[JsonPropertyName("flavor_name")]
	public string? FlavorName { get; set; }

	[JsonPropertyName("flavor_text")]
	public string? FlavorText { get; set; }

	[JsonPropertyName("frame_effects")]
	public string[]? ScryfallFrameEffects { get; set; }
	[JsonIgnore]
	private FrameEffects? _frameEffects { get; set; }
	[JsonIgnore]
	public FrameEffects? FrameEffects
	{
		get
		{
			if (ScryfallFrameEffects is null) return null;

			if (_frameEffects is null)
			{
				FrameEffects frameEffects = ScryfallParser.BindJsonStringsToModel<FrameEffects>(ScryfallFrameEffects, property => true);

				_frameEffects = frameEffects;
			}

			return _frameEffects;
		}
	}

	[JsonPropertyName("frame")]
	public required string FrameType { get; set; }

	[JsonPropertyName("full_art")]
	public bool IsFullArt { get; set; }

	[JsonPropertyName("games")]
	public required string[] PrintAvailableInGameModes { get; set; }
	[JsonIgnore]
	private PrintAvailability? _printAvailability { get; set; }
	[JsonIgnore]
	public PrintAvailability PrintAvailability
	{
		get
		{
			if (_printAvailability is null)
			{
				PrintAvailability printAvailability = ScryfallParser.BindJsonStringsToModel<PrintAvailability>(PrintAvailableInGameModes, property => true);

				_printAvailability = printAvailability;
			}

			return _printAvailability;
		}
	}

	[JsonPropertyName("highres_image")]
	public bool ImageIsHighResolution { get; set; }

	[JsonPropertyName("illustration_id")]
	public Guid? IllustrationId { get; set; }

	[JsonPropertyName("image_status")]
	public required string ScryfallImageStatus { get; set; }
	[JsonIgnore]
	private ImageStatus? _imageStatus { get; set; }
	[JsonIgnore]
	public ImageStatus ImageStatus
	{
		get
		{
			if (_imageStatus is null)
			{
				_imageStatus = ScryfallParser.ToEnumKey<ImageStatus>(ScryfallImageStatus);
			}

			return _imageStatus.Value;
		}
	}

	[JsonPropertyName("image_uris")]
	public ImageUris? ImageUris { get; set; }

	[JsonPropertyName("oversized")]
	public bool CardIsOversized { get; set; }

	[JsonPropertyName("prices")]
	public required CardPrices Prices { get; set; }

	[JsonPropertyName("printed_name")]
	public string? LocalizedName { get; set; }

	[JsonPropertyName("printed_text")]
	public string? LocalizedText { get; set; }

	[JsonPropertyName("printed_type_line")]
	public string? LocalizedTypeLine { get; set; }

	[JsonPropertyName("promo")]
	public bool IsPromoPrint { get; set; }

	//Is probably static types, but there is no api documentation for this. Consider handling manually after parsing data.
	[JsonPropertyName("promo_types")]
	public string[]? PromoTypes { get; set; }

	[JsonPropertyName("purchase_uris")]
	public VendorUris? VendorUris { get; set; }

	[JsonPropertyName("rarity")]
	public required string ScryfallRarity { get; set; }
	[JsonIgnore]
	private Rarity? _rarity { get; set; }
	[JsonIgnore]
	public Rarity Rarity
	{
		get
		{
			if (_rarity is null)
			{
				_rarity = ScryfallParser.ToEnumKey<Rarity>(ScryfallRarity);
			}

			return _rarity.Value;
		}
	}

	[JsonPropertyName("related_uris")]
	public required RelatedUris RelatedUris { get; set; }

	[JsonPropertyName("released_at")]
	public DateOnly ReleasedDate { get; set; }

	[JsonPropertyName("reprint")]
	public bool IsReprint { get; set; }

	[JsonPropertyName("scryfall_set_uri")]
	public required Uri ScryfallSetUri { get; set; }

	[JsonPropertyName("set_name")]
	public required string FullSetName { get; set; }

	[JsonPropertyName("set_search_uri")]
	public required Uri ScryfallSetSearchUri { get; set; }

	//Is probably static types, but there is no api documentation for this. Consider handling manually after parsing data.
	[JsonPropertyName("set_type")]
	public required string SetType { get; set; }

	[JsonPropertyName("set_uri")]
	public required Uri SetUri { get; set; }

	//Seems to be an abbreviation of the set, there is currently no info on scryfall api documentation (feb2025). This is most likely static, but keeping it as a string should be fine for now.
	[JsonPropertyName("set")]
	public required string SetCode { get; set; }

	[JsonPropertyName("set_id")]
	public Guid SetId { get; set; }

	[JsonPropertyName("story_spotlight")]
	public bool IsStorySpotlight { get; set; }

	[JsonPropertyName("textless")]
	public bool IsTextlessPrint { get; set; }

	[JsonPropertyName("variation")]
	public bool IsPrintVariation { get; set; }

	[JsonPropertyName("variation_of")]
	public Guid? VariationOfId { get; set; }

	[JsonPropertyName("security_stamp")]
	public string? SecurityStamp { get; set; }
	[JsonIgnore]
	private SecurityStampType? _securityStampType { get; set; }
	[JsonIgnore]
	public SecurityStampType SecurityStampType
	{
		get
		{
			if (_securityStampType is null)
			{
				_securityStampType = string.IsNullOrEmpty(SecurityStamp)
					? SecurityStampType.None
					: ScryfallParser.ToEnumKey<SecurityStampType>(SecurityStamp);
			}

			return _securityStampType.Value;
		}
	}

	[JsonPropertyName("watermark")]
	public string? WaterMark { get; set; }

	[JsonPropertyName("preview.previewed_at")]
	public DateOnly? PreviewDate { get; set; }

	[JsonPropertyName("preview.source_uri")]
	public Uri? PreviewSourceUri { get; set; }

	[JsonPropertyName("preview.source")]
	public string? PreviewSourceName { get; set; }
}