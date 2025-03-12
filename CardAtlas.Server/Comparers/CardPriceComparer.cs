using CardAtlas.Server.Models.Data.Cards;

namespace CardAtlas.Server.Comparers;

public class CardPriceComparer : IEqualityComparer<CardPrice>
{
	public bool Equals(CardPrice? x, CardPrice? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.Id == y.Id
			&& x.Price == y.Price
			&& x.FoilPrice == y.FoilPrice
			&& x.PurchaseUri == y.PurchaseUri
			&& x.CardId == y.CardId
			&& x.VendorId == y.VendorId
			&& x.CurrencyId == y.CurrencyId;
	}

	public int GetHashCode(CardPrice obj)
	{
		var hash = new HashCode();
		hash.Add(obj.Id);
		hash.Add(obj.FoilPrice);
		hash.Add(obj.PurchaseUri);
		hash.Add(obj.Price);
		hash.Add(obj.CardId);
		hash.Add(obj.VendorId);
		hash.Add(obj.CurrencyId);

		return hash.ToHashCode();
	}
}