using Domain.Entities;

namespace Application.Common.Interface;

public interface INotificationHub
{
    public Task SendNotification(Notification notification);
}