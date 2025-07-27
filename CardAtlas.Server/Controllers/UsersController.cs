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
public class UsersController : ControllerBase
{
	private readonly IUsersService _userService;

	public UsersController(IUsersService userService)
	{
		_userService = userService;
	}

	[Route("[action]")]
	[HttpPost]
	public async Task<ActionResult> SignUp(SignUpDTO signUpDTO)
	{
		if (!StringValidator.IsValidPassword(signUpDTO))
		{
			return BadRequest(ValidationErrors.InvalidPassword);
		}

		IdentityResult userCreationResult = await _userService.CreateUserAsync(signUpDTO);
		if (!userCreationResult.Succeeded)
		{
			return Conflict(userCreationResult.Errors);
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
