using heartbeat_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace heartbeat_api.Controllers;


[ApiController]
[Route("[controller]")]
public class HeartbeatController : ControllerBase
{
    private readonly IHeartbeatService _heartbeatService;
    public HeartbeatController(IHeartbeatService heartbeatService)
    {
        _heartbeatService = heartbeatService;
    }

    [HttpGet(Name = "Heartbeat")]
    public async Task<IActionResult> Heartbeat()
    {
        try
        {
            if (!_heartbeatService.IsValidRequest(Request))
                return StatusCode(StatusCodes.Status400BadRequest, "Unauthorized. Invalid Request.");

            var token = _heartbeatService.GetToken(Request);
            var userId = _heartbeatService.GetUserId(token);
            var userName = _heartbeatService.GetUserName(token);

            await _heartbeatService.Heartbeat(userId, userName, "kernel-panic:", 75);
            return StatusCode(StatusCodes.Status200OK, new { response = "OK" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

