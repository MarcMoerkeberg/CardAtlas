using Asp.Versioning.ApiExplorer;
using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Extensions
{
	public static class WebApplicationExtensions
	{
		private static AppSettings? _appSettings;

		/// <summary>
		/// Returns the <see cref="AppSettings"/> object from the <see cref="IConfiguration"/>.<br/>
		/// Throws a <see cref="NullReferenceException"/> if the <see cref="AppSettings"/> object is null. This is usually due to an invalid appsettings.json file or the buildprovider not being properly configured.
		/// </summary>
		/// <exception cref="NullReferenceException"></exception>
		private static AppSettings GetAppSettings(IConfiguration configuration)
		{
			if (_appSettings != null) return _appSettings;

			AppSettings? appSettings = configuration.Get<AppSettings>();

			if (appSettings is null) throw new NullReferenceException("AppSettings has not been configured from appsettings.");

			return appSettings;
		}

		/// <summary>
		/// Adds UseSwagger and UseSwaggerUI to the application.<br/>
		/// Creates swagger endpoints for each <see cref="ApiVersionDescription"/>.
		/// </summary>
		public static void UseSwaggerUI(this WebApplication app)
		{
			if (!app.Environment.IsDevelopment()) return;

			AppSettings appSettings = GetAppSettings(app.Configuration);
			IReadOnlyList<ApiVersionDescription> apiDescriptiontions = app.DescribeApiVersions();

			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				foreach (ApiVersionDescription apiDescriptiontion in apiDescriptiontions)
				{
					string url = $"/swagger/{apiDescriptiontion.GroupName}/swagger.json";
					string name = apiDescriptiontion.GroupName.ToUpperInvariant();

					options.SwaggerEndpoint(url, name);
				}
			});
		}
	}
}
