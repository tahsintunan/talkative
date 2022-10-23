using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Application.Common.Dto.LoginDto;
using Application.Common.Interface;
using Application.Common.Dto.SignupDto;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuth authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupDto signupRequestDto)
        {
            try
            {
                if (
                    await _authService.CheckIfUserExists(
                        signupRequestDto.Username!,
                        signupRequestDto.Email!
                    )
                )
                    return BadRequest(
                        new
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            message = "User already exists"
                        }
                    );
                await _authService.SignupUser(signupRequestDto);
                return StatusCode(
                    StatusCodes.Status201Created,
                    new { response = "User created successfully" }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { response = "Something went wrong." }
                );
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            try
            {
                var username = request.Username!;
                var password = request.Password!;

                if (!await _authService.CheckIfUsernameExists(username))
                    return BadRequest(
                        new
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            message = "Username does not exist"
                        }
                    );

                if (!await _authService.CheckIfPasswordMatches(username, password))
                    return BadRequest(
                        new
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            message = "Password does not match"
                        }
                    );

                string accessToken = await _authService.LoginUser(request);

                var value = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpContext.Response.Cookies.Append(
                    "authorization",
                    value.ToString(),
                    new CookieOptions { HttpOnly = false, Expires = DateTime.Now.AddDays(7) }
                );

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { response = "Something went wrong." }
                );
            }
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
