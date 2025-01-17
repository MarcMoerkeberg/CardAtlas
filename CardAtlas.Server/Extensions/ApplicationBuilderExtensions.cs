using CardAtlas.Server.DAL;
using CardAtlas.Server.Guards;
using CardAtlas.Server.Models.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CardAtlas.Server.Extensions;

public static class ApplicationBuilderExtensions
{
	/// <summary>
	/// Returns the <see cref="AppSettings"/> object from the <see cref="IServiceCollection"/>.<br/>
	/// Throws a <see cref="NullReferenceException"/> if the <see cref="AppSettings"/> object is null. This is usually due to an invalid appsettings.json file or the buildprovider not being properly configured.
	/// </summary>
	/// <exception cref="NullReferenceException"></exception>
	private static AppSettings GetAppSettings(this IServiceCollection services)
	{
		AppSettings? appSettings = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value;
		if (appSettings == null) throw new NullReferenceException("Could not initialize AppSettings from appsettings.json.");

		return appSettings;
	}

	/// <summary>
	/// Adds the database context and connection for <see cref="ApplicationDbContext"/>.
	/// </summary>
	public static void AddDatabaseContext(this IServiceCollection services)
	{
		AppSettings appSettings = GetAppSettings(services);
		if(appSettings.HasValidConnectionStrings()) throw new NullReferenceException("Could not initialize connectionstrings from appsettings.json when adding the dbcontext.");

		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(appSettings.ConnectionStrings.Database, sqlServerOptions =>
			{
				sqlServerOptions.EnableRetryOnFailure(
					maxRetryCount: 5,
					maxRetryDelay: TimeSpan.FromSeconds(20),
					errorNumbersToAdd: null
				);
			});
		});
	}
}
