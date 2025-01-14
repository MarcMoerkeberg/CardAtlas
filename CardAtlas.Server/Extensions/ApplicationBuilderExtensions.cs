using CardAtlas.Server.DAL;
using CardAtlas.Server.Guards;
using CardAtlas.Server.Models.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CardAtlas.Server.Extensions;

public static class ApplicationBuilderExtensions
{
	/// <summary>
	/// Adds the database context and connection for <see cref="ApplicationDbContext"/>.
	/// </summary>
	public static void AddDatabaseContext(this IServiceCollection services)
	{
		AppSettings? appSettings = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value;
		if (!appSettings.HasValidConnectionStrings()) throw new NullReferenceException("Could not initialize AppSettings from appsettings.json.");

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
