using Asp.Versioning;
using CardAtlas.Server.Models.DTOs.Request;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CardAtlas.Server.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/[controller]/[action]")]
	public class AuthenticationController
	{
		private readonly IAuthenticationService _authenticationService;

		AuthenticationController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		[HttpPost]
		public string SignIn([FromBody] string email, string password)
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		public async Task<string> SignUp(SignUpDTO signUpDTO)
		{
			await _authenticationService.CreateUserAsync(signUpDTO);

			throw new NotImplementedException();
		}

		[HttpPost]
		public string SignOut()
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		public string ResetPassword([FromBody] string email)
		{
			throw new NotImplementedException();
		}
	}
}
