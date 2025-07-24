using Microsoft.AspNetCore.Identity;

namespace CardAtlas.Server.Models.Data;

public class User : IdentityUser
{
	public required string DisplayName { get; init; }
}
