using server.Application.Dto.ChatHistoryDto;
using server.Application.ViewModels;

namespace server.Application.Interface;

public interface IChatService
{
    public List<ChatHistoryVm> GetMessageHistory(ChatHistoryDto chatHistoryRequestDto);
}