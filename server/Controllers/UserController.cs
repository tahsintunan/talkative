using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Interface;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user=await _userService.GetUserById(id);
            if (user == null)
            {
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "User doesn't exist" });
            }
            return Ok(user);
        }

    }
}
