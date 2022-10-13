using Application.Dto.MessageDto;

namespace Application.Interface;

public interface IRabbitmqService
{
    Task FanOut(MessageDto messageDto);
}