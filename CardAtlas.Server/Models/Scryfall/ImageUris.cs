using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Scryfall;

public class ImageUris
{
	[JsonPropertyName("png")]
	public Uri? Png { get; set; }

	[JsonPropertyName("border_crop")]
	public Uri? BorderCrop { get; set; }

	[JsonPropertyName("art_crop")]
	public Uri? ArtCrop { get; set; }

	[JsonPropertyName("large")]
	public Uri? Large { get; set; }

	[JsonPropertyName("normal")]
	public Uri? Normal { get; set; }

	[JsonPropertyName("small")]
	public Uri? Small { get; set; }
}
