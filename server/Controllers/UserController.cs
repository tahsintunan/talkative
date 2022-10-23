﻿using Application.Common.ViewModels;
using Application.Users.Commands.BanUser;
using Application.Users.Commands.ForgetPassword;
using Application.Users.Commands.UnbanUser;
using Application.Users.Commands.UpdatePassword;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class UserController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<UserVm>>?> GetAllUsers(
            [FromQuery] GetAllUsersQuery getAllUsersQuery
        )
        {
            return Ok(await Mediator.Send(getAllUsersQuery));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserVm>> GetUser(string id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery() { UserId = id }));
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
            updateUserCommand.Id = HttpContext.Items["User"]!.ToString();
            await Mediator.Send(updateUserCommand);
            return NoContent();
        }

        [HttpPut("unban/{id}")]
        public async Task<ActionResult> UnbanUser(string id)
        {
            await Mediator.Send(new UnbanUserCommand() { UserId = id });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BanUser(string id)
        {
            BanUserCommand banUserCommand = new BanUserCommand() { UserId = id };
            await Mediator.Send(banUserCommand);
            return NoContent();
        }
    }
}
