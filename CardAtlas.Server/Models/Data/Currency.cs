using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Currency : TypeEntity<CurrencyType>
{
	[InverseProperty("Currency")]
	public ICollection<Vendor> Vendors { get; set; } = new HashSet<Vendor>();
}

public enum CurrencyType
{
	NotImplemented = -1,
	Usd = 1,
	Eur = 2,
	Tix = 3,
}