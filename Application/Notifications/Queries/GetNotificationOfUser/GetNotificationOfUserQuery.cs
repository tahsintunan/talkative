using System.Text.Json.Serialization;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;
using INotification = Application.Common.Interface.INotification;

namespace Application.Notifications.Queries.GetNotificationOfUser;

public class GetNotificationOfUserQuery : IRequest<IList<NotificationVm>>
{
    [JsonIgnore] public string? UserId { get; set; }

    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetNotificationOfUserQueryHandler
    : IRequestHandler<GetNotificationOfUserQuery, IList<NotificationVm>>
{
    private readonly IMapper _mapper;
    private readonly INotification _notification;

    public GetNotificationOfUserQueryHandler(IMapper mapper, INotification notification)
    {
        _mapper = mapper;
        _notification = notification;
    }

    public async Task<IList<NotificationVm>> Handle(
        GetNotificationOfUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        return await _notification.GetNotifications(request.UserId!, skip, limit);
    }
}