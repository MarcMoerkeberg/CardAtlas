using CardAtlas.Server.Models.Data;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CardAtlas.UnitTests.TestHelpers;

public static class MoqHelper
{
	public static Mock<UserManager<User>> GetUserManagerMock()
	{
		return new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		); //Some issue with ctor arguments for proxying this class
	}
}
