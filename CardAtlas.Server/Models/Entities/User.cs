using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CardAtlas.Server.Models.Data;

public class User : IdentityUser
{
	[MinLength(1)]
	[MaxLength(50)]
	public required string DisplayName { get; init; }
}
