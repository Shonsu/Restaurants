using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFound)
        {
            logger.LogWarning(notFound.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFound.Message);
        }
        catch (ForbiddenException forbidden)
        {
            logger.LogError(forbidden, forbidden.Message);
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(forbidden.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}
