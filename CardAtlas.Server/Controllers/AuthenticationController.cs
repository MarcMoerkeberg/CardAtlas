using Asp.Versioning;
using CardAtlas.Server.Models.DTOs.Request;
using CardAtlas.Server.Resources.Errors;
using CardAtlas.Server.Services.Interfaces;
using CardAtlas.Server.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CardAtlas.Server.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/[controller]/[action]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService _authenticationService;

		public AuthenticationController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		[HttpPost]
		public string SignIn([FromBody] string email, string password)
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		public async Task<ActionResult> SignUp(SignUpDTO signUpDTO)
		{
			if (!StringValidator.IsValidPassword(signUpDTO))
			{
				return BadRequest(ValidationErrors.InvalidPassword);
			}

			IdentityResult userCreationResult = await _authenticationService.CreateUserAsync(signUpDTO);
			if (!userCreationResult.Succeeded)
			{
				return Conflict(userCreationResult.Errors);
			}

			return Created();
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
