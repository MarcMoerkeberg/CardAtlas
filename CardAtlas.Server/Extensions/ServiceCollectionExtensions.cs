using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CardAtlas.Server.DAL;
using CardAtlas.Server.Exceptions;
using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Resources;
using CardAtlas.Server.Resources.Errors;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CardAtlas.Server.Extensions;

public static class ServiceCollectionExtensions
{
	private static readonly ApiVersion _defaultApiVersion = new(1, 0);
	private static AppSettings? _appSettings;

	/// <summary>
	/// Returns the <see cref="AppSettings"/> object from the <see cref="IServiceCollection"/>.<br/>
	/// Throws a <see cref="NullReferenceException"/> if the <see cref="AppSettings"/> object is null. This is usually due to an invalid appsettings.json file or the buildprovider not being configured.
	/// </summary>
	/// <exception cref="NullReferenceException"></exception>
	private static AppSettings GetAppSettings(IServiceCollection services)
	{
		if (_appSettings is null)
		{
			AppSettings? appSettings = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value;

			if (appSettings is null) throw new NullReferenceException(Errors.ErrorInitializingConnectionStrings);
			if (string.IsNullOrEmpty(appSettings.AppName)) throw new NullReferenceException(Errors.ErrorInitializingConnectionStrings);
			if (appSettings.ConnectionStrings is null) throw new NullReferenceException(Errors.ErrorInitializingConnectionStrings);

			_appSettings = appSettings;
		}

		return _appSettings;
	}

	/// <summary>
	/// Adds the database context and connection for <see cref="ApplicationDbContext"/>.
	/// </summary>
	public static void AddDatabaseContext(this IServiceCollection services)
	{
		AppSettings appSettings = GetAppSettings(services);
		Action<DbContextOptionsBuilder> dbContextOptions = options =>
		options.UseSqlServer(appSettings.ConnectionStrings.Database, sqlServerOptions =>
			sqlServerOptions.EnableRetryOnFailure(
				maxRetryCount: 5,
				maxRetryDelay: TimeSpan.FromSeconds(20),
				errorNumbersToAdd: null
			)
		);

		services.AddDbContextFactory<ApplicationDbContext>(dbContextOptions, lifetime: ServiceLifetime.Scoped);
		services.AddDbContext<ApplicationDbContext>(dbContextOptions);
	}

	/// <summary>
	/// Adds all the swagger documents to the API for each ApiVersion.
	/// </summary>
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
					Title = $"{appSettings.AppName} {apiDescriptiontion.GroupName}",
					Version = apiDescriptiontion.ApiVersion.ToString(),
					Description = apiDescriptiontion.IsDeprecated ? ApplicationSettings.ApiVersionDeprecated : string.Empty
				};

				options.SwaggerDoc(apiDescriptiontion.GroupName, openApiInfo);
			}
		});
	}

	/// <summary>
	/// Adds API versioning to the application.
	/// </summary>
	public static void AddVersioning(this IServiceCollection services)
	{
		const string apiVersionHeader = "api-version";
		const string apiVersionFormat = "'v'VVV";

		services.AddApiVersioning(options =>
		{
			options.DefaultApiVersion = _defaultApiVersion;
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ReportApiVersions = true; //Adds headers "api-supported-versions" and "api-deprecated-versions" to the response
			options.ApiVersionReader = ApiVersionReader.Combine(
				new HeaderApiVersionReader(apiVersionHeader), //Reads request the header "api-version"
				new QueryStringApiVersionReader(apiVersionHeader) //Reads the query string parameter "api-version"
			);
		})
		.AddApiExplorer(options =>
		{
			options.GroupNameFormat = apiVersionFormat; //Version formatting: "v1.0", "v2.0", etc.
			options.SubstituteApiVersionInUrl = true;
		});
	}

	/// <summary>
	/// Adds dependency injection for all classes implementing an interface within the application.
	/// </summary>
	public static void AddDependencyInjection(this IServiceCollection services)
	{
		AddScopedDependencies(services, "Services");
		AddScopedDependencies(services, "Repositories");
		AddScryfallApi(services);
		AddComparerDependencies(services);
	}

	/// Adds scoped lifetime dependency injection for classes that implements one or more interfaces and lives within the CardAtlas.Server namespace.<br/>
	/// <paramref name="namespaceSuffix"/> is the suffix of the namespace where the classes are located (ex. Services or Repositories).
	/// </summary>
	private static void AddScopedDependencies(IServiceCollection services, string namespaceSuffix)
	{
		IEnumerable<Type> classesWithInterfaces = AssemblyHelper.GetClassesThatImplementInterfaces(namespaceSuffix);

		foreach (Type @class in classesWithInterfaces)
		{
			IEnumerable<Type> serviceInterfaces = @class.GetInterfaces()
					.Where(@interface =>
						!string.IsNullOrEmpty(@interface.Namespace) &&
						@interface.Namespace.StartsWith("CardAtlas.Server", StringComparison.Ordinal)
					);

			foreach (Type @interface in serviceInterfaces)
			{
				services.AddScoped(@interface, @class);
			}
		}
	}

	/// <summary>
	/// Dependency injects <see cref="ScryfallApi.ScryfallApi"/>.
	/// </summary>
	private static void AddScryfallApi(IServiceCollection services)
	{
		AppSettings appSettings = GetAppSettings(services);

		services.AddSingleton<ScryfallApi.IScryfallApi>(serviceProvider =>
		{
			return new ScryfallApi.ScryfallApi(appSettings.AppName);
		});
	}

	/// <summary>
	/// Adds global error handling with <see cref="GlobalExceptionHandler"/> and <see cref="ProblemDetails"/>.
	/// </summary>
	public static void AddGlobalExceptionHandling(this IServiceCollection services)
	{
		services.AddExceptionHandler<GlobalExceptionHandler>();
		ProblemDetailsExtensions.AddProblemDetails(services);
	}

	/// <summary>
	/// Adds scoped lifetime dependency injection for all comparers within CardAtlas.Server.Comparers namespace that implements IEqualityComparer.
	/// </summary>
	private static void AddComparerDependencies(IServiceCollection services)
	{
		const string comparerNamespace = "CardAtlas.Server.Comparers";
		IEnumerable<Type> comparers = AssemblyHelper.GetClassesThatImplementInterfaces(comparerNamespace);

		foreach (Type comparer in comparers)
		{
			IEnumerable<Type> interfaces = comparer.GetInterfaces()
				.Where(@interface =>
					@interface.IsGenericType &&
					@interface.GetGenericTypeDefinition() == typeof(IEqualityComparer<>)
				);

			foreach (Type @interface in interfaces)
			{
				services.AddScoped(@interface, comparer);
			}
		}
	}

	public static void AddIdentityConfiguration(this IServiceCollection services)
	{
		services.AddIdentity<User, IdentityRole>(options =>
		{
			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireUppercase = true;
			options.Password.RequireNonAlphanumeric = true;
			options.Password.RequiredLength = 8;

			options.User.RequireUniqueEmail = true;

			options.SignIn.RequireConfirmedEmail = false;
		})
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddDefaultTokenProviders();
	}
}
