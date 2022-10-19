
using Application.Blocks.Command.Block;
using Application.Blocks.Queries.GetBlockedUsers;
using Application.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

public class BlockController: ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult> Block(string id)
    {
        BlockCommand command = new()
        {
            BlockedBy =  HttpContext.Items["User"]!.ToString(),
            Blocked = id
        };
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet("list")]
    public async Task<ActionResult<UserVm>> GetBlockedUsers()
    {
        var userId = HttpContext.Items["User"]!.ToString();
        return Ok(await Mediator.Send(new GetBlockedUsersQuery() { Id = userId }));
    }
    
}