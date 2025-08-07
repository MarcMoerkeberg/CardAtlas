using CardAtlas.Server.DAL;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Entities;
using CardAtlas.Server.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;
using System.Threading.Channels;

namespace CardAtlas.Server.Repositories;

public class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly Channel<OutboxMessage> _outboxChannel;

	public UserRepository(
		ApplicationDbContext dbContext,
		Channel<OutboxMessage> outboxChannel,
		UserManager<User> userManager)
	{
		_dbContext = dbContext;
		_outboxChannel = outboxChannel;
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

			//TODO: Add logging
			return rolesResult;
		}

		OutboxMessage outboxMessage = OutboxMapper.ToEmailMessage(userToCreate.Email!);
		_dbContext.OutboxMessages.Add(outboxMessage);

		try
		{
			await _dbContext.SaveChangesAsync();
			await transaction.CommitAsync();
			await _outboxChannel.Writer.WriteAsync(outboxMessage);

			return IdentityResult.Success;

		}
		catch (Exception)
		{
			await transaction.RollbackAsync();

			//TODO: Add logging and proper error message
			return IdentityResult.Failed(new IdentityError { Code = "", Description = "" });
		}
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
