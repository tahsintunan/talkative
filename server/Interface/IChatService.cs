using server.Dto.RequestDto.ChatHistoryRequestDto;
using server.Dto.ResponseDto.ChatHistoryResponseDto;

namespace server.Interface;

public interface IChatService
{
    public List<ChatHistoryResponseDto> GetMessageHistory(ChatHistoryRequestDto chatHistoryRequestDto);
}