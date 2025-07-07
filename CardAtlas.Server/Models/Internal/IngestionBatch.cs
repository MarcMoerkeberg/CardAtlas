using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Image;

namespace CardAtlas.Server.Models.Internal;

public class IngestionBatch
{
	public HashSet<Card> CardBatch { get; set; } = new();
	public HashSet<Artist> ArtistBatch { get; set; } = new();
	public HashSet<GameFormat> GameFormatsBatch { get; set; } = new();
	public HashSet<Keyword> KeywordsBatch { get; set; } = new();
	public HashSet<PromoType> PromoTypesBatch { get; set; } = new();
	public Dictionary<(Guid cardScryfallId, string cardName), List<CardImage>> ImageBatch { get; set; } = new();
	public Dictionary<(Guid cardScryfallId, string cardName), List<(Guid artistScryfallId, CardArtist cardArtist)>> CardArtistBatch { get; set; } = new();
	public Dictionary<Guid, List<CardPrice>> CardPriceBatch { get; set; } = new();
	public Dictionary<Guid, List<CardGamePlatform>> CardGamePlatformBatch { get; set; } = new();
	public Dictionary<Guid, List<CardPrintFinish>> CardPrintFinishBatch { get; set; } = new();
	public Dictionary<Guid, List<(string formatName, CardLegality legality)>> CardLegalitiesBatch { get; set; } = new();
	public Dictionary<Guid, List<(string keywordName, CardKeyword cardKeyword)>> CardKeywordsBatch { get; set; } = new();
	public Dictionary<Guid, List<(string promoTypeName, CardPromoType cardPromoType)>> CardPromoTypesBatch { get; set; } = new();

	public IngestionBatch(
		IEqualityComparer<Artist> artistComparer,
		IEqualityComparer<GameFormat> gameFormatComparer,
		IEqualityComparer<Keyword> keywordComparer,
		IEqualityComparer<PromoType> promoTypeComparer)
	{
		ArtistBatch = new HashSet<Artist>(artistComparer);
		GameFormatsBatch = new HashSet<GameFormat>(gameFormatComparer);
		KeywordsBatch = new HashSet<Keyword>(keywordComparer);
		PromoTypesBatch = new HashSet<PromoType>(promoTypeComparer);
	}

	public void Flush()
	{
		CardBatch.Clear();
		ImageBatch.Clear();
		ArtistBatch.Clear();
		CardArtistBatch.Clear();
		CardPriceBatch.Clear();
		CardGamePlatformBatch.Clear();
		CardPrintFinishBatch.Clear();
		GameFormatsBatch.Clear();
		CardLegalitiesBatch.Clear();
		KeywordsBatch.Clear();
		CardKeywordsBatch.Clear();
		PromoTypesBatch.Clear();
		CardPromoTypesBatch.Clear();
	}

	public void Merge(IngestionBatch batch)
	{
		CardBatch.UnionWith(batch.CardBatch);
		ArtistBatch.UnionWith(batch.ArtistBatch);
		GameFormatsBatch.UnionWith(batch.GameFormatsBatch);
		KeywordsBatch.UnionWith(batch.KeywordsBatch);
		PromoTypesBatch.UnionWith(batch.PromoTypesBatch);

		foreach ((var key, var value) in batch.ImageBatch)
		{
			ImageBatch[key] = value;
		}

		foreach ((var key, var value) in batch.CardArtistBatch)
		{
			CardArtistBatch[key] = value;
		}

		foreach ((var key, var value) in batch.CardPriceBatch)
		{
			CardPriceBatch[key] = value;
		}

		foreach ((var key, var value) in batch.CardGamePlatformBatch)
		{
			CardGamePlatformBatch[key] = value;
		}

		foreach ((var key, var value) in batch.CardPrintFinishBatch)
		{
			CardPrintFinishBatch[key] = value;
		}

		foreach ((var key, var value) in batch.CardLegalitiesBatch)
		{
			CardLegalitiesBatch[key] = value;
		}

		foreach ((var key, var value) in batch.CardKeywordsBatch)
		{
			CardKeywordsBatch[key] = value;
		}

		foreach ((var key, var value) in batch.CardPromoTypesBatch)
		{
			CardPromoTypesBatch[key] = value;
		}
	}
}
