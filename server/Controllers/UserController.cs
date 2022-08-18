using Microsoft.AspNetCore.Mvc;
using server.Dto.UserDto.UpdateUserDto;
using server.Interface;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "User doesn't exist" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            try
            {
                await _userService.UpdateUserInfo(updateUserDto);
                return NoContent();
            }catch (Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            try
            {
                await _userService.DeleteUserById(id);
                return NoContent();
            }catch(Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

    }
}
