using ScryfallApi.Models.Types;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;

namespace ScryfallApi.Models;

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
	public BulkDataType BulkDataType => _bulkDataType ??= ScryfallParser.ToEnumKey<BulkDataType>(ScryfallBulkDataType);

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
	private ContentType? _contentType { get; set; }
	[JsonIgnore]
	public ContentType ContentType => _contentType ??= new ContentType(ScryfallContentType);

	[JsonPropertyName("content_encoding")]
	public required string ScryfallContentEncoding { get; set; }
	[JsonIgnore]
	private Encoding? _contentEncoding { get; set; }
	[JsonIgnore]
	public Encoding ContentEncoding => _contentEncoding ??= Encoding.GetEncoding(ScryfallContentEncoding);
}
