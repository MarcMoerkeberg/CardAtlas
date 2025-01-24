using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Internal;
using Hellang.Middleware.ProblemDetails;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersioning();
builder.Services.AddSwagger();
builder.Services.AddDatabaseContext();
builder.Services.AddDependencyInjection();
builder.Services.AddGlobalExceptionHandling();

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");
app.UseSwaggerUI();
app.UseProblemDetails();

app.Run();
