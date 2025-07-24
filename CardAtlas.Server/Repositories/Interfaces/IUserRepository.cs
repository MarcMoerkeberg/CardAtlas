using CardAtlas.Server.Models.Data;
using Microsoft.AspNetCore.Identity;

namespace CardAtlas.Server.Repositories.Interfaces;

public interface IUserRepository
{
	Task<IdentityResult> CreateAsync(User userToCreate, string password, IEnumerable<string> roles);
}
