using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.Server.Models.Interfaces;
using CardAtlas.Server.Models.Internal;
using System.Numerics;

namespace CardAtlas.Server.Extensions;

public static class UpsertExtensions
{
	/// <summary>
	/// Compares the <paramref name="batchedEntities"/> to the <paramref name="existingEntities"/> and populates a <see cref="UpsertContainer{TEntity}"/> appropriately.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey">The key to compare with existing entities (ex. cardId and imageTypeId).</typeparam>
	/// <typeparam name="TEntityId">The Id type on TEntity. This is necessary to allow for dirrefent types of ids to be automatically assigned.</typeparam>
	/// <param name="lookupKey">The properties on which to compare existing to batched entities. The types should match those on <typeparamref name="TKey"/>.</param>
	/// <returns>Returns a populated <see cref="UpsertContainer{TEntity}"/> with data for upsertion.<br/>
	/// Note that the container may be empty if no entities needs updating or inserting.</returns>
	public static UpsertContainer<TEntity> ToUpsertData<TEntity, TKey, TEntityId>(
		this IEnumerable<TEntity> batchedEntities,
		IEnumerable<TEntity> existingEntities,
		Func<TEntity, TKey> lookupKey,
		IEqualityComparer<TEntity> comparer)
		where TEntity : class, IIdable<TEntityId>
		where TKey : notnull
		where TEntityId : INumber<TEntityId>
	{
		var upsertContainer = new UpsertContainer<TEntity>();
		Dictionary<TKey, TEntity> existingEntityLookup = existingEntities.ToDictionary(lookupKey);

		foreach (TEntity batchedEntity in batchedEntities)
		{
			TKey key = lookupKey(batchedEntity);

			if (existingEntityLookup.TryGetValue(key, out TEntity? existingEntity))
			{
				batchedEntity.Id = existingEntity.Id;
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

	public static UpsertContainer<CardLegality> ToUpsertData(
	this IEnumerable<CardLegality> batchedEntities,
	IEnumerable<CardLegality> existingEntities,
	IEqualityComparer<CardLegality> comparer)
	{
		return batchedEntities.ToUpsertData<CardLegality, (long, long), long>(
			existingEntities,
			cardLegality => (cardLegality.CardId, cardLegality.GameFormatId),
			comparer
		);
	}

	public static UpsertContainer<CardPrice> ToUpsertData(
	this IEnumerable<CardPrice> batchedEntities,
	IEnumerable<CardPrice> existingEntities,
	IEqualityComparer<CardPrice> comparer)
	{
		return batchedEntities.ToUpsertData<CardPrice, (long, int, int), long>(
			existingEntities,
			cardPrice => (cardPrice.CardId, cardPrice.VendorId, cardPrice.CurrencyId),
			comparer
		);
	}

	public static UpsertContainer<CardImage> ToUpsertData(
	this IEnumerable<CardImage> batchedEntities,
	IEnumerable<CardImage> existingEntities,
	IEqualityComparer<CardImage> comparer)
	{
		return batchedEntities.ToUpsertData<CardImage, (long, int), long>(
			existingEntities,
			image => (image.CardId, image.ImageTypeId),
			comparer
		);
	}

	public static UpsertContainer<Artist> ToUpsertData(
	this IEnumerable<Artist> batchedEntities,
	IEnumerable<Artist> existingEntities,
	IEqualityComparer<Artist> comparer)
	{
		return batchedEntities.ToUpsertData<Artist, Guid, int>(
			existingEntities,
			artist => artist.ScryfallId!.Value,
			comparer
		);
	}


}
