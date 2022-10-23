using Application.Common.Dto.ChatHistoryDto;
using Application.Common.ViewModels;

namespace Application.Common.Interface;

public interface IChat
{
    public List<ChatHistoryVm> GetMessageHistory(ChatHistoryDto chatHistoryRequestDto);
}
