using System.Text.Json.Serialization;

namespace ScryfallApi.Scryfall;

public class CardPrices
{
	[JsonPropertyName("usd")]
	public string? ScryfallUsd { get; set; }
	[JsonIgnore]
	private decimal? _usd;
	[JsonIgnore]
	public decimal? Usd => _usd ??= ParsePrice(ScryfallUsd);

	[JsonPropertyName("usd_foil")]
	public string? ScryfallUsdFoil { get; set; }
	[JsonIgnore]
	private decimal? _usdFoil;
	[JsonIgnore]
	public decimal? UsdFoil => _usdFoil ??= ParsePrice(ScryfallUsdFoil);

	[JsonPropertyName("usd_etched")]
	public string? ScryfallUsdEtched { get; set; }
	[JsonIgnore]
	private decimal? _usdEtched;
	[JsonIgnore]
	public decimal? UsdEtched => _usdEtched ??= ParsePrice(ScryfallUsdEtched);

	[JsonPropertyName("eur")]
	public string? ScryfallEur { get; set; }
	[JsonIgnore]
	private decimal? _eur;
	[JsonIgnore]
	public decimal? Eur => _eur ??= ParsePrice(ScryfallEur);

	[JsonPropertyName("eur_foil")]
	public string? ScryfallEurFoil { get; set; }
	[JsonIgnore]
	private decimal? _eurFoil;
	[JsonIgnore]
	public decimal? EurFoil => _eurFoil ??= ParsePrice(ScryfallEurFoil);

	[JsonPropertyName("eur_etched")]
	public string? ScryfallEurEtched { get; set; }
	[JsonIgnore]
	private decimal? _eurEtched;
	[JsonIgnore]
	public decimal? EurEtched => _eurEtched ??= ParsePrice(ScryfallEurEtched);

	[JsonPropertyName("tix")]
	public string? ScryfallMtgoTix { get; set; }
	[JsonIgnore]
	private decimal? _mtgoTix; 
	[JsonIgnore]
	public decimal? MtgoTix => _mtgoTix ??= ParsePrice(ScryfallMtgoTix);

	private decimal? ParsePrice(string? value) => decimal.TryParse(value, out var parsedDecimal) 
		? parsedDecimal 
		: null;
}
