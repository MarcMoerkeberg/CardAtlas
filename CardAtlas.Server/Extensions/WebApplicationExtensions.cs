using Asp.Versioning.ApiExplorer;
using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Extensions;

public static class WebApplicationExtensions
{
	/// <summary>
	/// Adds UseSwagger and UseSwaggerUI to the application.<br/>
	/// Creates swagger endpoints for each <see cref="ApiVersionDescription"/>.
	/// </summary>
	public static void UseSwaggerUI(this WebApplication app)
	{
		if (!app.Environment.IsDevelopment()) return;

		AppSettings appSettings = app.Services.GetRequiredService<AppSettings>();
		IReadOnlyList<ApiVersionDescription> apiDescriptiontions = app.DescribeApiVersions();

		app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			foreach (ApiVersionDescription apiDescriptiontion in apiDescriptiontions)
			{
				string url = $"/swagger/{apiDescriptiontion.GroupName}/swagger.json";
				string name = $"{appSettings.AppName} {apiDescriptiontion.GroupName}";

				options.SwaggerEndpoint(url, name);
			}
		});
	}
}
