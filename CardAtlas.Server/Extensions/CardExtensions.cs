using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Cards;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Extensions;

public static class CardExtensions
{
	/// <summary>
	/// Finds the first <see cref="Card"/> in <paramref name="targetCollection"/> with properties that match the provided <paramref name="cardFace"/>.
	/// </summary>
	/// <returns>The first <see cref="Card"/> with properties that match <paramref name="cardFace"/>. Returns null if none is found.</returns>
	public static Card? FindMatchByName(this IEnumerable<Card> targetCollection, CardFace cardFace)
	{
		return targetCollection.FirstOrDefault(cardFromDb => 
			string.Equals(cardFromDb.Name, cardFace.Name)
		);
	}

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
