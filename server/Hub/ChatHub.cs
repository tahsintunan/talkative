using Microsoft.AspNetCore.SignalR;
using server.Dto.MessageDto;
using server.Interface;
using server.Model.Message;

namespace server.Hub;

public class ChatHub: Microsoft.AspNetCore.SignalR.Hub, IChatHub
{
    private readonly IHubContext<ChatHub> _hubContext;
    public ChatHub(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task SendMessage(Message message)
    {
        var msg = new MessageDto
        {
            SenderId = message.SenderId,
            ReceiverId = message.ReceiverId,
            MessageText = message.MessageText,
            Datetime = message.Datetime
        };
        
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", msg);
    }
}