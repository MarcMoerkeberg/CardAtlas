using ScryfallApi.Scryfall.Types;
using System.Text.Json.Serialization;

namespace ScryfallApi.Scryfall;

public class RelatedCard
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

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

	[JsonPropertyName("component")]
	public required string ScryfallComponentType { get; set; }
	[JsonIgnore]
	public ComponentType? _componentType { get; set; }
	[JsonIgnore]
	public ComponentType ComponentType
	{
		get
		{
			if (_componentType == null)
			{
				_componentType = ScryfallParser.ToEnumKey<ComponentType>(ScryfallComponentType);
			}

			return _componentType.Value;
		}
	}

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("type_line")]
	public required string TypeLine { get; set; }

	[JsonPropertyName("uri")]
	public required Uri Uri { get; set; }

}