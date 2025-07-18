namespace TektonChallengeProducts.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var env = context.RequestServices.GetService<IHostEnvironment>();
            string errorMsg = (env != null && env.IsDevelopment())
                            ? ex.Message
                            : "Ocurrió un error inesperado.";

            await context.Response.WriteAsync($"{{\"error\": \"{errorMsg}\"}}");
        }
    }
}
