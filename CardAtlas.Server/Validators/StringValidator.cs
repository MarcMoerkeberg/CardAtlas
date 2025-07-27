using CardAtlas.Server.Guards;
using CardAtlas.Server.Models.DTOs.Request;

namespace CardAtlas.Server.Validators;

public static class StringValidator//TODO: Add tests
{
	/// <summary>
	/// Validates wether the password in the <paramref name="signUpDTO"/> conforms to the businessrules of a valid password.
	/// </summary>
	/// <returns>True if the password passes as valid; otherwise false.</returns>
	public static bool IsValidPassword(SignUpDTO signUpDTO) => IsValidPassword(signUpDTO.Password);

	/// <summary>
	/// Validates wether the <paramref name="input"/> conforms to the businessrules for a valid password.
	/// </summary>
	/// <returns>True if the <paramref name="input"/> is a valid password; otherwise false.</returns>
	public static bool IsValidPassword(string input) =>
		!string.IsNullOrWhiteSpace(input) &&
		input is { Length: >= 8 and <= 40 } &&
		StringGuards.ContainsDigit(input) &&
		StringGuards.ContainsUppercase(input) &&
		StringGuards.ContainsLowercase(input) &&
		StringGuards.ContainsSpecialCharacter(input);

}
