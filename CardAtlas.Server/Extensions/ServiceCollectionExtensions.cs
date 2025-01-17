using CardAtlas.Server.DAL;
using CardAtlas.Server.Guards;
using CardAtlas.Server.Models.Internal;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CardAtlas.Server.Extensions;

public static class ServiceCollectionExtensions
{
	private static AppSettings? _appSettings;

	/// <summary>
	/// Returns the <see cref="AppSettings"/> object from the <see cref="IServiceCollection"/>.<br/>
	/// Throws a <see cref="NullReferenceException"/> if the <see cref="AppSettings"/> object is null. This is usually due to an invalid appsettings.json file or the buildprovider not being properly configured.
	/// </summary>
	/// <exception cref="NullReferenceException"></exception>
	private static AppSettings GetAppSettings(IServiceCollection services)
	{
		if(_appSettings != null) return _appSettings;

		AppSettings? appSettings = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value;

		if (appSettings is null) throw new NullReferenceException("Could not initialize AppSettings from appsettings.json.");
		if (string.IsNullOrEmpty(appSettings.AppName)) throw new NullReferenceException("Could not initialize AppName from appsettings.json.");
		if (!appSettings.HasValidConnectionStrings()) throw new NullReferenceException("Could not initialize connectionstrings from appsettings.json.");

		return appSettings;
	}

	/// <summary>
	/// Adds the database context and connection for <see cref="ApplicationDbContext"/>.
	/// </summary>
	public static void AddDatabaseContext(this IServiceCollection services)
	{
		AppSettings appSettings = GetAppSettings(services);

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

	/// <summary>
	/// Adds all the swagger documents to the API for each ApiVersion.
	/// </summary>
	/// <param name="services"></param>
	public static void AddSwagger(this IServiceCollection services)
	{
		IReadOnlyList<ApiVersionDescription> apiDescriptiontions = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions;
		AppSettings appSettings = GetAppSettings(services);

		services.AddSwaggerGen(options =>
		{
			foreach (ApiVersionDescription apiDescriptiontion in apiDescriptiontions)
			{
				var openApiInfo = new OpenApiInfo
				{
					Title = $"{appSettings.AppName} {apiDescriptiontion.GroupName.ToUpperInvariant()}",
					Version = apiDescriptiontion.ApiVersion.ToString(),
					Description = apiDescriptiontion.IsDeprecated ? "This version has been deprecated." : string.Empty
				};

				options.SwaggerDoc(apiDescriptiontion.GroupName, openApiInfo);
			}
		});
	}
}
