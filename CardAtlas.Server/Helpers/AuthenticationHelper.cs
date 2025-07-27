using CardAtlas.Server.Models.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace CardAtlas.Server.Helpers;

public static class AuthenticationHelper
{
	/// <summary>
	/// Creates <see cref="SigningCredentials"/> from the jwt secret in <paramref name="appSettings"/>. 
	/// </summary>
	/// <returns>A new <see cref="SigningCredentials"/> with the jwt key encrypted.</returns>
	private static SigningCredentials GetSigningCredentials(AppSettings appSettings)
	{
		var key = new SymmetricSecurityKey(appSettings.JwtSettings.Key);

		return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
	}

	/// <summary>
	/// Generates a <see cref="JwtSecurityToken"/> from the <paramref name="userClaims"/> and <paramref name="appSettings"/>.
	/// </summary>
	/// <returns>A new <see cref="JwtSecurityToken"/> populated with the <paramref name="userClaims"/> and <paramref name="appSettings"/>.</returns>
	public static JwtSecurityToken GenerateSecurityToken(IEnumerable<Claim> userClaims, AppSettings appSettings)
	{
		return new JwtSecurityToken(
			issuer: appSettings.AppName,
			audience: appSettings.JwtSettings.Audience,
			claims: userClaims,
			expires: DateTime.Now.Add(appSettings.JwtSettings.TimeToLive),
			signingCredentials: GetSigningCredentials(appSettings)
		);
	}
}
