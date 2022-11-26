using Application.Common.ViewModels;

namespace Application.Common.Interface;

public interface INotificationHub
{
    public Task SendNotification(NotificationVm notificationVm);
}