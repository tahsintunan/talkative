namespace server.Dto.ResponseDto.ChatHistoryResponseDto;

public class ChatHistoryResponseDto
{
    public string? SenderId { get; set; }
    public string? MessageText { get; set; }
    public DateTime Datetime { get; set; }
}