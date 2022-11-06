using Application.Common.Enums;
using Application.Common.ViewModels;
using Application.Users.Commands.BanUser;
using Application.Users.Commands.ForgetPassword;
using Application.Users.Commands.UnbanUser;
using Application.Users.Commands.UpdatePassword;
using Application.Users.Commands.UpdatePicture;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetTopActiveUsers;
using Application.Users.Queries.GetTweetAndFollowCountOfUser;
using Application.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

public class UserController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IList<UserVm>>> GetAllUsers(
        [FromQuery] GetAllUsersQuery getAllUsersQuery
    )
    {
        return Ok(await Mediator.Send(getAllUsersQuery));
    }

    [HttpGet("top-active-users")]
    public async Task<ActionResult<IList<UserVm>>> GetTopActiveUsers([FromQuery] GetTopActiveUsersQuery getTopActiveUsersQuery)
    {
        return Ok(await Mediator.Send(getTopActiveUsersQuery));
    }

    [HttpGet("count/{id}")]
    public async Task<ActionResult<GetTweetAndFollowCountOfUserVm>> GetCount(string id)
    {
        return Ok(await Mediator.Send(new GetTweetAndFollowCountOfUserQuery { UserId = id }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserVm>> GetUser(string id)
    {
        GetUserByIdQuery getUserByIdQuery = new() { UserId = id, CurrentUser = HttpContext.Items["User"]!.ToString() };
        return Ok(await Mediator.Send(getUserByIdQuery));
    }

    [HttpPost("forget-password")]
    public async Task<ActionResult> ForgotPassword(ForgetPasswordCommand forgetPasswordCommand)
    {
        await Mediator.Send(forgetPasswordCommand);
        return NoContent();
    }

    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordCommand updatePasswordCommand)
    {
        updatePasswordCommand.UserId = HttpContext.Items["User"]!.ToString();
        await Mediator.Send(updatePasswordCommand);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand updateUserCommand)
    {
        if (updateUserCommand.UserId != HttpContext.Items["User"]!.ToString() || (bool)HttpContext.Items["Admin"]!)
        {
            return Unauthorized();
        }
        await Mediator.Send(updateUserCommand);
        return NoContent();
    }

    [HttpPatch("profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture(IFormFile profilePicture)
    {
        UpdatePictureCommand updateProfilePictureCommand = new()
        {
            Picture = profilePicture,
            Type = PictureType.ProfilePicture,
            UserId = HttpContext.Items["User"]!.ToString(),
        };
        await Mediator.Send(updateProfilePictureCommand);
        return NoContent();
    }

    [HttpPatch("cover-picture")]
    public async Task<IActionResult> UpdateCoverPicture(IFormFile coverPicture)
    {
        UpdatePictureCommand updateProfilePictureCommand = new()
        {
            Picture = coverPicture,
            Type = PictureType.CoverPicture,
            UserId = HttpContext.Items["User"]!.ToString()
        };
        await Mediator.Send(updateProfilePictureCommand);
        return NoContent();
    }

    [HttpPut("unban/{id}")]
    public async Task<ActionResult> UnbanUser(string id)
    {
        if (!(bool)HttpContext.Items["Admin"]!)
        {
            return Unauthorized();
        }
        await Mediator.Send(new UnbanUserCommand { UserId = id });
        return NoContent();
    }

    [HttpDelete("ban/{id}")]
    public async Task<IActionResult> BanUser(string id)
    {
        if (!(bool)HttpContext.Items["Admin"]!)
        {
            return Unauthorized();
        }
        BanUserCommand banUserCommand = new() { UserId = id };
        await Mediator.Send(banUserCommand);
        return NoContent();
    }
}