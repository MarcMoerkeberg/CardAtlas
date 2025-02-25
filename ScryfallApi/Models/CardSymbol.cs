using ScryfallApi.Scryfall.Types;
using System.Text.Json.Serialization;

namespace ScryfallApi.Models;

public class CardSymbol
{
	[JsonPropertyName("object")]
	public required string ScryfallObjectType { get; set; }
	[JsonIgnore]
	private ObjectType? _objectType { get; set; }
	[JsonIgnore]
	public ObjectType ObjectType => _objectType ??= ScryfallParser.ToEnumKey<ObjectType>(ScryfallObjectType);

	[JsonPropertyName("symbol")]
	public required string Symbol { get; set; }

	[JsonPropertyName("loose_variant")]
	public string? LooseVariant { get; set; }

	[JsonPropertyName("english")]
	public required string Description { get; set; }

	[JsonPropertyName("transposable")]
	public bool IsTransposable { get; set; }

	[JsonPropertyName("represents_mana")]
	public bool RepresentsMana { get; set; }

	[JsonPropertyName("mana_value")]
	public decimal? ConvertedManaCost { get; set; }

	[JsonPropertyName("appears_in_mana_costs")]
	public bool AppearsInManaCosts { get; set; }

	[JsonPropertyName("funny")]
	public bool IsOnlyUsedOnFunnyOrUnCards { get; set; }

	[JsonPropertyName("colors")]
	public required IEnumerable<string> Colors { get; set; }

	[JsonPropertyName("hybrid")]
	public bool IsHybridManaSymbol { get; set; }

	[JsonPropertyName("phyrexian")]
	public bool IsPhyrexianManaSymbol { get; set; }

	[JsonPropertyName("gatherer_alternates")]
	public IEnumerable<string>? GathererAlternates { get; set; }

	[JsonPropertyName("svg_uri")]
	public Uri? SvgUri { get; set; }
}
