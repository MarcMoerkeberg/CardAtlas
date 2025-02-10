using System.Collections.Concurrent;
using System.Reflection;

namespace ScryfallApi;

public static class ScryfallHelper
{
	/// <summary>
	/// Parses the <paramref name="targets"/> into keys of the provided <typeparamref name="TEnum"/>.<br/>
	/// Normalizes the string before parsing it as the enum key.
	/// </summary>
	/// <param name="targets">Should exist as en entry within <typeparamref name="TEnum"/>.</param>
	public static IEnumerable<TEnum> ParseAsScryfallEnum<TEnum>(this IEnumerable<string> targets) where TEnum : struct, Enum
	{
		return targets.Select(target => target.ParseAsScryfallEnum<TEnum>());
	}

	/// <summary>
	/// Parses the <paramref name="target"/> into a key of the provided <typeparamref name="TEnum"/>.<br/>
	/// Normalizes the string before parsing it as the enum.
	/// </summary>
	/// <param name="target">Should exist as en entry within <typeparamref name="TEnum"/>.</param>
	public static TEnum ParseAsScryfallEnum<TEnum>(this string target) where TEnum : struct, Enum
	{
		string normalizedTarget = NormalizeStringForEnumParsing(target);
		return Enum.TryParse(normalizedTarget, ignoreCase: true, out TEnum parsedResult)
			? parsedResult
			: default;
	}

	/// <summary>
	/// Returns new normalized string for parsing enum key.
	/// </summary>
	private static string NormalizeStringForEnumParsing(string target)
	{
		return target.Replace("_", string.Empty);
	}

	/// <summary>
	/// Creates a dictionary with keys from <paramref name="targetDictionary"/> that matches an entry within <typeparamref name="TEnum"/>.
	/// </summary>
	/// <typeparam name="TEnum">The enum to match keys from <paramref name="targetDictionary"/>.</typeparam>
	/// <typeparam name="TValue">The <see cref="Type"/> to parse <paramref name="targetDictionary"/> values to.</typeparam>
	/// <param name="targetDictionary"></param>
	/// <returns></returns>
	public static Dictionary<TEnum, TValue> ToEnumKeyDictionary<TEnum, TValue>(this Dictionary<string, string>? targetDictionary) where TEnum : struct, Enum
	{
		var result = new Dictionary<TEnum, TValue>();
		if (targetDictionary is null)
		{
			return result;
		}

		foreach (var (key, value) in targetDictionary)
		{
			(bool parseKeySuccess, TEnum parsedKey) = ParseEnum<TEnum>(key);
			(bool parseValueSuccess, TValue? parsedValue) = ParseAs<TValue>(value);
			//TODO: add logging for failing parsing.

			if (parseKeySuccess && parseValueSuccess && !result.ContainsKey(parsedKey))
			{
				result.Add(parsedKey, parsedValue!);
			}
		}

		return result;
	}

	/// <summary>
	/// Parses <paramref name="target"/> to type <typeparamref name="T"/>.<br/>
	/// WARNING: Does not parse all types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="target"></param>
	/// <returns>Tuple with flag indicating wether the parsing was successfull or not, and the parsed value.</returns>
	private static (bool parsingSuccess, T parsedValue) ParseAs<T>(string target)
	{
		(bool, T) errorResult = (false, default!);
		Type type = typeof(T);

		if (target is null) return errorResult;

		return type switch
		{
			_ when type == typeof(string) =>
			(true, (T)(object)target),

			_ when type == typeof(bool) =>
			(bool.TryParse(target, out bool boolValue), (T)(object)boolValue),

			_ when type == typeof(byte) =>
			(byte.TryParse(target, out byte byteValue), (T)(object)byteValue),

			_ when type == typeof(char) =>
			(char.TryParse(target, out char charValue), (T)(object)charValue),

			_ when type == typeof(decimal) =>
			(decimal.TryParse(target, out decimal decimalValue), (T)(object)decimalValue),

			_ when type == typeof(double) =>
			(double.TryParse(target, out double doubleValue), (T)(object)doubleValue),

			_ when type == typeof(float) =>
			(float.TryParse(target, out float floatValue), (T)(object)floatValue),

			_ when type == typeof(int) =>
			(int.TryParse(target, out int intValue), (T)(object)intValue),

			_ when type == typeof(long) =>
			(long.TryParse(target, out long longValue), (T)(object)longValue),

			_ when type == typeof(sbyte) =>
			(sbyte.TryParse(target, out sbyte sbyteValue), (T)(object)sbyteValue),

			_ when type == typeof(short) =>
			(short.TryParse(target, out short shortValue), (T)(object)shortValue),

			_ when type == typeof(uint) =>
			(uint.TryParse(target, out uint uintValue), (T)(object)uintValue),

			_ when type == typeof(ulong) =>
			(ulong.TryParse(target, out ulong ulongValue), (T)(object)ulongValue),

			_ when type == typeof(ushort) =>
			(ushort.TryParse(target, out ushort ushortValue), (T)(object)ushortValue),

			_ when type.IsEnum =>
			ParseEnum<T>(target),

			_ when type == typeof(Uri) =>
			ParseUri<T>(target),

			_ => errorResult
		};
	}

	/// <summary>
	/// Parses <paramref name="value"/> to enum of type <typeparamref name="TEnum"/>.<br/>
	/// Fails if <typeparamref name="TEnum"/> is not an enum or <paramref name="value"/> is not a key within the enum.<br/>
	/// </summary>
	/// <typeparam name="TEnum">Should be a scryfall Enum type.</typeparam>
	/// <param name="value">Should be a key within the enum.</param>
	/// <returns>Tuple containing flag wether the parsing was successfull or not, and the parsed value.</returns>
	private static (bool parsingResult, TEnum parsedValue) ParseEnum<TEnum>(string value)
	{
		var errorResult = (false, default(TEnum)!);

		if (string.IsNullOrEmpty(value)) return errorResult;
		if (!typeof(TEnum).IsEnum) return errorResult;

		try
		{
			Type enumType = typeof(TEnum);
			string normalizedTarget = NormalizeStringForEnumParsing(value);

			TEnum parsedEnum = (TEnum)Enum.Parse(enumType, normalizedTarget, ignoreCase: true);

			return (true, parsedEnum);
		}
		catch
		{
			return errorResult;
		}
	}

	/// <summary>
	/// Returns a new <see cref="Uri"/> with <paramref name="value"/> as the absolute url. <br/>
	/// Fails if <paramref name="value"/> is not well formatted for uri consumption.
	/// </summary>
	/// <param name="value">Should be an absolute string uri.</param>
	private static (bool parsingResult, T parsedValue) ParseUri<T>(string value)
	{
		var errorResult = (false, default(T)!);

		if (string.IsNullOrEmpty(value)) return errorResult;
		if (!Uri.IsWellFormedUriString(value, UriKind.Absolute)) return errorResult;

		return (Uri.TryCreate(value, UriKind.Absolute, out Uri? uriResult), (T)(object)uriResult!);
	}

	/// <summary>
	/// Caches the properties for each type, using a case-insensitive dictionary keyed by property name.
	/// </summary>
	private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> _typeProperties =
		new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

	/// <summary>
	/// Gets a dictionary of public instance properties for <paramref name="modelType"/>.
	/// </summary>
	/// <param name="modelType">The type of the model to retrieve properties for.</param>
	/// <returns>A dictionary with <see cref="PropertyInfo"/> as values and property names as keys for <paramref name="modelType"/>.</returns>
	private static Dictionary<string, PropertyInfo> GetModelProperties(Type modelType)
	{
		Dictionary<string, PropertyInfo> properties = _typeProperties.GetOrAdd(modelType, type =>
			type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.ToDictionary(property => property.Name, StringComparer.OrdinalIgnoreCase));

		return properties;
	}

	/// <summary>
	/// Returns a new <typeparamref name="TModel"/> with properties populated by <paramref name="parserFunction"/> if properties on the model matches the json string.<br/>
	/// </summary>
	/// <typeparam name="TModel">The type of the model to create and bind values to.</typeparam>
	/// <param name="jsonStrings">A collection of JSON string keys corresponding to property names.</param>
	/// <param name="parserFunction">If a matching property from <typeparamref name="TModel"/> is found, it uses this function to populate the propety.</param>
	/// <returns>An instance of <typeparamref name="TModel"/> with properties populated by <paramref name="parserFunction"/> where <paramref name="jsonStrings"/> matches property names.</returns>
	public static TModel BindJsonStringsToModel<TModel>(IEnumerable<string> jsonStrings, Func<string, object> parserFunction) where TModel : class, new()
	{
		var model = new TModel();
		Dictionary<string, PropertyInfo> properties = GetModelProperties(typeof(TModel));

		foreach (string jsonString in jsonStrings)
		{
			if (string.IsNullOrEmpty(jsonString))
				continue;

			if (properties.TryGetValue(jsonString, out PropertyInfo? property) && property is not null && property.CanWrite)
			{
				object propValue = parserFunction(jsonString);
				property.SetValue(model, propValue);
			}
		}

		return model;
	}

	/// <summary>
	/// Creates a new instance of <typeparamref name="TModel"/> with populated properties from the keys and values from <paramref name="dictionary"/> and the <paramref name="parserFunction"/>.
	/// </summary>
	/// <typeparam name="TModel">The type of the model to create and bind values to.</typeparam>
	/// <typeparam name="TTarget">The <see cref="Type"/> you want to parse values in <paramref name="dictionary"/> to.</typeparam>
	/// <param name="dictionary">A dictionary with keys corresponding to property names and values of <typeparamref name="TModel"/>.<br/>
	/// Values will be parsed with <paramref name="parserFunction"/>.
	/// </param>
	/// <param name="parserFunction">A function that parses a dictionary value of type <typeparamref name="TTarget"/> into an object for assignment.</param>
	/// <returns>An instance of <typeparamref name="TModel"/> with properties populated by <paramref name="parserFunction"/> where <paramref name="dictionary"/> keys matches property names.</returns>
	public static TModel BindDictionaryToModel<TModel, TTarget>(Dictionary<string, TTarget> dictionary, Func<TTarget, object?> parserFunction) where TModel : class, new()
	{
		var model = new TModel();
		Dictionary<string, PropertyInfo> properties = GetModelProperties(typeof(TModel));

		foreach (var keyValuePair in dictionary)
		{
			if (properties.TryGetValue(keyValuePair.Key, out PropertyInfo? property) && property is not null && property.CanWrite)
			{
				object? propValue = parserFunction(keyValuePair.Value);
				property.SetValue(model, propValue);
			}
		}

		return model;
	}
}
