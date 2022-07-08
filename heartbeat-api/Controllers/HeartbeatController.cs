using Microsoft.AspNetCore.Mvc;
using heartbeat_api.Services;

namespace heartbeat_api.Controllers;


[ApiController]
[Route("[controller]")]
public class HeartbeatController : ControllerBase
{
    private readonly ILogger<HeartbeatController> _logger;
    private readonly IConfiguration _configuration;
    public HeartbeatService _heartbeatService;
    public HeartbeatController(ILogger<HeartbeatController> logger)
    {
        _logger = logger;
        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _heartbeatService = new HeartbeatService(_configuration);
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
            await _heartbeatService.Heartbeat(userId: userId, prefix: "kernel-panic:", expiry: 90);
            return StatusCode(StatusCodes.Status200OK, "OK");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

