using CardAtlas.Server.Extensions;
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
	//key tuple is needed because cardscryfallid is not unique. value tuple is needed to assign artist id.
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

	/// <summary>
	/// The <see cref="Artist.ScryfallId"/> off every artist in this batch where it has a value.
	/// </summary>
	public IEnumerable<Guid> ArtistScryfallIds => Artists
		.Where(artist => artist.ScryfallId is not null)
		.Select(artist => artist.ScryfallId!.Value);

	/// <summary>
	/// Is null if referenced before populating via <see cref="AssignCardIdToEntities"/>.
	/// </summary>
	public HashSet<long> CardIds { get; private set; } = new();

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
		CardIds.Clear();
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
		CardIds = cards.Select(card => card.Id).ToHashSet();

		Dictionary<(Guid, string), List<CardArtist>> flattenedCardArtistBatch = CardArtistRelations.ToDictionary(
			batch => batch.Key,
			batch => batch.Value.Select(tuple => tuple.cardArtist).ToList()
		);

		flattenedCardArtistBatch.AssignCardIdToEntities(cards);
	}

	/// <summary>
	/// Assigns the <see cref="GameFormat.Id"/> from the provided <paramref name="gameFormats"/> to entities with relations to <see cref="GameFormat"/>.
	/// </summary>
	public void AssignGameFormatIdToEntities(IEnumerable<GameFormat> gameFormats) =>
		CardLegalityRelations.AssignRelationalIdToEntities(
			gameFormats,
			(cardLegality, id) => cardLegality.GameFormatId = id
		);

	/// <summary>
	/// Assigns the <see cref="Keyword.Id"/> from the provided <paramref name="keywords"/> to entities with relations to <see cref="Keyword"/>.
	/// </summary>
	public void AssignKeywordIdToEntities(IEnumerable<Keyword> keywords) =>
		CardKeywordRelations.AssignRelationalIdToEntities(
			keywords,
			(cardKeyword, id) => cardKeyword.KeywordId = id
		);

	/// <summary>
	/// Assigns the <see cref="PromoType.Id"/> from the provided <paramref name="promoTypes"/> to entities with relations to <see cref="PromoType"/>.
	/// </summary>
	public void AssignPromoTypesIdToEntities(IEnumerable<PromoType> promoTypes) =>
		CardPromoTypeRelations.AssignRelationalIdToEntities(
			promoTypes,
			(cardPromoType, id) => cardPromoType.PromoTypeId = id
		);

	/// <summary>
	/// Assigns the <see cref="Artist.Id"/> from the provided <paramref name="artists"/> to entities with relations to <see cref="Artist"/>.
	/// </summary>
	public void AssignArtistIdToEntities(IEnumerable<Artist> artists)
	{
		Dictionary<Guid, Artist> artistLookup = artists.ToDictionary(artist => artist.ScryfallId!.Value);

		foreach ((Guid artistScryfallId, CardArtist batchedCardArtist) in CardArtistRelations.Values.SelectMany(tuple => tuple))
		{
			if (!artistLookup.TryGetValue(artistScryfallId, out Artist? existingArtist)) continue;

			batchedCardArtist.ArtistId = existingArtist.Id;
		}
	}
}
