using Microsoft.AspNetCore.Mvc;
using server.Interface;
using server.Dto.RequestDto.SignupRequestDto;
using server.Dto.RequestDto.LoginRequestDto;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequestDto signupRequestDto)
        {
            try
            {
                if (await _authService.CheckIfUserExists(signupRequestDto.Username!, signupRequestDto.Email!))
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "User already exists" });
                await _authService.SignupUser(signupRequestDto);
                return StatusCode(StatusCodes.Status201Created, new {response = "User created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new {response = "Something went wrong." });
            }
        } 

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            try
            {   
                var username = request.Username!;
                var password = request.Password!;
                
                if (!await _authService.CheckIfUsernameExists(username))
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Username does not exist" });
                
                if (!await _authService.CheckIfPasswordMatches(username, password))
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Password does not match" });
                
                await _authService.LoginUser(request, HttpContext);
                return StatusCode(StatusCodes.Status200OK, new {response = "User logged in" });
            }
            catch (Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new {response = "Something went wrong." });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutUser(HttpContext);
            return StatusCode((StatusCodes.Status200OK));
        }
    }
}