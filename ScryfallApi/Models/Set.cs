﻿using ScryfallApi.Models.Types;
using System.Text.Json.Serialization;

namespace ScryfallApi.Models;

public class Set
{
	[JsonPropertyName("object")]
	public required string ScryfallObjectType { get; set; }
	[JsonIgnore]
	private ObjectType? _objectType { get; set; }
	[JsonIgnore]
	public ObjectType ObjectType => _objectType ??= ScryfallParser.ToEnumKey<ObjectType>(ScryfallObjectType);

	[JsonPropertyName("id")]
	public required Guid Id { get; set; }

	[JsonPropertyName("code")]
	public required string SetCode { get; set; }

	[JsonPropertyName("mtgo_code")]
	public string? MtgoSetCode { get; set; }

	[JsonPropertyName("arena_code")]
	public string? ArenaSetCode { get; set; }

	[JsonPropertyName("tcgplayer_id")]
	public int? TcgPlayerId { get; set; }

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("set_type")]
	public required string ScryfallSetType { get; set; }
	[JsonIgnore]
	private SetType? _setType { get; set; }
	[JsonIgnore]
	public SetType SetType => _setType ??= ScryfallParser.ToEnumKey<SetType>(ScryfallSetType);

	[JsonPropertyName("released_at")]
	public DateOnly? ReleasedDate { get; set; }

	[JsonPropertyName("block_code")]
	public string? BlockCode { get; set; }

	[JsonPropertyName("block")]
	public string? Block { get; set; }

	[JsonPropertyName("parent_set_code")]
	public string? ParentSetCode { get; set; }

	[JsonPropertyName("card_count")]
	public int CardCountInSet { get; set; }

	[JsonPropertyName("printed_size")]
	public int? PrintedSize { get; set; }

	[JsonPropertyName("digital")]
	public bool IsDigitalOnly { get; set; }

	[JsonPropertyName("foil_only")]
	public bool IsFoilOnly { get; set; }

	[JsonPropertyName("nonfoil_only")]
	public bool IsNonFoilOnly { get; set; }

	[JsonPropertyName("scryfall_uri")]
	public required Uri ScryfallUri { get; set; }

	[JsonPropertyName("uri")]
	public required Uri SetUri { get; set; }

	[JsonPropertyName("icon_svg_uri")]
	public required Uri IconSvgUri { get; set; }

	[JsonPropertyName("search_uri")]
	public required Uri SearchUri { get; set; }
}
