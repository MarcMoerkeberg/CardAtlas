using CardAtlas.Server.Models.DTOs.Request;
using Microsoft.AspNetCore.Identity;

namespace CardAtlas.Server.Services.Interfaces;

public interface IUsersService
{
	Task<IdentityResult> CreateUserAsync(SignUpDTO signUpDTO);
}
