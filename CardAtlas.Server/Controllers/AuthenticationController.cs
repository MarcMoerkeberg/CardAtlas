using Asp.Versioning;
using CardAtlas.Server.Models.DTOs.Request;
using CardAtlas.Server.Resources.Errors;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CardAtlas.Server.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class AuthenticationController : ApiControllerBase
{
	private readonly IAuthenticationService _authenticationService;

	public AuthenticationController(IAuthenticationService authenticationService)
	{
		_authenticationService = authenticationService;
	}

	[Route("SignIn")]
	[HttpPost]
	public async Task<IActionResult> Authorize([FromBody] SignInDTO signInDTO)
	{
		bool validSigninCredentials = await _authenticationService.VerifyUserCredentials(signInDTO);
		if (!validSigninCredentials)
		{
			//TODO: Add logging
			return UnauthorizedProblem(ValidationErrors.InvalidSignInCredentials);
		}

		string jwtToken = await _authenticationService.CreateToken(signInDTO.Email);

		return Ok(jwtToken);
	}

	[HttpPost("SignOut")]
	public string Deauthorize()
	{
		throw new NotImplementedException();
	}
}
