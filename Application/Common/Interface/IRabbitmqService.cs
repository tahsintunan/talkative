using Application.Common.Dto.MessageDto;

namespace Application.Common.Interface;

public interface IRabbitmqService
{
    Task FanOut(MessageDto messageDto);
}