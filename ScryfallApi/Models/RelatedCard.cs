using ScryfallApi.Models.Types;
using System.Text.Json.Serialization;

namespace ScryfallApi.Models;

public class RelatedCard
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("object")]
	public required string ScryfallObjectType { get; set; }
	[JsonIgnore]
	private ObjectType? _objectType { get; set; }
	[JsonIgnore]
	public ObjectType ObjectType => _objectType ??= ScryfallParser.ToEnumKey<ObjectType>(ScryfallObjectType);

	[JsonPropertyName("component")]
	public required string ScryfallComponentType { get; set; }
	[JsonIgnore]
	public ComponentType? _componentType { get; set; }
	[JsonIgnore]
	public ComponentType ComponentType => _componentType ??= ScryfallParser.ToEnumKey<ComponentType>(ScryfallComponentType);

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("type_line")]
	public required string TypeLine { get; set; }

	[JsonPropertyName("uri")]
	public required Uri Uri { get; set; }

}