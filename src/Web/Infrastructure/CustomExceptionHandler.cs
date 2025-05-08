using Blog.Application.Common.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace Blog.Web.Infrastructure;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionHandler()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new()
            {
                { typeof(Exception), HandleException },
            };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await HandleException(httpContext, exception);
        return true;
    }

    private async Task HandleException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status200OK;

        await httpContext.Response.WriteAsJsonAsync(Result<object>.Failure(
        [
            exception.Message
        ]));
    }
}
