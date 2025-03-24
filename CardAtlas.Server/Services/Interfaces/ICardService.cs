using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Services.Interfaces;

public interface ICardService
{
	/// <summary>
	/// Returns the <see cref="Card"/> from the db with the specified <paramref name="cardId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="Card"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Card> Get(long cardId);

	/// <summary>
	/// Adds the provided <paramref name="card"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="Card"/> with identity.</returns>
	Task<Card> Create(Card card);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="cardWithChanges"/>.<br/>
	/// </summary>
	/// <returns>The updated <see cref="Card"/>.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Card> Update(Card cardWithChanges);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="cardWithChanges"/>.<br/>
	/// Does not update the if there are no changes.
	/// </summary>
	/// <returns>The updated <see cref="Card"/>.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Card> UpdateIfChanged(Card cardWithChanges);

	/// <summary>
	/// Returns the <see cref="Card"/> entries from the db with the specified <paramref name="scryfallId"/>.<br/>
	/// Multiple cards may have the same scryfallId, since they are created as seperate card instances if they have multiple <see cref="ScryfallApi.Models.CardFace"/> (such as flip or split cards).
	/// </summary>
	/// <returns>The <see cref="Card"/> with the specified <paramref name="scryfallId"/> or null if no match is found.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<IEnumerable<Card>> GetFromScryfallId(Guid scryfallId);

	/// <summary>
	/// Returns the <see cref="CardPrice"/> with the specified <paramref name="priceId"/>.
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="CardPrice"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardPrice> GetPrice(long priceId);

	/// <summary>
	/// Gets all the prices for the card with the specified <paramref name="cardId"/>.
	/// </summary>
	/// <returns>All <see cref="CardPrice"/> entries which reference <paramref name="cardId"/>.</returns>
	Task<IEnumerable<CardPrice>> GetPrices(long cardId);

	/// <summary>
	/// Creates a new <see cref="CardPrice"/> entry in the database.
	/// </summary>
	/// <returns>The added <see cref="CardPrice"/> with identity.</returns>
	Task<CardPrice> CreatePrice(CardPrice priceToUpsert);

	/// <summary>
	/// Updates the existing entity with changes from <paramref name="priceToUpdate"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardPrice"/> entity.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardPrice> UpdatePrice(CardPrice priceToUpdate);

	/// <summary>
	/// Updates the existing entity with changes from <paramref name="priceToUpdate"/>.<br/>
	/// Does not update if there are no changes.
	/// </summary>
	/// <returns>The updated <see cref="CardPrice"/> entity.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardPrice> UpdatePriceIfChanged(CardPrice priceToUpdate);

	/// <summary>
	/// Creates a new <see cref="CardPrintFinish"/> entry in the database for each <paramref name="cardPrintFinishes"/>.
	/// </summary>
	/// <returns>The added <see cref="CardPrintFinish"/> entities with identity.</returns>
	Task<IEnumerable<CardPrintFinish>> CreateCardPrintFinishes(IEnumerable<CardPrintFinish> cardPrintFinishes);

	/// <summary>
	/// Returns the <see cref="CardLegality"/> from the db with the specified <paramref name="cardLegalityId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="CardLegality"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardLegality> GetCardLegality(long cardLegalityId);

	/// <summary>
	/// Adds the <paramref name="legality"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="CardLegality"/> with identity.</returns>
	Task<CardLegality> CreateCardLegality(CardLegality legality);

	/// <summary>
	/// Adds a new <see cref="CardLegality"/> entry in the database for each <paramref name="legalities"/>.
	/// </summary>
	/// <returns>The added <see cref="CardLegality"/> entities with identity.</returns>
	Task<IEnumerable<CardLegality>> CreateCardLegalities(IEnumerable<CardLegality> legalities);

	/// <summary>
	/// Updates the existing entity from <paramref name="legalityWithChanges"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardLegality"/> entity.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardLegality> UpdateCardLegality(CardLegality legalityWithChanges);

	/// <summary>
	/// Updates the existing entites from <paramref name="legalitiesWithChanges"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardLegality"/> entities.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<IEnumerable<CardLegality>> UpdateCardLegalities(IEnumerable<CardLegality> legalitiesWithChanges);

	/// <summary>
	/// Updates the existing entity with changes from <paramref name="legalityWithChanges"/>.<br/>
	/// Does not update if there are no changes.
	/// </summary>
	/// <returns>The updated <see cref="CardLegality"/> entity.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardLegality> UpdateCardLegalityIfChanged(CardLegality legalityWithChanges);

	/// <summary>
	/// Updates the existing entites with changes from <paramref name="legalitiesWithChanges"/>.<br/>
	/// Does not update if there are no changes.
	/// </summary>
	/// <returns>The updated <see cref="CardLegality"/> entites.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<IEnumerable<CardLegality>> UpdateCardLegalitiesIfChanged(IEnumerable<CardLegality> legalitiesWithChanges);

	/// <summary>
	/// Returns all <see cref="Keyword"/> entities from the database.
	/// </summary>
	/// <returns></returns>
	Task<IEnumerable<Keyword>> GetKeywords();

	/// <summary>
	/// Returns all <see cref="Keyword"/> entities with the source <paramref name="source"/> from the database.
	/// </summary>
	Task<IEnumerable<Keyword>> GetKeywords(SourceType source);

	/// <summary>
	/// Adds the <paramref name="keyword"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="Keyword"/> with identity.</returns>
	Task<Keyword> CreateKeyword(Keyword keyword);

	/// <summary>
	/// Adds a new <see cref="Keyword"/> entry in the database for each <paramref name="keywords"/>.
	/// </summary>
	/// <returns>The added <see cref="Keyword"/> entities with identity.</returns>
	Task<IEnumerable<Keyword>> CreateKeywords(IEnumerable<Keyword> keywords);

	/// <summary>
	/// Returns the <see cref="CardKeyword"/> from the db with the specified <paramref name="cardKeywordId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if none, or one or more <see cref="CardKeyword"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if none or more than one entity with that id is found.</exception>
	Task<CardKeyword> GetCardKeyword(long cardKeywordId);

	/// <summary>
	/// Adds the provided <paramref name="cardKeyword"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="CardKeyword"/> with identity.</returns>
	Task<CardKeyword> CreateCardKeyword(CardKeyword cardKeyword);

	/// <summary>
	/// Adds a new <see cref="CardKeyword"/> entry in the database for each <paramref name="cardKeywords"/>.
	/// </summary>
	/// <returns>The added <see cref="CardKeyword"/> entities with identity.</returns>
	Task<IEnumerable<CardKeyword>> CreateCardKeywords(IEnumerable<CardKeyword> cardKeywords);

	/// <summary>
	/// Updates the existing entity with changes from <paramref name="cardKeywordWithChanges"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardKeyword"/> entity.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardKeyword> UpdateCardKeyword(CardKeyword cardKeywordWithChanges);

	/// <summary>
	/// Updates the existing entites with changes from <paramref name="cardKeywordWithChanges"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardKeyword"/> entites.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if none or more than one entity with the id property from the input is found.</exception>
	Task<IEnumerable<CardKeyword>> UpdateCardKeywords(IEnumerable<CardKeyword> cardKeywordWithChanges);

	/// <summary>
	/// Return all <see cref="PromoType"/> entities from the database.
	/// </summary>
	Task<IEnumerable<PromoType>> GetPromoTypes();

	/// <summary>
	/// Returns all <see cref="PromoType"/> entities with the <paramref name="source"/> from the database.
	/// </summary>
	Task<IEnumerable<PromoType>> GetPromoTypes(SourceType source);

	/// <summary>
	/// Adds the <paramref name="promoType"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="PromoType"/> with identity.</returns>
	Task<PromoType> CreatePromoType(PromoType promoType);

	/// <summary>
	/// Adds a new <see cref="PromoType"/> entry in the database for each <paramref name="promoTypes"/>.
	/// </summary>
	/// <returns>The added <see cref="PromoType"/> entities with identity.</returns>
	Task<IEnumerable<PromoType>> CreatePromoTypes(IEnumerable<PromoType> promoTypes);
}
