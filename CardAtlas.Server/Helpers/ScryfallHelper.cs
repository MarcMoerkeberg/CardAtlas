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
			_ when type == typeof(bool) => bool.TryParse(target, out bool boolValue) 
				? (true, (T)(object)boolValue) 
				: errorResult,

			_ when type == typeof(byte) => byte.TryParse(target, out byte byteValue) 
				? (true, (T)(object)byteValue) 
				: errorResult,

			_ when type == typeof(char) => char.TryParse(target, out char charValue) 
				? (true, (T)(object)charValue) 
				: errorResult,
			
			_ when type == typeof(decimal) => decimal.TryParse(target, out decimal decimalValue) 
				? (true, (T)(object)decimalValue) 
				: errorResult,

			_ when type == typeof(double) => double.TryParse(target, out double doubleValue) 
				? (true, (T)(object)doubleValue) 
				: errorResult,

			_ when type == typeof(float) => float.TryParse(target, out float floatValue) 
				? (true, (T)(object)floatValue) 
				: errorResult,

			_ when type == typeof(int) => (int.TryParse(target, out int intValue), (T)(object)intValue),
				//? (true, (T)(object)intValue) 
				//: errorResult,

			_ when type == typeof(long) => long.TryParse(target, out long longValue) 
				? (true, (T)(object)longValue) 
				: errorResult,

			_ when type == typeof(sbyte) => sbyte.TryParse(target, out sbyte sbyteValue) 
				? (true, (T)(object)sbyteValue) 
				: errorResult,

			_ when type == typeof(short) => short.TryParse(target, out short shortValue) 
				? (true, (T)(object)shortValue) 
				: errorResult,

			_ when type == typeof(uint) => uint.TryParse(target, out uint uintValue) 
				? (true, (T)(object)uintValue) 
				: errorResult,

			_ when type == typeof(ulong) => ulong.TryParse(target, out ulong ulongValue) 
				? (true, (T)(object)ulongValue) 
				: errorResult,

			_ when type == typeof(ushort) => ushort.TryParse(target, out ushort ushortValue) 
				? (true, (T)(object)ushortValue) 
				: errorResult,

			_ when type.IsEnum => (true, ParseEnum<T>(target)),
			_ => (false, default!)
		};
	}

	private static T ParseEnum<T>(string value)
	{
		Type enumType = typeof(T);
		var test = Nullable.GetUnderlyingType(enumType);

		return value.ParseAsScryfallEnum<>();
	}
}
