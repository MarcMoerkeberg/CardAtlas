using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Scryfall.Types;
using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Scryfall;

public class RelatedCard
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("object")]
	public required string ObjectType { get; set; }
	public ObjectType ScryfallObjectType => ObjectType.ParseAsScryfallEnum<ObjectType>();

	[JsonPropertyName("component")]
	public required string ComponentType { get; set; }
	public ComponentType ScryfallComponentType => ComponentType.ParseAsScryfallEnum<ComponentType>();

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("type_line")]
	public required string TypeLine { get; set; }

	[JsonPropertyName("uri")]
	public required Uri Uri { get; set; }

}