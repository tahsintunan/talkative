using Domain.Entities;

namespace Application.Common.Interface;

public interface IRabbitmqService
{
    Task FanOut(Notification notification);
}