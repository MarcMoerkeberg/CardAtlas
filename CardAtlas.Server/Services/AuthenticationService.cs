using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.DTOs.Request;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CardAtlas.Server.Services;

public class AuthenticationService : IAuthenticationService
{
	private readonly IUserRepository _userRepository;

	public AuthenticationService(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<IdentityResult> CreateUserAsync(SignUpDTO signUpDTO)
	{
		User newUser = UserMapper.FromDTO(signUpDTO);

		IdentityResult createUserResult = await _userRepository.CreateAsync(
			userToCreate: newUser,
			password: signUpDTO.Password,
			roles: Roles.DefaultRoles
		);

		return createUserResult;
	}
}
