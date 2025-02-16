using CardAtlas.Server.Models.Data.Base;

namespace CardAtlas.Server.Models.Data;

public class Vendor : TypeEntity<VendorType>
{
	public CurrencyType Currency => Type switch
	{
		VendorType.TcgPlayer => CurrencyType.Usd,
		VendorType.CardMarket => CurrencyType.Eur,
		VendorType.CardHoarder => CurrencyType.Tix,
		_ => CurrencyType.NotImplemented
	};
}

public enum VendorType
{
	TcgPlayer,
	CardMarket,
	CardHoarder,
	NotImplemented = 0
}

public enum CurrencyType
{
	Usd,
	Eur,
	Tix,
	NotImplemented = 0
}