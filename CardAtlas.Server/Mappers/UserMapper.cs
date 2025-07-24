using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.DTOs.Request;

namespace CardAtlas.Server.Mappers;

public static class UserMapper
{
	public static User FromDTO(SignUpDTO signUpDTO)
	{
		return new User
		{
			DisplayName = signUpDTO.DisplayName,
			Email = signUpDTO.Email
		};
	}
}
