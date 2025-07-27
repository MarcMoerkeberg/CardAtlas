using CardAtlas.Server.Extensions;
using Hellang.Middleware.ProblemDetails;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureAppSettings(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersioning();
builder.Services.AddSwagger();
builder.Services.AddDatabaseContext();
builder.Services.AddDependencyInjection();
builder.Services.AddGlobalExceptionHandling();
builder.Services.AddAuthentication();
builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtAuthentication();

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");
app.UseSwaggerUI();
app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); // Remove to get production-like errors
}

app.Run();
