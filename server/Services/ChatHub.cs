using Microsoft.AspNetCore.SignalR;
using server.Dto.MessageDto;
using server.Interface;
using server.Model;

namespace server.Services;

public class ChatHub: Hub, IChatHub
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
            MessageText = message.MessageText
        };
        
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", msg);
    }
}