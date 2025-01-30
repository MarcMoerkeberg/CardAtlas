using CardAtlas.Server.Helpers;
using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Internal.Scryfall;

public class ScryfallList<T>
{
	[JsonPropertyName("object")]
	public required string ObjectType { get; set; }

	[JsonPropertyName("data")]
	public required T[] Data { get; set; }

	[JsonPropertyName("has_more")]
	public bool HasMore { get; set; }

	[JsonPropertyName("next_page")]
	public Uri? NextPage { get; set; }

	[JsonPropertyName("total_cards")]
	public int? TotalCards { get; set; }

	[JsonPropertyName("warnings")]
	public string[]? Warnings { get; set; }

	public ScryfallObjectType ScryfallObjectType => ObjectType.ParseAsScryfallEnum<ScryfallObjectType>();

}
