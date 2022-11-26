using Application.Common.Exceptions;
using Application.Common.Interface;

namespace server.Middlewares;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IToken tokenService, IUser userService)
    {
        var request = httpContext.Request;
        if (request.Path.HasValue && request.Path.Value.ToLower().Contains("auth"))
        {
            await _next.Invoke(httpContext);
            return;
        }

        var token = httpContext.Request.Cookies["authorization"]?.Split(" ").Last();
        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedException("No Token Found.");

        var userId = tokenService.ValidateAccessToken(token);
        var user = await userService.GetUserById(userId!);

        if (user == null)
            throw new UnauthorizedException("Invalid Token.");

        if (user.IsBanned)
            throw new UnauthorizedException("User is banned.");

        httpContext.Items["User"] = userId;
        httpContext.Items["Admin"] = user.IsAdmin;
        await _next.Invoke(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}