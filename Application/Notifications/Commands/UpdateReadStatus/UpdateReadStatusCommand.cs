using MediatR;
using INotification = Application.Common.Interface.INotification;
namespace Application.Notifications.Commands.UpdateReadStatus
{
    public class UpdateReadStatusCommand : IRequest
    {
        public string? NotificationId { get; set; }
    }

    public class UpdateReadStatusCommandHandler : IRequestHandler<UpdateReadStatusCommand>
    {
        private readonly INotification _notificationService;
        public UpdateReadStatusCommandHandler(INotification notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<Unit> Handle(UpdateReadStatusCommand request, CancellationToken cancellationToken)
        {
            await _notificationService.UpdateReadStatus(request.NotificationId!);
            return Unit.Value;
        }
    }
}