using CardAtlas.Server.Models.DTOs.Request;

namespace CardAtlas.UnitTests.DataHelpers;

public static class DTODataHelper
{
	public static SignInDTO GetSignInDTO(string password = "Password123!", string email = "test@email.com")
	{
		return new SignInDTO
		{
			Email = email,
			Password = password
		};
	}

	public static SignUpDTO GetSignUpDTO(string password = "Password123!", string email = "test@email.com")
	{
		return new SignUpDTO
		{
			Email = email,
			Password = password,
			DisplayName = "DisplayName"
		};
	}
}
