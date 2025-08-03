using CardAtlas.Server.Models.Internal;

namespace CardAtlas.UnitTests.DataHelpers;

public static class ConfigurationDataHelper
{
	public static AppSettings GetAppSettings(
		string appName = "app",
		string audience = "audience",
		int timeToLiveInMinutes = 30)
	{
		return new AppSettings
		{
			AppName = "app",
			ConnectionStrings = GetConnectionStrings(),
			JwtSettings = GetJwtSettings(timeToLiveInMinutes),
		};
	}

	private static ConnectionStrings GetConnectionStrings(string databaseConnection = "ConnectionStringForDb")
	{
		return new ConnectionStrings
		{
			Database = databaseConnection
		};
	}

	private static JwtSettings GetJwtSettings(int timeToLiveInMinutes)
	{
		return new JwtSettings
		{
			Audience = "audience",
			Secret = "MyVerySecretKey",
			TimeToLive = TimeSpan.FromMinutes(timeToLiveInMinutes)
		};
	}
}
