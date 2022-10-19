
using Application.Blocks.Command.Block;
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
    
}