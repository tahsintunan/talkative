using Microsoft.AspNetCore.Mvc;
using server.Interface;
using server.Model.User;

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
                if (await _authService.checkIfUsernameExists(user.Username))
                {
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Username exists" });
                }
                else
                {
                    await _authService.signupUser(user);
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something went wrong.");
            }
        }
    }
}