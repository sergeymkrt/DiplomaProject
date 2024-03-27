namespace DiplomaProject.WebApi.Middlewares;

public class RefreshTokenValidatorMiddleware(
    RequestDelegate next,
    ILogger<RefreshTokenValidatorMiddleware> logger,
    IServiceScopeFactory scopeFactory,
    IHostEnvironment environment)
{
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("refreshToken", out var header))
        {
            var refreshToken = header.ToString();
            var scope = scopeFactory.CreateScope();
            var authenticationService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();

            if (await authenticationService.IsTokenBlackListedAsync(refreshToken))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Token is blacklisted");
                return;
            }
        }
        await next(context);
    }
}

public static class RefreshTokenValidatorMiddlewareExtensions
{
    public static IApplicationBuilder UseRefreshTokenValidatorMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RefreshTokenValidatorMiddleware>();
    }
}