using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Extensions;

public static class GameExtensions
{
	/// <summary>
	/// Returns true if any entry in <paramref name="formats"/> has the same name as <paramref name="searchName"/> disregarding casing.
	/// </summary>
	public static bool ExistsWithName(this HashSet<GameFormat> formats, string? searchName)
	{
		if (formats is { Count: 0 } || string.IsNullOrWhiteSpace(searchName)) return false;

		return formats.Any(format =>
			string.Equals(format.Name, searchName, StringComparison.OrdinalIgnoreCase)
		);
	}

	/// <summary>
	/// Returns true if any entry in <paramref name="formats"/> has the same format <paramref name="sourceType"/> and name as <paramref name="searchName"/> disregarding casing.
	/// </summary>
	public static bool ExistsWithName(this HashSet<GameFormat> formats, string? searchName, SourceType sourceType)
	{
		if (formats is { Count: 0 } || string.IsNullOrWhiteSpace(searchName)) return false;

		return formats.Any(format =>
			format.SourceId == (int)sourceType &&
			string.Equals(format.Name, searchName, StringComparison.OrdinalIgnoreCase)
		);
	}

	/// <summary>
	/// Returns the first entry in <paramref name="formats"/> whith the same name as <paramref name="searchName"/> disregarding casing.<br/>
	/// Returns null if no entry is found.
	/// </summary>
	public static GameFormat? GetWithName(this HashSet<GameFormat> formats, string? searchName)
	{
		if (formats is { Count: 0 } || string.IsNullOrWhiteSpace(searchName)) return null;

		return formats.FirstOrDefault(format =>
			string.Equals(format.Name, searchName, StringComparison.OrdinalIgnoreCase)
		);
	}


	/// <summary>
	/// Returns the first entry in <paramref name="formats"/> whith the same <paramref name="sourceType"/> and name as <paramref name="searchName"/> disregarding casing.<br/>
	/// Returns null if no entry is found.
	/// </summary>
	public static GameFormat? GetWithName(this HashSet<GameFormat> formats, string? searchName, SourceType sourceType)
	{
		if (formats is { Count: 0 } || string.IsNullOrWhiteSpace(searchName)) return null;

		return formats.FirstOrDefault(format =>
			format.SourceId == (int)sourceType &&
			string.Equals(format.Name, searchName, StringComparison.OrdinalIgnoreCase)
		);
	}
}
