using System.Reflection;
using Blog.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder, ILogger logger)
    {
        _ = builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        _ = builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        _ = builder.Services.AddMediatR(cfg =>
        {
            _ = cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            _ = cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            _ = cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            _ = cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            _ = cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        logger.LogInformation("{Project} were configured", "Application");
    }
}
