namespace CardAtlas.Server.Helpers;

public static class ScryfallHelper
{
	public static IEnumerable<TEnum> ParseAsScryfallEnum<TEnum>(this IEnumerable<string> targets) where TEnum : struct, Enum
	{
		return targets.Select(target => target.ParseAsScryfallEnum<TEnum>());
	}

	public static TEnum ParseAsScryfallEnum<TEnum>(this string target) where TEnum : struct, Enum
	{
		string normalizedTarget = target.Replace("_", string.Empty);
		return Enum.TryParse(normalizedTarget, ignoreCase: true, out TEnum parsedResult)
			? parsedResult
			: default;
	}

	public static Dictionary<TEnum, TValue> ParseDictionaryAsScryfallEnum<TEnum, TValue>(this Dictionary<string, string> targetDictionary) where TEnum : struct, Enum
	{
		var result = new Dictionary<TEnum, TValue>();

		//TODO: Implement

		return result;
	}
}
