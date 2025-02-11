using ScryfallApi.Scryfall.Types;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;

namespace ScryfallApi.Scryfall;

public class BulkData
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("uri")]
	public required Uri ScryfallApiUri { get; set; }

	[JsonPropertyName("type")]
	public required string ScryfallBulkDataType { get; set; }
	[JsonIgnore]
	private BulkDataType? _bulkDataType { get; set; }
	[JsonIgnore]
	public BulkDataType BulkDataType
	{
		get
		{
			if (_bulkDataType == null)
			{
				_bulkDataType = ScryfallParser.ToEnumKey<BulkDataType>(ScryfallBulkDataType);
			}

			return _bulkDataType.Value;
		}
	}

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("description")]
	public required string Description { get; set; }

	[JsonPropertyName("download_uri")]
	public required Uri DownloadUri { get; set; }

	[JsonPropertyName("updated_at")]
	public DateTime UpdatedDate { get; set; }

	[JsonPropertyName("size")]
	public long FileSizeInBytes { get; set; }

	[JsonPropertyName("content_type")]
	public required string ScryfallContentType { get; set; }
	[JsonIgnore]
	public ContentType? _contentType { get; set; }
	[JsonIgnore]
	public ContentType ContentType
	{
		get
		{
			if (_contentType == null)
			{
				_contentType = new ContentType(ScryfallContentType);
			}

			return _contentType;
		}
	}

	[JsonPropertyName("content_encoding")]
	public required string ScryfallContentEncoding { get; set; }
	[JsonIgnore]
	public Encoding ContentEncoding => Encoding.GetEncoding(ScryfallContentEncoding);
}
