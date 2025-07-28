using CardAtlas.Server.Guards;

namespace CardAtlas.UnitTests.GuardTests;

class StringGuardsTests
{
	[TestCase(" ")]
	[TestCase("!")]
	[TestCase("!?@#")]
	[TestCase("helloWorld!")]
	[Test]
	public void ContainsSpecialCharacter_WithSpecialCharacters_ShouldBeTrue(string input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsSpecialCharacter(input);

		Assert.That(
			containsSpecialCharacters,
			Is.True,
			"Should be true, when containing one or more special characters. Whitepsace is considered a special character."
		);
	}

	[TestCase("HelloWorld")]
	[TestCase("HelloWorld123")]
	[TestCase("123")]
	[Test]
	public void ContainsSpecialCharacter_NoSpecialCharacters_ShouldBeFalse(string input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsSpecialCharacter(input);

		Assert.That(
			containsSpecialCharacters,
			Is.False,
			"Should be false, when containing no special characters.");
	}

	[TestCase(null)]
	[TestCase("")]
	public void ContainsSpecialCharacter_NullOrEmpty_ShouldBeFalse(string? input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsSpecialCharacter(input);

		Assert.That(
			containsSpecialCharacters,
			Is.False,
			"Should be false, when input is null or empty."
		);
	}

	[TestCase("Helloworld")]
	[TestCase("HelloWorld")]
	[TestCase("helloWorld")]
	[TestCase("HELLOWORLD")]
	[TestCase("H")]
	[Test]
	public void ContainsUppercase_WithUppercaseCharacters_ShouldBeTrue(string input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsUppercase(input);

		Assert.That(
			containsSpecialCharacters,
			Is.True,
			"Should be true, when input contains one or more uppercase characters."
		);
	}

	[TestCase("helloworld")]
	[TestCase("h")]
	[Test]
	public void ContainsUppercase_NoUppercaseCharacters_ShouldBeFalse(string input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsUppercase(input);

		Assert.That(
			containsSpecialCharacters,
			Is.False,
			"Should be false, when input contains no uppercase characters."
		);
	}

	[TestCase(null)]
	[TestCase("")]
	[TestCase(" ")]
	public void ContainsUppercase_NullEmptyOrWhitespace_ShouldBeFalse(string? input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsUppercase(input);

		Assert.That(
			containsSpecialCharacters,
			Is.False,
			"Should be false, when input is null, empty or only whitespace characters."
		);
	}

	[TestCase("HELLOWORLd")]
	[TestCase("helloworld")]
	[TestCase("helloworlD")]
	[TestCase("helloWorld")]
	[TestCase("HelloWorld")]
	[TestCase("h")]
	[Test]
	public void ContainsLowercase_WithLowercaseCharacters_ShouldBeTrue(string input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsLowercase(input);

		Assert.That(
			containsSpecialCharacters,
			Is.True,
			"Should be true, when input contains one or more uppercase characters."
		);
	}

	[TestCase("HELLOWORLD")]
	[TestCase("H")]
	[Test]
	public void ContainsLowercase_NoLowercaseCharacters_ShouldBeFalse(string input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsLowercase(input);

		Assert.That(
			containsSpecialCharacters,
			Is.False,
			"Should be false, when input contains no uppercase characters."
		);
	}

	[TestCase(null)]
	[TestCase("")]
	[TestCase(" ")]
	public void ContainsLowercase_NullEmptyOrWhitespace_ShouldBeFalse(string? input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsLowercase(input);

		Assert.That(
			containsSpecialCharacters,
			Is.False,
			"Should be false, when input is null, empty or only whitespace characters."
		);
	}

	[TestCase("1")]
	[TestCase("123")]
	[TestCase("HelloWorld1")]
	[TestCase("1HelloWorld")]
	[TestCase("Hello1World")]
	[Test]
	public void ContainsDigit_WithLowercaseCharacters_ShouldBeTrue(string input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsDigit(input);

		Assert.That(
			containsSpecialCharacters,
			Is.True,
			"Should be true, when input contains one or more uppercase characters."
		);
	}

	[TestCase("HelloWorld")]
	[TestCase("H")]
	[Test]
	public void ContainsDigit_NoLowercaseCharacters_ShouldBeFalse(string input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsDigit(input);

		Assert.That(
			containsSpecialCharacters,
			Is.False,
			"Should be false, when input contains no uppercase characters."
		);
	}

	[TestCase(null)]
	[TestCase("")]
	[TestCase(" ")]
	public void ContainsDigit_NullEmptyOrWhitespace_ShouldBeFalse(string? input)
	{
		bool containsSpecialCharacters = StringGuards.ContainsDigit(input);

		Assert.That(
			containsSpecialCharacters,
			Is.False,
			"Should be false, when input is null, empty or only whitespace characters."
		);
	}
}
