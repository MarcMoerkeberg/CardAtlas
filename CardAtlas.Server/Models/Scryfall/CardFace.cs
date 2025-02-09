using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Scryfall.Types;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Scryfall;

public class CardFace
{
	[JsonPropertyName("artist")]
	public string? Artist { get; set; }

	[JsonPropertyName("artist_id")]
	public Guid? ArtistId { get; set; }

	[JsonPropertyName("cmc")]
	public decimal? ConvertedManaCost { get; set; }

	[JsonPropertyName("color_indicator")]
	public string[]? ColorIndicator { get; set; }

	[JsonPropertyName("colors")]
	public string[]? Colors { get; set; }

	[JsonPropertyName("defense")]
	public string? Defense { get; set; }

	[JsonPropertyName("flavor_text")]
	public string? FlavorText { get; set; }

	[JsonPropertyName("illustration_id")]
	public Guid? IllustrationId { get; set; }

	[JsonPropertyName("image_uris")]
	public Dictionary<string, Uri?>? ScryfallImageUris { get; set; }
	private ImageUris? _imageUris { get; set; }
	public ImageUris? ImageUris
	{
		get
		{
			if (ScryfallImageUris == null || ScryfallImageUris.Count == 0) return null;

			if (_imageUris is null)
			{
				var imageUris = new ImageUris();

				foreach (KeyValuePair<string, Uri?> scryfallImageUri in ScryfallImageUris)
				{
					PropertyInfo? property = typeof(ImageUris)
						.GetProperty(scryfallImageUri.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

					if (property != null && property.CanWrite)
					{
						property.SetValue(imageUris, scryfallImageUri.Value);
					}
				}

				_imageUris = imageUris;
			}

			return _imageUris;
		}
	}

	[JsonPropertyName("layout")]
	public string? Layout { get; set; }

	[JsonPropertyName("loyalty")]
	public string? Loyalty { get; set; }

	[JsonPropertyName("mana_cost")]
	public required string ManaCost { get; set; }

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("object")]
	public required string ObjectType { get; set; }
	public ObjectType ScryfallObjectType => ObjectType.ParseAsScryfallEnum<ObjectType>();

	[JsonPropertyName("oracle_id")]
	public Guid? OracleId { get; set; }

	[JsonPropertyName("oracle_text")]
	public string? OracleText { get; set; }

	[JsonPropertyName("power")]
	public string? Power { get; set; }

	[JsonPropertyName("printed_name")]
	public string? LocalizedName { get; set; }

	[JsonPropertyName("printed_text")]
	public string? LocalizedText { get; set; }

	[JsonPropertyName("printed_type_line")]
	public string? LocalizedTypeLine { get; set; }

	[JsonPropertyName("toughness")]
	public string? Toughness { get; set; }

	[JsonPropertyName("type_line")]
	public string? TypeLine { get; set; }

	[JsonPropertyName("watermark")]
	public string? Watermark { get; set; }
}
