namespace CardAtlas.Server.Guards;

public static class StringGuards
{
	/// <summary>
	/// Evalueates wether the <paramref name="input"/> contains special characters.
	/// </summary>
	/// <returns>True if <paramref name="input"/> contains any special character; otherwise false.</returns>
	public static bool ContainsSpecialCharacter(string input) => !string.IsNullOrEmpty(input) && input.Any(c => !char.IsLetterOrDigit(c));

	/// <summary>
	/// Evalueates wether the <paramref name="input"/> contains uppercase characters.
	/// </summary>
	/// <returns>True if <paramref name="input"/> contains any uppercase character; otherwise false.</returns>
	public static bool ContainsUppercase(string input) => !string.IsNullOrWhiteSpace(input) && input.Any(char.IsUpper);

	/// <summary>
	/// Evalueates wether the <paramref name="input"/> contains lowercase characters.
	/// </summary>
	/// <returns>True if <paramref name="input"/> contains any lowercase character; otherwise false.</returns>
	public static bool ContainsLowercase(string input) => !string.IsNullOrWhiteSpace(input) && input.Any(char.IsLower);

	/// <summary>
	/// Evalueates wether the <paramref name="input"/> contains digit characters.
	/// </summary>
	/// <returns>True if <paramref name="input"/> contains any digit character; otherwise false.</returns>
	public static bool ContainsDigit(string input) => !string.IsNullOrWhiteSpace(input) && input.Any(char.IsDigit);
}
