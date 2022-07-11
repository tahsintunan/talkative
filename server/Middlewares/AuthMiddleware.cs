using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using server.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

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

            if (request.Path.HasValue && (request.Path.Value == "/api/Auth/login" || request.Path.Value == "/api/Auth/signup"))
            {
                await _next.Invoke(httpContext);
                return;
            }
            
            string? userId=this.DecodeAccessToken(httpContext);
            
            var user=await userService.GetUserById(userId!);

            if(user == null)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Unauthorized");
                return;
            }

            httpContext.Items["User"] = userId;
            await _next.Invoke(httpContext);
            return;
        }


        private string? DecodeAccessToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                return null;
            }

            token = token.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string userId = jwtSecurityToken.Claims.First(claim => claim.Type == "user_id").Value;

            return userId;

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
