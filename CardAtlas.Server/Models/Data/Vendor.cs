using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.Cards;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Vendor : TypeEntity<VendorType>
{
	[InverseProperty("Vendor")]
	public ICollection<CardPrice> CardPrices { get; set; } = new HashSet<CardPrice>();
}

public enum VendorType
{
	NotImplemented = -1,
	TcgPlayer = 1,
	CardMarket = 2,
	CardHoarder = 3,
	Mtgo = 4,
}