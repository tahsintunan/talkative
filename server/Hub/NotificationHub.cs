using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace server.Hub;

public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub, INotificationHub
{
    private readonly IHubContext<NotificationHub> _hubContext;
    public NotificationHub(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task SendNotification(Notification notification)
    {
        var notificationVm = new NotificationVm()
        {
            EventType = notification.EventType,
            EventTriggererId = notification.EventTriggererId,
            EventTriggererUsername = notification.EventTriggererUsername,
            TweetId = notification.TweetId,
            CommentId = notification.CommentId,
            DateTime = notification.Datetime
        };
        
        // logic here
        await _hubContext.Clients.All.SendAsync("GetNotification", notificationVm);
        
        // send notification to client
        // var connectionIds = new List<string>() { "sdf", "sdf" };
        // await _hubContext.Clients.Clients(connectionIds).SendAsync("GetNotification", notificationVm);
    }
}