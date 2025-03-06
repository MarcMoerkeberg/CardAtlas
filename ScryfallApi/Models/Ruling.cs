using ScryfallApi.Models.Types;
using System.Text.Json.Serialization;

namespace ScryfallApi.Models;

public class Ruling
{
	[JsonPropertyName("object")]
	public required string ScryfallObjectType { get; set; }
	[JsonIgnore]
	private ObjectType? _objectType { get; set; }
	[JsonIgnore]
	public ObjectType ObjectType => _objectType ??= ScryfallParser.ToEnumKey<ObjectType>(ScryfallObjectType);

	[JsonPropertyName("oracle_id")]
	public Guid OracleId { get; set; }

	[JsonPropertyName("source")]
	public required string Source { get; set; }

	[JsonPropertyName("pubished_at")]
	public DateOnly PublishedDate { get; set; }

	[JsonPropertyName("comment")]
	public required string RulingText { get; set; }
}
