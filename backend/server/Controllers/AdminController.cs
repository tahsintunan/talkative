using Application.Common.ViewModels;
using Application.Users.Commands.BanUser;
using Application.Users.Commands.UnbanUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.SearchUsers;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

public class AdminController : ApiControllerBase
{
    [HttpGet("user")]
    public async Task<ActionResult<IList<UserVm>>> GetAllUsers(
        [FromQuery] GetAllUsersQuery getAllUsersQuery
    )
    {
        if (!(bool)HttpContext.Items["Admin"]!)
            return Unauthorized();

        return Ok(await Mediator.Send(getAllUsersQuery));
    }

    [HttpGet("user/search/{username}")]
    public async Task<ActionResult<IList<UserVm>>> SearchUser(
        string username,
        [FromQuery] SearchUserQuery searchUserQuery
    )
    {
        if (!(bool)HttpContext.Items["Admin"]!)
            return Unauthorized();

        searchUserQuery.Username = username;
        return Ok(await Mediator.Send(searchUserQuery));
    }

    [HttpPut("user/profile")]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand updateUserCommand)
    {
        if (!(bool)HttpContext.Items["Admin"]!)
            return Unauthorized();

        await Mediator.Send(updateUserCommand);
        return NoContent();
    }

    [HttpPatch("user/ban/{id}")]
    public async Task<IActionResult> BanUser(string id)
    {
        if (!(bool)HttpContext.Items["Admin"]!)
            return Unauthorized();

        await Mediator.Send(new BanUserCommand { UserId = id });
        return NoContent();
    }

    [HttpPatch("user/unban/{id}")]
    public async Task<ActionResult> UnbanUser(string id)
    {
        if (!(bool)HttpContext.Items["Admin"]!)
            return Unauthorized();

        await Mediator.Send(new UnbanUserCommand { UserId = id });
        return NoContent();
    }
}