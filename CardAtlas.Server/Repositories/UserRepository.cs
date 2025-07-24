using CardAtlas.Server.DAL;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

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
}
