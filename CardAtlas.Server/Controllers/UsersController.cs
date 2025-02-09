using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace CardAtlas.Server.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/[controller]/[action]")]
	public class UsersController
	{
		[HttpPost]
		public string SignIn([FromBody] string email, string password)
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		public string SignUp(object userInformation)
		{
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
