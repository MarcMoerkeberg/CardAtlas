using CardAtlas.Server.Exceptions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using CardAtlas.Server.Resources.Errors;
using CardAtlas.Server.Services;
using CardAtlas.UnitTests.TestHelpers;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Net;
using System.Security.Claims;

namespace CardAtlas.UnitTests.ServiceTests;

public class AuthenticationServiceTests
{
	private Mock<IUserRepository> _userRepositoryMock;
	private Mock<UserManager<User>> _userManagerMock;
	private AppSettings _appSettings;
	private AuthenticationService _authenticationService;

	[SetUp]
	public void SetUp()
	{
		_userRepositoryMock = new Mock<IUserRepository>();
		_userManagerMock = MoqHelper.GetUserManagerMock();
		_appSettings = ConfigurationDataHelper.GetAppSettings();

		_authenticationService = new AuthenticationService(
			_appSettings,
			_userRepositoryMock.Object,
			_userManagerMock.Object
		);
	}

	[Test]
	public void CreateToken_NoUserExistsWithEmail_ShouldThrowNotFoundHttpException()
	{
		const string nonExistingEmail = "unknown@email.com";
		_userRepositoryMock
			.Setup(repo => repo.GetClaimsAsync(nonExistingEmail))
			.ReturnsAsync((List<Claim>?)null);

		HttpException exception = Assert.ThrowsAsync<HttpException>(async () =>
			await _authenticationService.CreateToken(nonExistingEmail)
		);

		Assert.That(
			exception.StatusCodeEnum,
			Is.EqualTo(HttpStatusCode.NotFound),
			"Thrown exception should have status code 404 (not found), when no user is found with the provided email."
		);
		Assert.That(
			exception.Message,
			Is.EqualTo(string.Format(Errors.UserNotFoundWithEmail, nonExistingEmail)),
			"Thrown exception should have a proper description and details of why the request failed."
		);
		Assert.That(
			exception.Title,
			Is.EqualTo(Errors.UserNotFound),
			"Thrown exception should have a proper describing title."
		);
	}

	[Test]
	public void CreateToken_UserHasNoRoleClaims_ShouldThrowForbiddenHttpException()
	{
		const string existingUserEmail = "knownUser@email.com";
		IReadOnlyList<Claim> claims = [
			new Claim(ClaimTypes.Email, existingUserEmail)
		];
		_userRepositoryMock
			.Setup(repo => repo.GetClaimsAsync(existingUserEmail))
			.ReturnsAsync(claims);

		HttpException exception = Assert.ThrowsAsync<HttpException>(async () =>
			await _authenticationService.CreateToken(existingUserEmail)
		);

		Assert.That(
			exception.StatusCodeEnum,
			Is.EqualTo(HttpStatusCode.Forbidden),
			"Thrown exception should have status code 404 (not found), when no user is found with the provided email."
		);
		Assert.That(
			exception.Message,
			Is.EqualTo(string.Format(Errors.UserHasNoRolesWithEmail, existingUserEmail)),
			"Thrown exception should have a proper description and details of why the request failed."
		);
		Assert.That(
			exception.Title,
			Is.EqualTo(Errors.NoRolesAssigned),
			"Thrown exception should have a proper describing title."
		);
	}


	[Test]
	public async Task CreateToken_ValidUserAndClaims_ShouldReturnJwtString()
	{
		const string existingUserEmail = "knownUser@email.com";
		IReadOnlyList<Claim> claims = [
			new Claim(ClaimTypes.Email, existingUserEmail),
			new Claim(ClaimTypes.Role, Roles.Admin)
		];
		_userRepositoryMock
			.Setup(repo => repo.GetClaimsAsync(existingUserEmail))
			.ReturnsAsync(claims);

		string jwtString = await _authenticationService.CreateToken(existingUserEmail);

		Assert.That(
			jwtString,
			Is.Not.Null.Or.Empty,
			"Should create a valid token when provided with valid user claims."
		);
		Assert.That(
			jwtString.Split('.').Length,
			Is.EqualTo(3),
			"A valid JWT should contain 3 segments."
		);
	}
}
