using server.Application.Dto.MessageDto;

namespace server.Application.Interface;

public interface IRabbitmqService
{
    Task FanOut(MessageDto messageDto);
}