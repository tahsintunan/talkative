using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace Application.Followers.Commands.AddFollower
{
    public class AddFollowerCommand : IRequest
    {
        public string? FollowerId { get; set; }
        public string? FollowingId { get; set; }
    }

    public class AddFollowerCommandHandler : IRequestHandler<AddFollowerCommand>
    {
        private readonly IFollow _followerService;
        private readonly Common.Interface.INotification _notificationService;

        public AddFollowerCommandHandler(
            IFollow followerService,
            Common.Interface.INotification notificationService
        )
        {
            _followerService = followerService;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(
            AddFollowerCommand request,
            CancellationToken cancellationToken
        )
        {
            var followerExists = await _followerService.CheckIfFollowerExists(
                request.FollowerId!,
                request.FollowingId!
            );
            if (!followerExists)
            {
                await _followerService.AddNewFollower(
                    new Follower()
                    {
                        FollowerId = request.FollowerId,
                        FollowingId = request.FollowingId,
                        Id = ObjectId.GenerateNewId().ToString(),
                    }
                );
            }
            await _notificationService.TriggerFollowNotification(request);
            return Unit.Value;
        }
    }
}
