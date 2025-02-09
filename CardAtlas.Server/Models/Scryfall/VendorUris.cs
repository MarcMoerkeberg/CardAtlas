using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Scryfall;

public class VendorUris
{
	[JsonPropertyName("tcgplayer")]
	public Uri? TcgPlayer { get; set; }

	[JsonPropertyName("cardmarket")]
	public Uri? CardMarket { get; set; }

	[JsonPropertyName("cardhoarder")]
	public Uri? CardHoarder { get; set; }
}
