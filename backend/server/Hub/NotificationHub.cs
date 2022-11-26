using Application.Common.Interface;
using Application.Common.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace server.Hub;

public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub, INotificationHub
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHub(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotification(NotificationVm notificationVm)
    {
        await _hubContext.Clients.All.SendAsync("GetNotification", notificationVm);
    }
}