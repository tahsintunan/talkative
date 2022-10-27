using MediatR;
using INotification = Application.Common.Interface.INotification;
namespace Application.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationCommand : IRequest
    {
        public string? NotificationId { get; set; }
    }

    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
    {
        private readonly INotification _notificationService;
        public DeleteNotificationCommandHandler(INotification notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            await _notificationService.DeleteNotification(request.NotificationId!);
            return Unit.Value;
        }
    }
}