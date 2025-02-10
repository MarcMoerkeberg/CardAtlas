using ScryfallApi.Scryfall.Types;
using System.Text.Json.Serialization;

namespace ScryfallApi.Scryfall;

public class ScryfallList<TData>
{
	[JsonPropertyName("object")]
	public required string ScryfallObjectType { get; set; }
	[JsonIgnore]
	public ObjectType ObjectType => ScryfallObjectType.ParseAsScryfallEnum<ObjectType>();

	[JsonPropertyName("data")]
	public required IEnumerable<TData> Data { get; set; }

	[JsonPropertyName("has_more")]
	public bool HasMore { get; set; }

	[JsonPropertyName("next_page")]
	public Uri? NextPage { get; set; }

	[JsonPropertyName("total_cards")]
	public int? TotalCards { get; set; }

	[JsonPropertyName("warnings")]
	public string[]? Warnings { get; set; }
}
