using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Guards;

public static class AppSettingGuards
{
	public static bool HasValidConnectionStrings(this AppSettings? appSettings)
	{
		return appSettings != null
			&& appSettings.ConnectionStrings != null
			&& !string.IsNullOrWhiteSpace(appSettings.ConnectionStrings.Database);
	}
}
