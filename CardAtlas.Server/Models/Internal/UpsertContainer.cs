namespace CardAtlas.Server.Models.Internal;

public class UpsertContainer<TEntity> where TEntity : class
{
	public List<TEntity> ToInsert { get; set; } = new();
	public List<TEntity> ToUpdate { get; set; } = new();
}
