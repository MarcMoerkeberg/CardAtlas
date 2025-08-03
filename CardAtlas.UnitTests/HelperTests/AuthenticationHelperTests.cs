using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Internal;
using CardAtlas.UnitTests.TestHelpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CardAtlas.UnitTests.HelperTests;

public class AuthenticationHelperTests
{
	[Test]
	[TestCase("CardAtlas api")]
	public void GenerateSecurityToken_SetIssuerFromAppSettings_ShouldSetAppNameAsIssuer(string issuer)
	{
		AppSettings appSettings = ConfigurationDataHelper.GetAppSettings(appName: issuer);
		Claim[] claims = [new Claim(ClaimTypes.NameIdentifier, "SomeUserName")];
		JwtSecurityToken token = AuthenticationHelper.GenerateSecurityToken(claims, appSettings);

		Assert.That(
			token,
			Is.Not.Null,
			"Expected a valid token to be created from the provided appsettings and claims."
		);
		Assert.That(
			token.Issuer,
			Is.EqualTo(appSettings.AppName),
			"The token issuer should be set from the appname of the appsettings."
		);
	}

	[Test]
	[TestCase("some-audience")]
	public void GenerateSecurityToken_InitializeAudienceWithJwtSettings_ShouldAddAudience(string audience)
	{
		AppSettings appSettings = ConfigurationDataHelper.GetAppSettings(audience: audience);
		Claim[] claims = [new Claim(ClaimTypes.NameIdentifier, "SomeUserName")];
		JwtSecurityToken token = AuthenticationHelper.GenerateSecurityToken(claims, appSettings);

		Assert.That(
			token,
			Is.Not.Null,
			"Expected a valid token to be created from the provided appsettings and claims."
		);
		Assert.That(
			token.Audiences,
			Does.Contain(appSettings.JwtSettings.Audience),
			"The token audience should contain the audience from the jwt settings in appsettings."
		);
	}

	[Test]
	[TestCase(30)]
	public void GenerateSecurityToken_SetExpirationFromJwtSettings_ShouldSetExpirationTimeFromTimeToLive(int expiration)
	{
		AppSettings appSettings = ConfigurationDataHelper.GetAppSettings(timeToLiveInMinutes: expiration);
		Claim[] claims = [new Claim(ClaimTypes.NameIdentifier, "SomeUserName")];
		JwtSecurityToken token = AuthenticationHelper.GenerateSecurityToken(claims, appSettings);

		Assert.That(
			token,
			Is.Not.Null,
			"Expected a valid token to be created from the provided appsettings and claims."
		);
		Assert.That(
			token.ValidTo - token.ValidFrom,
			Is.EqualTo(appSettings.JwtSettings.TimeToLive),
			"The token expiration should be the same as time to live in the jwt settings in appsettings."
		);
	}

	[Test]
	[TestCase("SomeUserName", "test@email.com")]
	public void GenerateSecurityToken_WithValidClaims_ShouldContainProvidedClaims(string nameIdentifier, string email)
	{
		AppSettings appSettings = ConfigurationDataHelper.GetAppSettings();
		Claim nameClaim = new Claim(ClaimTypes.NameIdentifier, nameIdentifier);
		Claim emailClaim = new Claim(ClaimTypes.Email, email);

		JwtSecurityToken token = AuthenticationHelper.GenerateSecurityToken([nameClaim, emailClaim], appSettings);

		Assert.That(
			token,
			Is.Not.Null,
			"Expected a valid token to be created from the provided appsettings and claims."
		);
		Assert.That(
			token.Claims,
			Has.One.Matches<Claim>(claim =>
				claim.Type == nameClaim.Type &&
				claim.Value == nameClaim.Value
			),
			"The claims on the token should contain one claim for each provided, when generating a new token."
		);
		Assert.That(
			token.Claims,
			Has.One.Matches<Claim>(claim =>
				claim.Type == emailClaim.Type &&
				claim.Value == emailClaim.Value
			),
			"The claims on the token should contain one claim for each provided, when generating a new token."
		);
	}

	public void GenerateSecurityToken_WithInvalidClaims_ShouldThrowArgumentException(string nameIdentifier, string email)
	{
		AppSettings appSettings = ConfigurationDataHelper.GetAppSettings();

		Func<JwtSecurityToken> generateTokenMethod = () => AuthenticationHelper.GenerateSecurityToken([], appSettings);

		Assert.That(
			generateTokenMethod,
			Throws.TypeOf<ArgumentOutOfRangeException>(),
			"Should throw an ArgumentOutOfRangeException, when generating a token with no claims."
		);
	}
}
