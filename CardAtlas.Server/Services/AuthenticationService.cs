using CardAtlas.Server.Exceptions;
using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.DTOs.Request;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using CardAtlas.Server.Resources.Errors;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CardAtlas.Server.Services;

public class AuthenticationService : IAuthenticationService
{
	private readonly AppSettings _appSettings;
	private readonly IUserRepository _userRepository;
	private readonly UserManager<User> _userManager;

	public AuthenticationService(
		AppSettings appSettings,
		IUserRepository userRepository,
		UserManager<User> userManager)
	{
		_appSettings = appSettings;
		_userRepository = userRepository;
		_userManager = userManager;
	}

	public async Task<bool> VerifyUserCredentials(SignInDTO authenticateDTO)
	{
		User? user = await _userManager.FindByEmailAsync(authenticateDTO.Email);

		return user is not null && await _userManager.CheckPasswordAsync(user, authenticateDTO.Password);
	}

	public async Task<string> CreateToken(string email)
	{
		IReadOnlyList<Claim>? claims = await _userRepository.GetClaimsAsync(email);
		if (claims is null)
		{
			throw new HttpException(
				statusCode: HttpStatusCode.NotFound,
				message: string.Format(Errors.UserNotFoundWithEmail, email),
				title: Errors.UserNotFound
			);
		}
		else if (!claims.Any(claim => claim.Type == ClaimTypes.Role))
		{
			throw new HttpException(
				statusCode: HttpStatusCode.Forbidden,
				message: string.Format(Errors.UserHasNoRolesWithEmail, email),
				title: Errors.NoRolesAssigned
			);
		}

		JwtSecurityTokenHandler tokenHandler = new();
		JwtSecurityToken token = AuthenticationHelper.GenerateSecurityToken(claims, _appSettings);

		return tokenHandler.WriteToken(token);
	}
}
