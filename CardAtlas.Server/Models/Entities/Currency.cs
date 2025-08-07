using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.CardRelations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Currency : TypeEntity<CurrencyType>
{
	[InverseProperty("Currency")]
	public ICollection<CardPrice> CardPrices { get; set; } = new HashSet<CardPrice>();
}

public enum CurrencyType
{
	NotImplemented = -1,
	Usd = 1,
	Eur = 2,
	Tix = 3,
}