using CardAtlas.Server.Models.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CardAtlas.Server.Repositories.Interfaces;

public interface IUserRepository
{
	/// <summary>
	/// Creates the specified user.
	/// </summary>
	/// <returns>An <see cref="IdentityResult"/> indicating wether user was sucessfully created.</returns>
	Task<IdentityResult> CreateAsync(User userToCreate, string password, IEnumerable<string> roles);

	/// <summary>
	/// Returns all claims from the <see cref="User"/> with the associated <paramref name="email"/>.
	/// </summary>
	/// <returns>A list of <see cref="Claim"/> for the user associated with the <paramref name="email"/>; or null if no user is found.</returns>
	Task<List<Claim>?> GetClaimsAsync(string email);

	/// <summary>
	/// Returns all claims associated with the <paramref name="user"/>.
	/// </summary>
	/// <returns>A list of <see cref="Claim"/> for the specified <paramref name="user"/>.</returns>
	Task<List<Claim>> GetClaimsAsync(User user);
}
