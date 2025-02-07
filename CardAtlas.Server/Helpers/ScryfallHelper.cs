namespace CardAtlas.Server.Helpers;

public static class ScryfallHelper
{
	public static IEnumerable<TEnum> ParseAsScryfallEnum<TEnum>(this IEnumerable<string> targets) where TEnum : struct, Enum
	{
		return targets.Select(target => target.ParseAsScryfallEnum<TEnum>());
	}

	public static TEnum ParseAsScryfallEnum<TEnum>(this string target) where TEnum : struct, Enum
	{
		string normalizedTarget = NormalizeStringForEnumParsing(target);
		return Enum.TryParse(normalizedTarget, ignoreCase: true, out TEnum parsedResult)
			? parsedResult
			: default;
	}

	private static string NormalizeStringForEnumParsing(string target)
	{
		return target.Replace("_", string.Empty);
	}

	public static Dictionary<TKey, TValue> ToEnumKeyDictionary<TKey, TValue>(this Dictionary<string, string>? targetDictionary) where TKey : struct, Enum
	{
		var result = new Dictionary<TKey, TValue>();
		if (targetDictionary is null)
		{
			return new Dictionary<TKey, TValue>();
		}

		foreach (var (key, value) in targetDictionary)
		{
			bool keyParsed = Enum.TryParse(key, ignoreCase: true, out TKey parsedKey);
			bool valueIsNullable = Nullable.GetUnderlyingType(typeof(TValue)) != null;
			(bool parseValueSuccess, TValue? parsedValue) = valueIsNullable
				? (true, default)
				: ParseAs<TValue>(value);

			if (keyParsed && parseValueSuccess)
			{
				result.Add(parsedKey, parsedValue!);
			}
		}

		return result;
	}

	private static (bool parsingSuccess, T parsedValue) ParseAs<T>(string target)
	{
		Type type = typeof(T);
		var errorResult = (false, default(T)!);

		return type switch
		{
			_ when type == typeof(bool) => (bool.TryParse(target, out bool boolValue), (T)(object)boolValue),
			_ when type == typeof(byte) => (byte.TryParse(target, out byte byteValue), (T)(object)byteValue),
			_ when type == typeof(char) => (char.TryParse(target, out char charValue), (T)(object)charValue),
			_ when type == typeof(decimal) => (decimal.TryParse(target, out decimal decimalValue), (T)(object)decimalValue),
			_ when type == typeof(double) => (double.TryParse(target, out double doubleValue), (T)(object)doubleValue),
			_ when type == typeof(float) => (float.TryParse(target, out float floatValue), (T)(object)floatValue),
			_ when type == typeof(int) => (int.TryParse(target, out int intValue), (T)(object)intValue),
			_ when type == typeof(long) => (long.TryParse(target, out long longValue),(T)(object)longValue),
			_ when type == typeof(sbyte) => (sbyte.TryParse(target, out sbyte sbyteValue), (T)(object)sbyteValue),
			_ when type == typeof(short) => (short.TryParse(target, out short shortValue), (T)(object)shortValue),
			_ when type == typeof(uint) => (uint.TryParse(target, out uint uintValue), (T)(object)uintValue),
			_ when type == typeof(ulong) => (ulong.TryParse(target, out ulong ulongValue), (T)(object)ulongValue),
			_ when type == typeof(ushort) => (ushort.TryParse(target, out ushort ushortValue), (T)(object)ushortValue),
			_ when type.IsEnum => ParseEnum<T>(target),
			_ => (false, default!)
		};
	}

	private static (bool parsingResult, T parsedValue) ParseEnum<T>(string value)
	{
		var result = (false, default(T)!);

		try
		{
			Type enumType = typeof(T);
			string normalizedTarget = NormalizeStringForEnumParsing(value);

			T parsedEnum = (T)Enum.Parse(enumType, normalizedTarget, ignoreCase: true);

			result = (true, parsedEnum);
		}
		catch (Exception)
		{
		}
		
		return result;
	}
}
