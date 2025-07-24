using CardAtlas.Server.Models.DTOs.Request;

namespace CardAtlas.Server.Services.Interfaces;

public interface IAuthenticationService
{
	Task CreateUserAsync(SignUpDTO signUpDTO);
}
