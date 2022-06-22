using Microsoft.AspNetCore.Mvc;
using server.Interface;
using server.Model.User;
using server.Dto.RequestDto;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public IAuthService _authService;
        public ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(User user)
        {
            try
            {
                if (user == null || user.Username == null || user.Password == null)
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Invalid request" });
                
                if (await _authService.checkIfUsernameExists(user.Username))
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Username exists" });
                
                await _authService.signupUser(user);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something went wrong.");
            }
        } 

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            try
            {
                if (request == null || request.Username == null || request.Password == null)
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Invalid request" });
                
                string username = request.Username;
                string password = request.Password;
                
                if (!await _authService.checkIfUsernameExists(username))
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Username does not exist" });
                
                if (!await _authService.checkIfPasswordMatches(username, password))
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Password does not match" });
                
                var loginResponse = await _authService.loginUser(request);
                return StatusCode(StatusCodes.Status200OK, loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }
    }
}