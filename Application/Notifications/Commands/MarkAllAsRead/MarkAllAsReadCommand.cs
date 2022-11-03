using System.Text.Json.Serialization;
using MediatR;
using INotification = Application.Common.Interface.INotification;

namespace Application.Notifications.Commands.MarkAllAsRead
{
    public class MarkAllAsReadCommand : IRequest
    {
        [JsonIgnore] public string? UserId { get; set; }
    }

    public class MarkAllAsReadCommandHandler : IRequestHandler<MarkAllAsReadCommand>
    {
        private readonly INotification _notificationService;
        public MarkAllAsReadCommandHandler(INotification notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<Unit> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
        {
            await _notificationService.MarkAllAsRead(request.UserId!);
            return Unit.Value;
        }
    }
}