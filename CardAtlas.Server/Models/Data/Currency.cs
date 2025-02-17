using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Currency : TypeEntity<CurrencyType>
{
	[InverseProperty("Currency")]
	public required ICollection<Vendor> Vendors { get; set; }
}

public enum CurrencyType
{
	Usd = 1,
	Eur = 2,
	Tix = 3,
	NotImplemented = 0
}