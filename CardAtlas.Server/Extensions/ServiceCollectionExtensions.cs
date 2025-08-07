using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CardAtlas.Server.BackgroundServices;
using CardAtlas.Server.DAL;
using CardAtlas.Server.Exceptions;
using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Entities;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Resources;
using Hellang.Middleware.ProblemDetails;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Channels;

namespace CardAtlas.Server.Extensions;

public static class ServiceCollectionExtensions
{
	private static readonly ApiVersion _defaultApiVersion = new(1, 0);

	/// <summary>
	/// Configures <see cref="AppSettings"/> for the service provider.
	/// </summary>
	public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration config)
	{
		services
			.AddOptions<AppSettings>()
			.Bind(config)
			.ValidateDataAnnotations()
			.ValidateOnStart()
			.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value!);
	}

	/// <summary>
	/// Adds the database context and connection for <see cref="ApplicationDbContext"/>.
	/// </summary>
	public static void AddDatabaseContext(this IServiceCollection services)
	{
		Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptions = (serviceProvider, options) =>
		{
			AppSettings appSettings = serviceProvider.GetRequiredService<AppSettings>();

			options.UseSqlServer(appSettings.ConnectionStrings.Database, sqlServerOptions =>
				sqlServerOptions.EnableRetryOnFailure(
					maxRetryCount: 5,
					maxRetryDelay: TimeSpan.FromSeconds(20),
					errorNumbersToAdd: null
				)
			);
		};

		services.AddDbContextFactory<ApplicationDbContext>(dbContextOptions, lifetime: ServiceLifetime.Scoped);
		services.AddDbContext<ApplicationDbContext>(dbContextOptions);
	}

	/// <summary>
	/// Adds all the swagger documents to the API for each ApiVersion.
	/// </summary>
	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen();

		services.AddSingleton<IConfigureOptions<SwaggerGenOptions>>(serviceProvider =>
			new ConfigureOptions<SwaggerGenOptions>(options =>
			{
				var apiDescProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
				var appSettings = serviceProvider.GetRequiredService<AppSettings>();

				foreach (ApiVersionDescription apiDescriptiontion in apiDescProvider.ApiVersionDescriptions)
				{
					var openApiInfo = new OpenApiInfo
					{
						Title = $"{appSettings.AppName} {apiDescriptiontion.GroupName}",
						Version = apiDescriptiontion.ApiVersion.ToString(),
						Description = apiDescriptiontion.IsDeprecated
						? ApplicationSettings.ApiVersionDeprecated
						: string.Empty
					};

					options.SwaggerDoc(apiDescriptiontion.GroupName, openApiInfo);
				}
			})
		);
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
		services.AddScoped<SmtpClient>();

		AddScopedDependencies(services, "Services");
		AddScopedDependencies(services, "Repositories");
		AddScryfallApi(services);
		AddComparerDependencies(services);
		AddBackgroundServiceDependencies(services);
	}

	/// <summary>
	/// Adds BackgroundService dependencies.
	/// </summary>
	private static void AddBackgroundServiceDependencies(IServiceCollection services)
	{
		var outboxOptions = new UnboundedChannelOptions
		{
			SingleReader = true,
			SingleWriter = false,
		};
		var outboxChannel = Channel.CreateUnbounded<OutboxMessage>(outboxOptions);

		services.AddSingleton(outboxChannel);
		services.AddHostedService<OutboxChannelBackgroundService>();
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
		services.AddSingleton<ScryfallApi.IScryfallApi>(serviceProvider =>
		{
			AppSettings appSettings = serviceProvider.GetRequiredService<AppSettings>();

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

	/// <summary>
	/// Configures identity services for user and role management within the application.
	/// </summary>
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

	/// <summary>
	/// Configures and adds Jason Web Token authentication.
	/// </summary>
	public static void AddJwtAuthentication(this IServiceCollection services)
	{
		services.AddSingleton<IConfigureOptions<JwtBearerOptions>>(serviceProvider =>
			new ConfigureNamedOptions<JwtBearerOptions>(
				JwtBearerDefaults.AuthenticationScheme,
				options =>
				{
					var appSettings = serviceProvider.GetRequiredService<AppSettings>();

					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,

						ValidIssuer = appSettings.AppName,
						ValidAudience = appSettings.JwtSettings.Audience,
						IssuerSigningKey = new SymmetricSecurityKey(appSettings.JwtSettings.Key)
					};
				}
			)
		);

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer();
	}
}
