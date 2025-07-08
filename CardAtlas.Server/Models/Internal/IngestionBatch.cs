using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Image;

namespace CardAtlas.Server.Models.Internal;

public class IngestionBatch
{
	public HashSet<Card> Cards { get; set; } = new();
	public HashSet<Artist> Artists { get; set; }
	public HashSet<GameFormat> GameFormats { get; set; }
	public HashSet<Keyword> Keywords { get; set; }
	public HashSet<PromoType> PromoTypes { get; set; }
	public Dictionary<(Guid cardScryfallId, string cardName), List<CardImage>> Images { get; set; } = new();
	public Dictionary<(Guid cardScryfallId, string cardName), List<(Guid artistScryfallId, CardArtist cardArtist)>> CardArtistRelations { get; set; } = new();
	public Dictionary<Guid, List<CardPrice>> CardPrices { get; set; } = new();
	public Dictionary<Guid, List<CardGamePlatform>> CardGamePlatformRelations { get; set; } = new();
	public Dictionary<Guid, List<CardPrintFinish>> CardPrintFinishRelations { get; set; } = new();
	public Dictionary<Guid, List<(string formatName, CardLegality legality)>> CardLegalityRelations { get; set; } = new();
	public Dictionary<Guid, List<(string keywordName, CardKeyword cardKeyword)>> CardKeywordRelations { get; set; } = new();
	public Dictionary<Guid, List<(string promoTypeName, CardPromoType cardPromoType)>> CardPromoTypeRelations { get; set; } = new();

	/// <summary>
	/// The <see cref="Card.ScryfallId"/> off every card in this batch where it has a value.
	/// </summary>
	public IEnumerable<Guid> CardScryfallIds => Cards
		.Where(card => card.ScryfallId is not null)
		.Select(card => card.ScryfallId!.Value);

	public IngestionBatch(
		IEqualityComparer<Artist> artistComparer,
		IEqualityComparer<GameFormat> gameFormatComparer,
		IEqualityComparer<Keyword> keywordComparer,
		IEqualityComparer<PromoType> promoTypeComparer)
	{
		Artists = new HashSet<Artist>(artistComparer);
		GameFormats = new HashSet<GameFormat>(gameFormatComparer);
		Keywords = new HashSet<Keyword>(keywordComparer);
		PromoTypes = new HashSet<PromoType>(promoTypeComparer);
	}

	/// <summary>
	/// Flushes all batched entities.
	/// </summary>
	public void Flush()
	{
		Cards.Clear();
		Images.Clear();
		Artists.Clear();
		CardArtistRelations.Clear();
		CardPrices.Clear();
		CardGamePlatformRelations.Clear();
		CardPrintFinishRelations.Clear();
		GameFormats.Clear();
		CardLegalityRelations.Clear();
		Keywords.Clear();
		CardKeywordRelations.Clear();
		PromoTypes.Clear();
		CardPromoTypeRelations.Clear();
	}

	/// <summary>
	/// Merges the provided <paramref name="batch"/> into the existing batched entities.
	/// </summary>
	public void Merge(IngestionBatch batch)
	{
		Cards.UnionWith(batch.Cards);
		Artists.UnionWith(batch.Artists);
		GameFormats.UnionWith(batch.GameFormats);
		Keywords.UnionWith(batch.Keywords);
		PromoTypes.UnionWith(batch.PromoTypes);

		foreach ((var key, var value) in batch.Images)
		{
			Images[key] = value;
		}

		foreach ((var key, var value) in batch.CardArtistRelations)
		{
			CardArtistRelations[key] = value;
		}

		foreach ((var key, var value) in batch.CardPrices)
		{
			CardPrices[key] = value;
		}

		foreach ((var key, var value) in batch.CardGamePlatformRelations)
		{
			CardGamePlatformRelations[key] = value;
		}

		foreach ((var key, var value) in batch.CardPrintFinishRelations)
		{
			CardPrintFinishRelations[key] = value;
		}

		foreach ((var key, var value) in batch.CardLegalityRelations)
		{
			CardLegalityRelations[key] = value;
		}

		foreach ((var key, var value) in batch.CardKeywordRelations)
		{
			CardKeywordRelations[key] = value;
		}

		foreach ((var key, var value) in batch.CardPromoTypeRelations)
		{
			CardPromoTypeRelations[key] = value;
		}
	}

	/// <summary>
	/// Assigns <see cref="Card.Id"/> from the provided <paramref name="cards"/> to the batched entities with relations to <see cref="Card"/>.
	/// </summary>
	public void AssignCardIdToEntities(IEnumerable<Card> cards)
	{
		Images.AssignCardIdToEntities(cards);
		CardPrices.AssignCardIdToEntities(cards);
		CardGamePlatformRelations.AssignCardIdToEntities(cards);
		CardPrintFinishRelations.AssignCardIdToEntities(cards);
		CardLegalityRelations.AssignCardIdToEntities(cards);
		CardKeywordRelations.AssignCardIdToEntities(cards);
		CardPromoTypeRelations.AssignCardIdToEntities(cards);

		Dictionary<(Guid, string), List<CardArtist>> flattenedCardArtistBatch = CardArtistRelations.ToDictionary(
			batch => batch.Key,
			batch => batch.Value.Select(tuple => tuple.cardArtist).ToList()
		);

		flattenedCardArtistBatch.AssignCardIdToEntities(cards);
	}

}
