using CardAtlas.Server.Models.DTOs.Request;
using CardAtlas.Server.Validators;
using CardAtlas.UnitTests.DataHelpers;

namespace CardAtlas.UnitTests.ValidatorTests;

class StringValidatorTests
{
	[TestCase(null)]
	[TestCase("")]
	[TestCase("      ")]
	public void IsValidPassword_NullEmptyOrWhitespace_ShouldBeFalse(string? input)
	{
		bool isValidPassword = StringValidator.IsValidPassword(input);

		Assert.That(
			isValidPassword,
			Is.False,
			"Null, empty, or whitespace strings should not be valid passwords."
		);
	}

	[TestCase("ShortPa")] // 7 characters
	[TestCase("41CharactersIsTooLongToBeAValidPassword..")] // >40 chars
	public void IsValidPassword_InvalidLength_ShouldBeFalse(string input)
	{
		bool isValidPassword = StringValidator.IsValidPassword(input);

		Assert.That(
			isValidPassword,
			Is.False,
			"Passwords must be between 8 and 40 characters."
		);
	}

	[TestCase("ValidPassword1!")]
	[TestCase("AnotherValid$Passoword2")]
	[TestCase("Password123!")]
	public void IsValidPassword_AllRulesMet_ShouldBeTrue(string input)
	{
		bool isValidPassword = StringValidator.IsValidPassword(input);

		Assert.That(
			isValidPassword,
			Is.True,
			"Passwords meeting all rules should be valid."
		);
	}

	[TestCase("validpass1!")]  // No uppercase
	[TestCase("VALIDPASS1!")]  // No lowercase
	[TestCase("ValidPass!!!")] // No digit
	[TestCase("ValidPass123")] // No special char
	public void IsValidPassword_MissingRequiredCharacter_ShouldBeFalse(string input)
	{
		bool isValidPassword = StringValidator.IsValidPassword(input);

		Assert.That(
			isValidPassword,
			Is.False,
			"Passwords missing a required character should be invalid."
		);
	}

	[Test]
	public void IsValidPassword_FromSignUpDTOWithValidPassword_ShouldBeTrue()
	{
		SignUpDTO dto = DTODataHelper.GetSignUpDTO(password: "ValidPassword1!");

		bool isValidPassword = StringValidator.IsValidPassword(dto);

		Assert.That(
			isValidPassword,
			Is.True,
			"Should validate the Password property of SignUpDTO, which meets all password-criteria."
		);
	}

	[TestCase("")]
	[TestCase("      ")]
	[TestCase("ShortPa")] // 7 characters
	[TestCase("41CharactersIsTooLongToBeAValidPassword..")] // >40 chars
	[TestCase("validpass1!")]  // No uppercase
	[TestCase("VALIDPASS1!")]  // No lowercase
	[TestCase("ValidPass!!!")] // No digit
	[TestCase("ValidPass123")] // No special char
	[Test]
	public void IsValidPassword_FromSignUpDTOWithInvalidPassword_ShouldBeFalse(string password)
	{
		SignUpDTO dto = DTODataHelper.GetSignUpDTO(password: password);

		bool isValidPassword = StringValidator.IsValidPassword(dto);

		Assert.That(
			isValidPassword,
			Is.False,
			"Should validate the Password property of SignUpDTO, which meets all password-criteria."
		);
	}
}
