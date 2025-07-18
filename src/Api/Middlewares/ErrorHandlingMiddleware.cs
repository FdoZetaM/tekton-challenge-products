namespace TektonChallengeProducts.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private readonly RequestDelegate next = next;
    private readonly ILogger<ErrorHandlingMiddleware> logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.ContentType = "application/json";

            var env = context.RequestServices.GetService<IHostEnvironment>();
            string errorMsg = (env != null && env.IsDevelopment()) ? ex.Message : "Ocurrió un error inesperado.";

            SetStatusCode(context, ex);

            await context.Response.WriteAsync($"{{\"error\": \"{errorMsg}\"}}");
        }
    }

    private static void SetStatusCode(HttpContext context, Exception ex)
    {
        if (ex is KeyNotFoundException || ex is FluentValidation.ValidationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
}
