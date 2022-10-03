using Microsoft.AspNetCore.Mvc;
using server.Application.Dto.MessageDto;

namespace server.Application.Interface;

public interface IRabbitmqService
{
    Task<IActionResult> FanOut(MessageDto messageDto);
}