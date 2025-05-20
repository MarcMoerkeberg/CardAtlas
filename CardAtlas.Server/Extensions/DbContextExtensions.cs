using CardAtlas.Server.DAL;
using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Extensions;

public static class DbContextExtensions
{
	public static async Task<int> UpsertAsync<TEntity>(this ApplicationDbContext context, UpsertContainer<TEntity> upsertionData)
		where TEntity : class
	{
		if (upsertionData is null) throw new ArgumentNullException(nameof(upsertionData));
		var setEntity = context.Set<TEntity>();

		if (upsertionData.ToInsert is { Count: > 0 })
		{
			await setEntity.AddRangeAsync(upsertionData.ToInsert);
		}

		if (upsertionData.ToUpdate is { Count: > 0 })
		{
			setEntity.UpdateRange(upsertionData.ToUpdate);
		}

		return await context.SaveChangesAsync();
	}
}
