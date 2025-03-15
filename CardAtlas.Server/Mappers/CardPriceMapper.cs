using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using ApiCard = ScryfallApi.Models.Card;

namespace CardAtlas.Server.Mappers;

public static class CardPriceMapper
{
	/// <summary>
	/// Returns a new list of <see cref="CardPrice"/> objects populted with data from <paramref name="apiCard"/><br/>
	/// The list may be empty or missing some <see cref="VendorType"/> if <paramref name="apiCard"/> does not have sufficient data (eg. no prices).
	/// </summary>
	public static IEnumerable<CardPrice> MapCardPrices(long cardId, ApiCard apiCard) 
	{
		var cardPrices = new List<CardPrice>();

		if (apiCard.Prices is null) return cardPrices;

		var cardPriceMappings = new (VendorType Vendor, decimal? Price, decimal? FoilPrice, Uri? PurchaseUri)[]
		{
			(VendorType.TcgPlayer, apiCard.Prices.Usd, apiCard.Prices.UsdFoil, apiCard.VendorUris?.TcgPlayer),
			(VendorType.CardMarket, apiCard.Prices.Eur, apiCard.Prices.EurFoil, apiCard.VendorUris?.CardMarket),
			(VendorType.CardHoarder, apiCard.Prices.Usd, apiCard.Prices.UsdFoil, apiCard.VendorUris?.CardHoarder),
			(VendorType.Mtgo, apiCard.Prices.MtgoTix, null, null),
		};

		foreach (var (vendor, price, foilPrice, purchaseUri) in cardPriceMappings)
		{
			if(price is null && foilPrice is null) continue;

			cardPrices.Add(new CardPrice
			{
				Price = price,
				FoilPrice = foilPrice,
				PurchaseUri = purchaseUri,
				VendorId = (int)vendor,
				CurrencyId = (int)GetCurrencyTypeFromVendor(vendor),
				CardId = cardId,
			});
		}

		return cardPrices;
	}

	private static CurrencyType GetCurrencyTypeFromVendor(VendorType vendor)
	{
		return vendor switch
		{
			VendorType.TcgPlayer => CurrencyType.Usd,
			VendorType.CardMarket => CurrencyType.Eur,
			VendorType.CardHoarder => CurrencyType.Usd,
			VendorType.Mtgo => CurrencyType.Tix,
			_ => CurrencyType.NotImplemented
		};
	}

	/// <summary>
	/// Assigns all intrinsic properties from the <paramref name="source"/> onto the <paramref name="target"/>.<br/>
	/// These properties represent the core data of the <see cref="CardPrice"/> (such as foreign keys, Uri, numeric values, etc.)
	/// that are directly managed by the CardPrice entity, excluding any navigational or derived properties.
	/// </summary>
	/// <param name="target">The entity being updated.</param>
	/// <param name="source">The properties of this object will be assigned to <paramref name="target"/>.</param>
	public static void MergeProperties(CardPrice target, CardPrice source)
	{
		target.Id = source.Id;
		target.Price = source.Price;
		target.FoilPrice = source.FoilPrice;
		target.PurchaseUri = source.PurchaseUri;
		target.VendorId = source.VendorId;
		target.CurrencyId = source.CurrencyId;
		target.CardId = source.CardId;
	}
}
