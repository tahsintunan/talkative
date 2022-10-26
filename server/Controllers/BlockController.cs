using Application.Blocks.Command.Block;
using Application.Blocks.Command.Unblock;
using Application.Blocks.Queries.GetBlockedUserIds;
using Application.Blocks.Queries.GetBlockedUsers;
using Application.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

public class BlockController : ApiControllerBase
{
    [HttpPost("{id}")]
    public async Task<ActionResult> Block(string id)
    {
        BlockCommand command =
            new() { BlockedBy = HttpContext.Items["User"]!.ToString(), Blocked = id };
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<UserVm>> GetBlockedUsers(
        [FromQuery] GetBlockedUsersQuery getBlockedUsersQuery
    )
    {
        getBlockedUsersQuery.UserId = HttpContext.Items["User"]!.ToString();
        return Ok(await Mediator.Send(getBlockedUsersQuery));
    }

    [HttpGet("blocked-id")]
    public async Task<ActionResult<Dictionary<string, bool>>> GetBlockedUserIds(
            )
    {
        var userId = HttpContext.Items["User"]!.ToString();
        return Ok(await Mediator.Send(new GetBlockedUserIdsQuery() { UserId = userId }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Unblock(string id)
    {
        UnblockCommand command =
            new() { BlockedBy = HttpContext.Items["User"]!.ToString(), Blocked = id };
        await Mediator.Send(command);
        return NoContent();
    }
}
