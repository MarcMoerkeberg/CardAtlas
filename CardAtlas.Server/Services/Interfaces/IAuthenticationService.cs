using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.DTOs.Request;

namespace CardAtlas.Server.Services.Interfaces;

public interface IAuthenticationService
{
	/// <summary>
	/// Verifies the credentials of the user.
	/// </summary>
	/// <returns>Returns true if a user with the specified credentials exist; otherwise false.</returns>
	Task<bool> VerifyUserCredentials(SignInDTO authenticateDTO);

	/// <summary>
	/// Creates a jwt from the <see cref="User"/> associated with the <paramref name="email"/>.
	/// </summary>
	/// <returns>A serialized jwt for the <see cref="User"/> associated with the <paramref name="email"/>.</returns>
	Task<string> CreateToken(string email);

}
