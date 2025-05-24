using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Repositories.Interfaces;

public interface ICardRepository
{
	/// <summary>
	/// Returns the <see cref="Card"/> from the db with the specified <paramref name="cardId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="Card"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Card> Get(long cardId);

	/// <summary>
	/// Returns the <see cref="Card"/> entries from the db with the specified <paramref name="scryfallId"/>.<br/>
	/// Multiple cards may have the same scryfallId, since they are created as seperate card instances if they have multiple <see cref="ScryfallApi.Models.CardFace"/> (such as flip or split cards).
	/// </summary>
	/// <returns>The <see cref="Card"/> instances with the specified <paramref name="scryfallId"/> or null if no match is found.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<IEnumerable<Card>> Get(Guid scryfallId);

	/// <summary>
	/// Returns the <see cref="Card"/> entries from the db with the specified <paramref name="scryfallIds"/>.<br/>
	/// Multiple cards may have the same scryfallId, since they are created as seperate card instances if they have multiple <see cref="ScryfallApi.Models.CardFace"/> (such as flip or split cards).
	/// </summary>
	/// <returns>The <see cref="Card"/> instances with the specified <paramref name="scryfallIds"/> or null if no match is found.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<IEnumerable<Card>> Get(IEnumerable<Guid> scryfallIds);

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
	/// Returns the <see cref="CardLegality"/> from the db with the specified <paramref name="cardLegalityId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="CardLegality"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardLegality> GetCardLegality(long cardLegalityId);

	/// <summary>
	/// Returns all <see cref="CardLegality"/> entities associated with the <paramref name="cardId"/> from the database.
	/// </summary>
	Task<IEnumerable<CardLegality>> GetCardLegalities(long cardId);

	/// <summary>
	/// Returns all <see cref="CardLegality"/> entities associated with the <paramref name="cardIds"/> from the database.
	/// </summary>
	Task<IEnumerable<CardLegality>> GetCardLegalities(IEnumerable<long> cardIds);

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
	/// Returns the <see cref="CardKeyword"/> from the db with the specified <paramref name="cardKeywordId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if none, or one or more <see cref="CardKeyword"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if none or more than one entity with that id is found.</exception>
	Task<CardKeyword> GetCardKeyword(long cardKeywordId);

	/// <summary>
	/// Returns all <see cref="CardKeyword"/> entities associated with the <paramref name="cardId"/> from the database.
	/// </summary>
	Task<IEnumerable<CardKeyword>> GetCardKeywords(long cardId);

	/// <summary>
	/// Returns all <see cref="CardKeyword"/> entities associated with the <paramref name="cardIds"/> from the database.
	/// </summary>
	Task<IEnumerable<CardKeyword>> GetCardKeywords(IEnumerable<long> cardIds);

	/// <summary>
	/// Return all <see cref="PromoType"/> entities from the database.
	/// </summary>
	Task<IEnumerable<PromoType>> GetPromoTypes();

	/// <summary>
	/// Returns all <see cref="PromoType"/> entities with the <paramref name="source"/> from the database.
	/// </summary>
	Task<IEnumerable<PromoType>> GetPromoTypes(SourceType source);

	/// <summary>
	/// Returns the <see cref="CardPromoType"/> from the db with the specified <paramref name="cardPromoTypeId"/>.<br/>
	/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="CardPromoType"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardPromoType> GetCardPromoType(long cardPromoTypeId);

	/// <summary>
	/// Returns all <see cref="CardPromoType"/> entities associated with the <paramref name="cardId"/> from the database.
	/// </summary>
	Task<IEnumerable<CardPromoType>> GetCardPromoTypes(long cardId);

	/// <summary>
	/// Returns all <see cref="CardPromoType"/> entities associated with the <paramref name="cardIds"/> from the database.
	/// </summary>
	Task<IEnumerable<CardPromoType>> GetCardPromoTypes(IEnumerable<long> cardIds);

	/// <summary>
	/// Returns all <see cref="CardPrice"/> entities from the database.
	/// </summary>
	Task<IEnumerable<CardPrice>> GetCardPrices();

	/// <summary>
	/// Returns all <see cref="CardPrice"/> entities associated with the <paramref name="cardIds"/> from the database.
	/// </summary>
	Task<IEnumerable<CardPrice>> GetCardPrices(IEnumerable<long> cardIds);

	/// <summary>
	/// Returns all <see cref="CardGamePlatform"/> entities from the database.
	/// </summary>
	Task<IEnumerable<CardGamePlatform>> GetCardGamePlatforms();

	/// <summary>
	/// Returns all <see cref="CardGamePlatform"/> entities associated with the <paramref name="cardId"/> from the database.
	/// </summary>
	Task<IEnumerable<CardGamePlatform>> GetCardGamePlatforms(long cardId);

	/// <summary>
	/// Returns all <see cref="CardGamePlatform"/> entities associated with the <paramref name="cardIds"/> from the database.
	/// </summary>
	Task<IEnumerable<CardGamePlatform>> GetCardGamePlatforms(IEnumerable<long> cardIds);

	/// <summary>
	/// Returns all <see cref="CardPrintFinish"/> entities from the database.
	/// </summary>
	Task<IEnumerable<CardPrintFinish>> GetCardPrintFinishes();

	/// <summary>
	/// Returns all <see cref="CardPrintFinish"/> entities associated with the <paramref name="cardId"/> from the database.
	/// </summary>
	Task<IEnumerable<CardPrintFinish>> GetCardPrintFinishes(long cardId);

	/// <summary>
	/// Returns all <see cref="CardPrintFinish"/> entities associated with the <paramref name="cardIds"/> from the database.
	/// </summary>
	Task<IEnumerable<CardPrintFinish>> GetCardPrintFinishes(IEnumerable<long> cardIds);

	/// <summary>
	/// Adds the provided <paramref name="card"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="Card"/> with identity.</returns>
	Task<Card> Create(Card card);

	/// <summary>
	/// Creates a new <see cref="CardPrice"/> entry in the database.
	/// </summary>
	/// <returns>The added <see cref="CardPrice"/> with identity.</returns>
	Task<CardPrice> Create(CardPrice priceToUpsert);

	/// <summary>
	/// Creates a new <see cref="CardPrintFinish"/> entry in the database for each <paramref name="cardPrintFinishes"/>.
	/// </summary>
	/// <returns>The added <see cref="CardPrintFinish"/> entities with identity.</returns>
	Task<IEnumerable<CardPrintFinish>> Create(IEnumerable<CardPrintFinish> cardPrintFinishes);

	/// <summary>
	/// Adds the <paramref name="legality"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="CardLegality"/> with identity.</returns>
	Task<CardLegality> Create(CardLegality legality);

	/// <summary>
	/// Adds a new <see cref="CardLegality"/> entry in the database for each <paramref name="legalities"/>.
	/// </summary>
	/// <returns>The added <see cref="CardLegality"/> entities with identity.</returns>
	Task<IEnumerable<CardLegality>> Create(IEnumerable<CardLegality> legalities);

	/// <summary>
	/// Adds the <paramref name="keyword"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="Keyword"/> with identity.</returns>
	Task<Keyword> Create(Keyword keyword);

	/// <summary>
	/// Adds a new <see cref="Keyword"/> entry in the database for each <paramref name="keywords"/>.
	/// </summary>
	/// <returns>The added <see cref="Keyword"/> entities with identity.</returns>
	Task<IEnumerable<Keyword>> Create(IEnumerable<Keyword> keywords);

	/// <summary>
	/// Adds the provided <paramref name="cardKeyword"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="CardKeyword"/> with identity.</returns>
	Task<CardKeyword> Create(CardKeyword cardKeyword);

	/// <summary>
	/// Adds a new <see cref="CardKeyword"/> entry in the database for each <paramref name="cardKeywords"/>.
	/// </summary>
	/// <returns>The added <see cref="CardKeyword"/> entities with identity.</returns>
	Task<IEnumerable<CardKeyword>> Create(IEnumerable<CardKeyword> cardKeywords);

	/// <summary>
	/// Adds the <paramref name="promoType"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="PromoType"/> with identity.</returns>
	Task<PromoType> Create(PromoType promoType);

	/// <summary>
	/// Adds a new <see cref="PromoType"/> entry in the database for each <paramref name="promoTypes"/>.
	/// </summary>
	/// <returns>The added <see cref="PromoType"/> entities with identity.</returns>
	Task<IEnumerable<PromoType>> Create(IEnumerable<PromoType> promoTypes);

	/// <summary>
	/// Adds the <paramref name="cardPromoTypes"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="CardPromoType"/> with identity.</returns>
	Task<IEnumerable<CardPromoType>> Create(IEnumerable<CardPromoType> cardPromoTypes);

	/// <summary>
	/// Adds the provided <paramref name="cardGamePlatforms"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="CardGamePlatform"/> entities with identity.</returns>
	Task<IEnumerable<CardGamePlatform>> Create(IEnumerable<CardGamePlatform> cardGamePlatforms);

	/// <summary>
	/// Updates existing entity with changes from <paramref name="cardWithChanges"/>.<br/>
	/// </summary>
	/// <returns>The updated <see cref="Card"/>.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<Card> Update(Card cardWithChanges);

	/// <summary>
	/// Updates the existing entity with changes from <paramref name="priceToUpdate"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardPrice"/> entity.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardPrice> Update(CardPrice priceToUpdate);

	/// <summary>
	/// Updates the existing entity from <paramref name="legalityWithChanges"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardLegality"/> entity.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardLegality> Update(CardLegality legalityWithChanges);

	/// <summary>
	/// Updates the existing entites from <paramref name="legalitiesWithChanges"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardLegality"/> entities.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<IEnumerable<CardLegality>> Update(IEnumerable<CardLegality> legalitiesWithChanges);

	/// <summary>
	/// Updates the existing entity with changes from <paramref name="cardKeywordWithChanges"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardKeyword"/> entity.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
	Task<CardKeyword> Update(CardKeyword cardKeywordWithChanges);

	/// <summary>
	/// Updates the existing entites with changes from <paramref name="cardKeywordWithChanges"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardKeyword"/> entites.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if none or more than one entity with the id property from the input is found.</exception>
	Task<IEnumerable<CardKeyword>> Update(IEnumerable<CardKeyword> cardKeywordWithChanges);

	/// <summary>
	/// Updates the existing entities with changes from <paramref name="cardPromoTypes"/>.
	/// </summary>
	/// <returns>The updated <see cref="CardPromoType"/> entities.</returns>
	/// <exception cref="InvalidOperationException">Is thrown if none or more than one entity with the id property from the input is found.</exception>
	Task<IEnumerable<CardPromoType>> Update(IEnumerable<CardPromoType> cardPromoTypes);

	/// <summary>
	/// Creates and updates <see cref="Card"/> entities, based on the provided <paramref name="upsertionData"/>.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="Card"/> entities.</returns>
	Task<int> Upsert(UpsertContainer<Card> upsertionData);

	/// <summary>
	/// Creates and updates <see cref="Card"/> entities, based on the provided <paramref name="upsertionData"/>.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="Card"/> entities.</returns>
	Task<int> Upsert(UpsertContainer<CardLegality> upsertionData);

	/// <summary>
	/// Creates and updates <see cref="Keyword"/> entities, based on the provided <paramref name="upsertionData"/>.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="Keyword"/> entities.</returns>
	Task<int> Upsert(UpsertContainer<Keyword> upsertionData);

	/// <summary>
	/// Creates and updates <see cref="CardPrice"/> entities, based on the provided <paramref name="upsertionData"/>.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="CardPrice"/> entities.</returns>
	Task<int> Upsert(UpsertContainer<CardPrice> upsertionData);
}
