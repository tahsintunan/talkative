using System.IdentityModel.Tokens.Jwt;
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

    public async Task Invoke(HttpContext httpContext, IUser userService)
    {
        var request = httpContext.Request;
        if (request.Path.HasValue && request.Path.Value.ToLower().Contains("auth"))
        {
            await _next.Invoke(httpContext);
            return;
        }

        var userId = DecodeAccessToken(httpContext);
        var user = await userService.GetUserById(userId!);
        if (user == null)
        {
            httpContext.Response.StatusCode = 401;
            await httpContext.Response.WriteAsync("Unauthorized");
            return;
        }

        if (user.IsBanned)
        {
            httpContext.Response.StatusCode = 401;
            await httpContext.Response.WriteAsync("Unauthorized");
            return;
        }

        httpContext.Items["User"] = userId;
        httpContext.Items["Admin"] = user.IsAdmin;
        await _next.Invoke(httpContext);
    }

    private static string? DecodeAccessToken(HttpContext httpContext)
    {
        try
        {
            var token = httpContext.Request.Cookies["authorization"];
            if (token == null)
                return null;

            token = token.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var userId = jwtSecurityToken.Claims.First(claim => claim.Type == "user_id").Value;

            return userId;
        }
        catch
        {
            return null;
        }
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
