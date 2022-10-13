using Application.Common.Dto.ChatHistoryDto;
using Application.Common.ViewModels;

namespace Application.Common.Interface;

public interface IChatService
{
    public List<ChatHistoryVm> GetMessageHistory(ChatHistoryDto chatHistoryRequestDto);
}