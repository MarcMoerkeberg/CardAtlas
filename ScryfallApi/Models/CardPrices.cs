using System.Text.Json.Serialization;

namespace ScryfallApi.Scryfall;

public class CardPrices
{
	[JsonPropertyName("usd")]
	public decimal? Usd { get; set; }

	[JsonPropertyName("usd_foil")]
	public decimal? UsdFoil { get; set; }

	[JsonPropertyName("usd_etched")]
	public decimal? UsdEtched { get; set; }

	[JsonPropertyName("eur")]
	public decimal? Eur { get; set; }

	[JsonPropertyName("eur_foil")]
	public decimal? EurFoil { get; set; }

	[JsonPropertyName("eur_etched")]
	public decimal? EurEtched { get; set; }

	[JsonPropertyName("tix")]
	public decimal? MtgoTix { get; set; }
}
