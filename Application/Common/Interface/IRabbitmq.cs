using Domain.Entities;

namespace Application.Common.Interface;

public interface IRabbitmq
{
    Task FanOut(Notification notification);
}
