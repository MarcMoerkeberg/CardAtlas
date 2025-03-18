using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Interfaces;

namespace CardAtlas.Server.Extensions;

public static class IEnumerableExtensions
{
	/// <summary>
	/// Returns true if <paramref name="enumerable"/> is null or has no entries.
	/// </summary>
	public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
	{
		return enumerable is null || !enumerable.Any();
	}


	/// <summary>
	/// Returns the first entry in <paramref name="targetCollection"/> whith the same name as <paramref name="searchName"/> disregarding casing.<br/>
	/// Returns default of <typeparamref name="TSource"/> if no entry is found.
	/// </summary>
	public static TSource? FirstWithNameOrDefault<TSource>(this IEnumerable<TSource> targetCollection, string? searchName) where TSource : INameable
	{
		if (!targetCollection.Any() || string.IsNullOrWhiteSpace(searchName)) return default;

		return targetCollection.FirstOrDefault(target =>
			string.Equals(target.Name, searchName, StringComparison.OrdinalIgnoreCase)
		);
	}


	/// <summary>
	/// Returns the first entry in <paramref name="targetCollection"/> whith the same <paramref name="sourceType"/> and name as <paramref name="searchName"/> disregarding casing.<br/>
	/// Returns default of <typeparamref name="TSource"/> if no entry is found.
	/// </summary>
	public static TSource? FirstWithNameOrDefault<TSource>(this IEnumerable<TSource> targetCollection, string? searchName, SourceType sourceType) where TSource : INameable, ISourceable
	{
		if (!targetCollection.Any() || string.IsNullOrWhiteSpace(searchName)) return default;

		return targetCollection.FirstOrDefault(target =>
			target.SourceId == (int)sourceType &&
			string.Equals(target.Name, searchName, StringComparison.OrdinalIgnoreCase)
		);
	}

	/// <summary>
	/// Returns true if any entry in <paramref name="targetCollection"/> has the same name as <paramref name="searchName"/> disregarding casing.
	/// </summary>
	public static bool ExistsWithName<TSource>(this IEnumerable<TSource> targetCollection, string? searchName) where TSource : INameable
	{
		if (!targetCollection.Any() || string.IsNullOrWhiteSpace(searchName)) return false;

		return targetCollection.Any(target =>
			string.Equals(target.Name, searchName, StringComparison.OrdinalIgnoreCase)
		);
	}

	/// <summary>
	/// Returns true if any entry in <paramref name="targetCollection"/> has the same format <paramref name="sourceType"/> and name as <paramref name="searchName"/> disregarding casing.
	/// </summary>
	public static bool ExistsWithName<TSource>(this IEnumerable<TSource> targetCollection, string? searchName, SourceType sourceType) where TSource : INameable, ISourceable
	{
		if (!targetCollection.Any() || string.IsNullOrWhiteSpace(searchName)) return false;

		return targetCollection.Any(target =>
			target.SourceId == (int)sourceType &&
			string.Equals(target.Name, searchName, StringComparison.OrdinalIgnoreCase)
		);
	}
}
