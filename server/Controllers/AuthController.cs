using Application.Auth.Commands.Login;
using Application.Auth.Commands.Signup;
using Application.Common.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuth _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuth authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupCommand signupCommand)
        {
            var signupSuccess = await Mediator.Send(signupCommand);

            if (!signupSuccess)
            {
                return BadRequest(
                    new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        message = "User already exists"
                    }
                );
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            var accessToken = await Mediator.Send(request);

            var value = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpContext.Response.Cookies.Append(
                "authorization",
                value.ToString(),
                new CookieOptions { HttpOnly = false, Expires = DateTime.Now.AddDays(7) }
            );

            return NoContent();
        }

        [HttpPost("logout")]
        public Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("authorization");
            return Task.FromResult<IActionResult>(
                new OkObjectResult(new { message = "User logged out" })
            );
        }
    }
}
