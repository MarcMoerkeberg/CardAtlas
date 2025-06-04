namespace CardAtlas.Server.Extensions;

public static class StringExtensions
{
	/// <summary>
	/// Capitalizes the first letter of the specified string.
	/// </summary>
	/// <returns>A new string with the first letter capitalized. Returns an empty string if
	/// <paramref name="target"/> is null, empty, or consists only of whitespace.</returns>
	public static string CapitalizeFirstLetter(this string? target)
	{
		if (string.IsNullOrWhiteSpace(target)) return string.Empty;
	
		return char.ToUpper(target[0]) + target[1..];
	}
}
