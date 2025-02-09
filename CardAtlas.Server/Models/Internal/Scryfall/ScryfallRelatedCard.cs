using CardAtlas.Server.Helpers;
using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Internal.Scryfall;

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