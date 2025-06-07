using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Extensions;

public static class UpsertExtensions
{
	/// <summary>
	/// Assigns the identifier from <paramref name="existingEntity"/> to <paramref name="newEntity"/>.
	/// </summary>
	public delegate void AssignId<TEntity>(TEntity newEntity, TEntity existingEntity);

	/// <summary>
	/// Compares the <paramref name="batchedData"/> to the <paramref name="existingData"/> and populates a <see cref="UpsertContainer{TEntity}"/> appropriately.
	/// </summary>
	/// <param name="lookupKey">The properties on which to compare existing to batched entities.</param>
	/// <param name="assignId">Acion for assigning the identifier from existing entities to batched ones.</param>
	/// <returns>Returns a populated <see cref="UpsertContainer{TEntity}"/> with data for upsertion.<br/>
	/// Note that the container may be empty if no entities needs updating or inserting.</returns>
	public static UpsertContainer<TEntity> ToUpsertData<TEntity, TKey>(
		this IEnumerable<TEntity> batchedData,
		IEnumerable<TEntity> existingData,
		Func<TEntity, TKey> lookupKey,
		IEqualityComparer<TEntity> comparer,
		AssignId<TEntity> assignId)
		where TEntity : class
		where TKey : notnull
	{
		var upsertContainer = new UpsertContainer<TEntity>();
		Dictionary<TKey, TEntity> existingEntityLookup = existingData.ToDictionary(lookupKey);

		foreach (TEntity batchedEntity in batchedData)
		{
			if (existingEntityLookup.TryGetValue(lookupKey(batchedEntity), out TEntity? existingEntity))
			{
				assignId(batchedEntity, existingEntity);
				if (comparer.Equals(existingEntity, batchedEntity)) continue;

				upsertContainer.ToUpdate.Add(batchedEntity);
			}
			else
			{
				upsertContainer.ToInsert.Add(batchedEntity);
			}
		}

		return upsertContainer;
	}
}
