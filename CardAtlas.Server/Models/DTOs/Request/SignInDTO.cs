using System.ComponentModel.DataAnnotations;

namespace CardAtlas.Server.Models.DTOs.Request;

public record SignInDTO
{
	[Required]
	[EmailAddress]
	public required string Email { get; init; }

	[MinLength(1)]
	[MaxLength(40)]
	public required string Password { get; init; }
}
