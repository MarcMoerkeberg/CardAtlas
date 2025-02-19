using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Vendor : TypeEntity<VendorType>
{
	[ForeignKey("CurrencyId")]
	public Currency Currency { get; set; } = null!;
	public required int CurrencyId { get; set; }
}

public enum VendorType
{
	TcgPlayer = 1,
	CardMarket = 2,
	CardHoarder = 3,
	NotImplemented = 0
}