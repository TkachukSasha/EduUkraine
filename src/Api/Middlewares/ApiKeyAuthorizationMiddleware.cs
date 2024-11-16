using Microsoft.Extensions.Primitives;

namespace Api.Middlewares;

public class AuthConfig
{
    public required string ApiKey { get; set; }
}

public class ApiKeyAuthorizationMiddleware(AuthConfig authConfig, RequestDelegate next)
{
    private const string ApiKeyHeader = "X-API-Key";
    private const string ErrorMessage = "Not allowed to perform UnAuthorized operation";

    private readonly AuthConfig _authConfig = authConfig;
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if(!context.Request.Headers.TryGetValue(ApiKeyHeader, out StringValues apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(ErrorMessage);
            return;
        }

        if (_authConfig.ApiKey != apiKey.ToString())
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(ErrorMessage);
            return;
        }

        await _next(context);
    }
}
