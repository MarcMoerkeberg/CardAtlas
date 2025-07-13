using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Interfaces;

namespace CardAtlas.Server.Extensions;

public static class ScryfallIngestionExtensions
{
	/// <summary>
	/// Assigns the <see cref="Card.Id"/> of each card to the corresponding entities in the <paramref name="batchedEntities"/>.
	/// </summary>
	/// <typeparam name="TRelation">The type of the entity that are related to a <see cref="Card"/>. Must implement <see cref="ICardRelateable"/>.</typeparam>
	/// <param name="batchedEntities">Expects the keys to be assigned from <see cref="Card.ScryfallId"/> and match an entry in <paramref name="cards"/>.</param>
	/// <param name="cards">These should be persisted or have assigned <see cref="Card.Id"/>.</param>
	public static void AssignCardIdToEntities<TRelation>(
		this Dictionary<Guid, List<TRelation>> batchedEntities,
		IEnumerable<Card> cards)
		where TRelation : ICardRelateable
	{
		foreach (Card card in cards)
		{
			if (!card.ScryfallId.HasValue || card.ScryfallId == Guid.Empty) continue;
			if (!batchedEntities.TryGetValue(card.ScryfallId.Value, out List<TRelation>? batchedItems)) continue;

			foreach (TRelation relationalEntity in batchedItems)
			{
				relationalEntity.CardId = card.Id;
			}
		}
	}

	/// <summary>
	/// Assigns the <see cref="Card.Id"/> of each card to the corresponding entities in the <paramref name="batchedEntities"/>.
	/// </summary>
	/// <typeparam name="TRelation">The type of the entity that are related to a <see cref="Card"/>. Must implement <see cref="ICardRelateable"/>.</typeparam>
	/// <param name="batchedEntities">Expects the keys to be assigned from <see cref="Card.ScryfallId"/> and match an entry in <paramref name="cards"/>.</param>
	/// <param name="cards">These should be persisted or have assigned <see cref="Card.Id"/>.</param>
	public static void AssignCardIdToEntities<TRelation>(
		this Dictionary<(Guid cardScryfallId, string cardName), List<TRelation>> batchedEntities,
		IEnumerable<Card> cards)
		where TRelation : ICardRelateable
	{
		foreach (Card card in cards)
		{
			if (!card.ScryfallId.HasValue || card.ScryfallId == Guid.Empty) continue;
			if (!batchedEntities.TryGetValue((card.ScryfallId.Value, card.Name), out List<TRelation>? batchedItems)) continue;

			foreach (TRelation relationalEntity in batchedItems)
			{
				relationalEntity.CardId = card.Id;
			}
		}
	}

	/// <summary>
	/// Assigns the <see cref="Card.Id"/> of each card to the corresponding entities in the <paramref name="batchedItems"/>.
	/// </summary>
	/// <typeparam name="TRelation">The type of the entity that are related to a <see cref="Card"/>. Must implement <see cref="ICardRelateable"/>.</typeparam>
	/// <param name="batchedItems">Expects the keys to be assigned from <see cref="Card.ScryfallId"/> and match an entry in <paramref name="cards"/>.</param>
	/// <param name="cards">These should be persisted or have assigned <see cref="Card.Id"/>.</param>
	public static void AssignCardIdToEntities<TRelation>(
		this Dictionary<Guid, List<(string name, TRelation relation)>> batchedItems,
		IEnumerable<Card> cards)
		where TRelation : ICardRelateable
	{
		foreach (Card card in cards)
		{
			if (!card.ScryfallId.HasValue || card.ScryfallId == Guid.Empty) continue;
			if (!batchedItems.TryGetValue(card.ScryfallId.Value, out List<(string, TRelation)>? tuples)) continue;

			foreach ((string _, TRelation relationalEntity) in tuples)
			{
				relationalEntity.CardId = card.Id;
			}
		}
	}

	/// <summary>
	/// Assigns Id property from <typeparamref name="TEntity"/> onto <typeparamref name="TRelation"/> using the <paramref name="assignId"/> action.<br/>
	/// This method is used for assigning Ids to relational entities before persisting it. ie. Relationships between <see cref="Card"/> and <see cref="Keyword"/>.
	/// </summary>
	/// <typeparam name="TEntity">The entity containing the persisted Id. Ie. <see cref="Keyword"/>.</typeparam>
	/// <typeparam name="TRelation">The relational entity, which should recieve the Id. Ie. <see cref="CardKeyword"/>.</typeparam>
	/// <param name="entitiesWithIds">Entities with identity to create relations from. Ie. <see cref="Keyword"/>.</param>
	/// <param name="batchedData">The batched relational entities, which should have the foreign key assigned. Name is used for matching with existing entities.</param>
	/// <param name="assignId">The action for setting the Id. This is where you can assign specific properties to the id of the existing entities. Ie. Assigning <see cref="Keyword.Id"/> to <see cref="CardKeyword.KeywordId"/>.</param>
	public static void AssignRelationalIdToEntities<TEntity, TRelation>(
		this Dictionary<Guid, List<(string name, TRelation relation)>> batchedData,
		IEnumerable<TEntity> entitiesWithIds,
		Action<TRelation, int> assignId)
		where TEntity : IIdable<int>, INameable
	{
		if (!entitiesWithIds.Any() || batchedData.Count == 0) return;

		Dictionary<string, TEntity> existingEntityLookup = entitiesWithIds.ToDictionary(entity => entity.Name, StringComparer.OrdinalIgnoreCase);

		foreach ((string batchedName, TRelation batchedRelationalEntity) in batchedData.Values.SelectMany(tuple => tuple))
		{
			if (!existingEntityLookup.TryGetValue(batchedName, out TEntity? existingEntity)) continue;

			assignId(batchedRelationalEntity, existingEntity.Id);
		}
	}

	/// <summary>
	/// Finds entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.
	/// </summary>
	/// <param name="omitDefaultValues">The properties to omit and their default values.</param>
	/// <param name="filterExistingEntities">Filter for comparing properties between existing and new entities.</param>
	/// <returns>Returns a new list of all missing entities - Ie. Entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.</returns>
	private static List<TEntity> FindMissingEntities<TEntity, TFilter>(
		Dictionary<Guid, List<TEntity>> batchedEntities,
		IEnumerable<TEntity> existingEntities,
		Func<TEntity, bool> omitDefaultValues,
		Func<TEntity, TFilter> filterExistingEntities)
		where TFilter : notnull
	{
		IEnumerable<TEntity> allBatchedEntities = batchedEntities
			.Values
			.SelectMany(entityList => entityList)
			.Where(omitDefaultValues);

		if (!existingEntities.Any())
		{
			return allBatchedEntities.ToList();
		}

		HashSet<TFilter> existingEntitiesFilter = existingEntities
			.Select(filterExistingEntities)
			.ToHashSet();


		return allBatchedEntities
			.Where(entity => !existingEntitiesFilter.Contains(filterExistingEntities(entity)))
			.ToList();
	}

	/// <summary>
	/// Finds entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.
	/// </summary>
	/// <param name="omitDefaultValues">The properties to omit and their default values.</param>
	/// <param name="filterExistingEntities">Filter for comparing properties between existing and new entities.</param>
	/// <returns>Returns a new list of all missing entities - Ie. Entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.</returns>
	public static List<TEntity> FindMissingEntities<TEntity, TFilter>(
		this Dictionary<Guid, List<(string _, TEntity entity)>> batchedEntities,
		IEnumerable<TEntity> existingEntities,
		Func<TEntity, bool> omitDefaultValues,
		Func<TEntity, TFilter> filterExistingEntities)
		where TFilter : notnull
	{
		Dictionary<Guid, List<TEntity>> simplifiedBatch = batchedEntities.ToDictionary(
			kvp => kvp.Key,
			kvp => kvp.Value.Select(tuple => tuple.entity).ToList()
		);

		return FindMissingEntities(
			simplifiedBatch,
			existingEntities,
			omitDefaultValues,
			filterExistingEntities
		);
	}

	/// <summary>
	/// Finds entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.<br/>
	/// Skips any entities where CardId property is default value.
	/// </summary>
	/// <param name="filterExistingEntities">Filter for comparing properties between existing and new entities.</param>
	/// <returns>Returns a new list of all missing entities - Ie. Entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.</returns>
	public static List<TEntity> FindMissingEntities<TEntity, TFilter>(
		this Dictionary<Guid, List<TEntity>> batchedEntities,
		IEnumerable<TEntity> existingEntities,
		Func<TEntity, TFilter> filterExistingEntities)
		where TEntity : ICardRelateable
		where TFilter : notnull
	{
		return FindMissingEntities(
			batchedEntities,
			existingEntities,
			omitDefaultValues: entity => entity.CardId != 0,
			filterExistingEntities
		);
	}

	/// <summary>
	/// Finds entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.<br/>
	/// It uses <typeparamref name="TEntity"/>.Name for comparing entities.
	/// </summary>
	/// <typeparam name="TEntity">Must implement the <see cref="INameable"/> interface.</typeparam>
	/// <returns>A new list of all missing entities. Ie. Entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.</returns>
	public static List<TEntity> FindMissingEntities<TEntity>(
		this HashSet<TEntity> batchedEntities,
		IEnumerable<TEntity> existingEntities)
		where TEntity : class, INameable
	{
		IEnumerable<TEntity> missingEntities = existingEntities.Any()
			? batchedEntities.Where(entity => !existingEntities.ExistsWithName(entity.Name))
			: batchedEntities;

		return missingEntities.ToList();
	}

	/// <summary>
	/// Finds entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.
	/// </summary>
	/// <param name="filterExistingEntities">Filter for comparing properties between existing and new entities.</param>
	/// <returns>Returns a new list of all missing entities - Ie. Entities in <paramref name="batchedEntities"/> that are not in <paramref name="existingEntities"/>.</returns>
	public static List<TEntity> FindMissingEntities<TEntity, TFilter>(
	this IEnumerable<TEntity> batchedEntities,
	IEnumerable<TEntity> existingEntities,
	Func<TEntity, TFilter> filterExistingEntities)
	where TFilter : notnull
	{
		if (!existingEntities.Any())
		{
			return batchedEntities.ToList();
		}

		HashSet<TFilter> existingEntitiesFilter = existingEntities
			.Select(filterExistingEntities)
			.ToHashSet();

		IEnumerable<TEntity> missingEntities = batchedEntities.Where(batchedEntity =>
			!existingEntitiesFilter.Contains(filterExistingEntities(batchedEntity))
		);

		return missingEntities.ToList();
	}
}
