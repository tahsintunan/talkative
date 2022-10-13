using Application.Dto.ChatHistoryDto;
using Application.ViewModels;

namespace Application.Interface;

public interface IChatService
{
    public List<ChatHistoryVm> GetMessageHistory(ChatHistoryDto chatHistoryRequestDto);
}