using CardAtlas.Server.DAL;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;

namespace CardAtlas.Server.Repositories;

public class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	public UserRepository(
		ApplicationDbContext dbContext,
		UserManager<User> userManager)
	{
		_dbContext = dbContext;
		_userManager = userManager;
	}

	public async Task<IdentityResult> CreateAsync(User userToCreate, string password, IEnumerable<string> roles)
	{
		using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

		IdentityResult addUserResult = await _userManager.CreateAsync(userToCreate, password);
		if (!addUserResult.Succeeded)
		{
			await transaction.RollbackAsync();

			//TODO: Add logging
			return addUserResult;
		}

		IdentityResult rolesResult = await _userManager.AddToRolesAsync(userToCreate, roles);
		if (!rolesResult.Succeeded)
		{
			await transaction.RollbackAsync();
			await _userManager.DeleteAsync(userToCreate);

			//TODO: Add logging
			return rolesResult;
		}

		await transaction.CommitAsync();
		return IdentityResult.Success;
	}

	public async Task<IReadOnlyList<Claim>?> GetClaimsAsync(string userEmail)
	{
		User? user = await _userManager.FindByEmailAsync(userEmail);

		return user is null
			? null
			: await GetClaimsAsync(user);
	}

	public async Task<IReadOnlyList<Claim>> GetClaimsAsync(User user)
	{
		List<Claim> claims = new()
		{
			new Claim(ClaimTypes.Email, user.Email!),
			new Claim(ClaimTypes.Name, user.DisplayName)
		};

		IList<string> roles = await _userManager.GetRolesAsync(user);
		foreach (string role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		return claims;
	}
}
