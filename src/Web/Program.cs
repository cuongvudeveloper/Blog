using Blog.Application;
using Blog.Infrastructure;
using Blog.Infrastructure.Data;
using Blog.Web;
using Serilog;
using Serilog.Extensions.Logging;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", false, true);

Serilog.ILogger logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

logger.Information("Starting web host");

ILogger<Program> appLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

// Add services to the container.
builder.AddOptionConfigs(appLogger);
builder.AddApplicationServices(appLogger);
builder.AddInfrastructureServices(appLogger);
builder.AddWebServices(appLogger);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    _ = app.Map("/", () => Results.Redirect("/api"));
    _ = app.UseCors(options => options.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.MapEndpoints();

app.Run();

public partial class Program { }
