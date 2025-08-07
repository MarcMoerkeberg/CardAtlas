using Asp.Versioning;
using CardAtlas.Server.Models.DTOs.Request;
using CardAtlas.Server.Resources.Errors;
using CardAtlas.Server.Services.Interfaces;
using CardAtlas.Server.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CardAtlas.Server.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class UsersController : ApiControllerBase
{
	private readonly IUsersService _userService;

	public UsersController(IUsersService userService)
	{
		_userService = userService;
	}

	[Route(nameof(SignUp))]
	[HttpPost]
	public async Task<IActionResult> SignUp(SignUpDTO signUpDTO)
	{
		if (!StringValidator.IsValidPassword(signUpDTO))
		{
			//TODO: Add logging
			return BadRequestProblem(ValidationErrors.InvalidPassword);
		}

		IdentityResult userCreationResult = await _userService.CreateUserAsync(signUpDTO);
		if (!userCreationResult.Succeeded)
		{
			//TODO: Add logging
			string errorDetails = string.Join(Environment.NewLine, userCreationResult.Errors);
			return ConflictProblem(errorDetails);
		}

		return Created();
	}


	[Route("[action]")]
	[HttpPost]
	public string ResetPassword([FromBody] string email)
	{
		throw new NotImplementedException();
	}
}
