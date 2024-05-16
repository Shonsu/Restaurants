using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopwatch.Stop();
        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        if (elapsedMilliseconds > 4000)
        {
            logger.LogInformation(
                "Request [{Verb}] at {Path} took {Time} ms.",
                context.Request.Method,
                context.Request.Path,
                elapsedMilliseconds 
            );
        }
    }
}
