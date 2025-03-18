using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Extensions;

public static class CardExtensions
{
	/// <summary>
	/// Finds the first <see cref="CardPrice"/> in <paramref name="targetCollection"/> with properties that match the provided <paramref name="cardPrice"/>.
	/// </summary>
	/// <returns>the first <see cref="CardPrice"/> with properties that match <paramref name="cardPrice"/>. Returns null if none is found.</returns>
	public static CardPrice? FindMatchByVendorAndCurrency(this IEnumerable<CardPrice> targetCollection, CardPrice cardPrice)
	{
		return targetCollection.FirstOrDefault(collectionPrice =>
			collectionPrice.VendorId == cardPrice.VendorId &&
			collectionPrice.CurrencyId == cardPrice.CurrencyId &&
			collectionPrice.CardId == cardPrice.CardId
		);
	}
}
