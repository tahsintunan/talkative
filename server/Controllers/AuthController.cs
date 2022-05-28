using Microsoft.AspNetCore.Mvc;
using server.Interface;
using server.Model.User;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public IUserService _userService;
        public ILogger<AuthController> _logger;
        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(User user)
        {
            try
            {
                if (await _userService.findUser(user) != null)
                {
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Username exists" });
                }
                else
                {
                    await _userService.signupUser(user);
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