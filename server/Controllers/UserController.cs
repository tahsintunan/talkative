using Microsoft.AspNetCore.Mvc;
using server.Interface;
using server.Model;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService=userService;
        }
        [HttpPost]
        public async Task<IActionResult> Signup(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                await _userService.signupUser(user);
                return Ok(user);
            }
        }
    }
}