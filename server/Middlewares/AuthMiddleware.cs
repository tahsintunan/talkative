using server.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace server.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUserService userService)
        {
            var request = httpContext.Request;
            if (request.Path.HasValue && request.Path.Value.ToLower() is "/api/auth/login" or "/api/auth/signup")
            {
                await _next.Invoke(httpContext);
                return;
            }
            
            var userId= DecodeAccessToken(httpContext);
            var user = await userService.GetUserById(userId!);
            if(user == null)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Unauthorized");
                return;
            }

            httpContext.Items["User"] = userId;
            await _next.Invoke(httpContext);
        }


        private static string? DecodeAccessToken(HttpContext httpContext)
        {
            try
            {
                var token = httpContext.Request.Cookies["authorization"];
                if (token == null) { return null; }

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
}
