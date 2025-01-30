namespace CardAtlas.Server.Helpers;

public static class ScryfallHelper
{
	public static TEnum ParseAsScryfallEnum<TEnum>(this string target) where TEnum : struct, Enum
	{
		return Enum.TryParse(target, ignoreCase: true, out TEnum parsedResult)
			? parsedResult
			: default;
	}
}
