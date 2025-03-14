using CardAtlas.Server.Extensions;

namespace CardAtlas.UnitTests.ExtensionTests;

class StringExtensionsTests
{
	[Test]
	[TestCase(null)]
	[TestCase("")]
	[TestCase(" ")]
	public void CapitalizeFirstLetter_ExpectsEmptyString_WhenTargetIsNullEmptyOrWhitespace(string? target)
	{
		string result = target.CapitalizeFirstLetter();
		
		Assert.That(result, Is.EqualTo(string.Empty));
	}
	
	[Test]
	public void CapitalizeFirstLetter_ExpectsCapitalizeFirstLetter_WhenTargetIsLowercase()
	{
		string target = "hello world";

		string result = target.CapitalizeFirstLetter();
		
		Assert.That(result, Is.EqualTo("Hello world"));
	}
	
	[Test]
	[TestCase("h")]
	[TestCase("H")]
	public void CapitalizeFirstLetter_ExpectsCapitalize_WhenTargetIsSingleChar(string target)
	{
		string result = target.CapitalizeFirstLetter();
		
		Assert.That(result, Is.EqualTo("H"));
	}
	
	[Test]
	[TestCase("1")]
	[TestCase("1hello world")]
	[TestCase("!hello world")]
	[TestCase("?? hello world")]
	[TestCase("@hello world")]
	[TestCase("|| hello world")]
	public void CapitalizeFirstLetter_ExpectsNoChange_WhenTargetFirstCharIsNonLetter(string target)
	{
		string result = target.CapitalizeFirstLetter();
		
		Assert.That(result, Is.EqualTo(target));
	}
}
