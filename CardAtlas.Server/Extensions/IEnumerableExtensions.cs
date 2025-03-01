namespace CardAtlas.Server.Extensions;

public static class IEnumerableExtensions
{
	public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
	{
		return enumerable is null || !enumerable.Any();
	}
}
