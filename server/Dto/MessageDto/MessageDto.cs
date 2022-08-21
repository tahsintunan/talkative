namespace server.Dto.MessageDto;

public class MessageDto
{
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public string? MessageText { get; set; }
    public DateTime Datetime { get; set; }
}
