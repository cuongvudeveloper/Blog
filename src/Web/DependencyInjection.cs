using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Models;
using Blog.Infrastructure.Data;
using Blog.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Blog.Web;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder, ILogger logger)
    {
        _ = builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        _ = builder.Services.AddScoped<IUser, CurrentUser>();

        _ = builder.Services.AddHttpContextAccessor();
        _ = builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        _ = builder.Services.AddExceptionHandler<CustomExceptionHandler>();


        // Customise default API behaviour
        _ = builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        _ = builder.Services.AddEndpointsApiExplorer();

        _ = builder.Services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "Blog API";

            // Add JWT
            _ = configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        logger.LogInformation("{Project} services registered", "Web");
    }

    public static void AddOptionConfigs(this IHostApplicationBuilder builder, ILogger logger)
    {
        _ = builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        logger.LogInformation("{Project} were configured", "Configuration and Options");
    }
}
