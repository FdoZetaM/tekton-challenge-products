namespace TektonChallengeProducts.Api.Middlewares;

using System.Diagnostics;
using Application.Services;

public class RequestTimingMiddleware(RequestDelegate next, ILoggerService loggerService )
{
    private readonly RequestDelegate _next = next;
    private readonly ILoggerService loggerService = loggerService;

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        if (context.Request.Path.StartsWithSegments("/api"))
        {
            loggerService.LogInformation($"Request {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
