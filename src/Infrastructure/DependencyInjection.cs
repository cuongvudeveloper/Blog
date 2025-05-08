using System.Text;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Models;
using Blog.Domain.Entities;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Data.Interceptors;
using Blog.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder, ILogger logger)
    {
        string? connectionString = builder.Configuration.GetConnectionString("BlogDb");
        _ = Guard.Against.Null(connectionString, message: "Connection string 'BlogDb' not found.");

        _ = builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        _ = builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        _ = builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            _ = options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            _ = options.UseMySQL(connectionString);
        });


        _ = builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        _ = builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        JwtSettings? jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        _ = Guard.Against.Null(jwtSettings, message: "JwtSettings not found.");

        _ = builder.Services.AddAuthentication()
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = jwtSettings.ValidAudience,
                ValidIssuer = jwtSettings.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
            });

        _ = builder.Services.AddAuthorizationBuilder();

        _ = builder.Services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager();

        _ = builder.Services.AddSingleton(TimeProvider.System);
        _ = builder.Services.AddTransient<IIdentityService, IdentityService>();

        _ = builder.Services.AddAuthorization();

        logger.LogInformation("{Project} services registered", "Infrastructure");
    }
}
