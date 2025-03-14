namespace CardAtlas.Server.Extensions;

public static class StringExtensions
{
	public static string CapitalizeFirstLetter(this string? target)
	{
		if (string.IsNullOrWhiteSpace(target)) return string.Empty;
	
		return char.ToUpper(target[0]) + target[1..];
	}
}
