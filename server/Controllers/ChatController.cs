using Microsoft.AspNetCore.Mvc;
using server.Dto.MessageDto;
using server.Interface;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController: ControllerBase
{
    private readonly IRabbitmqService _rabbitmqService;
    private readonly ILogger<ChatController> _logger;
    public ChatController(IRabbitmqService rabbitmqService, ILogger<ChatController> logger)
    {
        _rabbitmqService = rabbitmqService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(MessageDto messageDto)
    {
        try
        {
            var senderId = messageDto.SenderId;
            var senderIdFromCookie = HttpContext.Items["User"];
            if (senderIdFromCookie == null || senderId != senderIdFromCookie.ToString())
            {
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, message = "Invalid sender" });
            }

            await _rabbitmqService.FanOut(messageDto);
            return StatusCode(StatusCodes.Status200OK, "Message pushed to Rabbitmq Exchange");
        }
        catch (Exception ex)
        {
            _logger.LogError("{ErrorMessage}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
        }
    }
}