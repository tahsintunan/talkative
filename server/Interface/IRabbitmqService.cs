using Microsoft.AspNetCore.Mvc;
using server.Dto.MessageDto;

namespace server.Interface;

public interface IRabbitmqService
{
    Task<IActionResult> FanOut(MessageDto messageDto);
}