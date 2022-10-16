using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace Application.Followers.Commands.AddFollower
{
    public class AddFollowerCommand:IRequest
    {
        public string? FollowerId { get; set; }
        public string? FollowingId { get; set; }
    }

    public class AddFollowerCommandHandler : IRequestHandler<AddFollowerCommand>
    {
        public IFollow _followerService;
        public AddFollowerCommandHandler(IFollow followerService)
        {
            _followerService = followerService;
        }

        public async Task<Unit> Handle(AddFollowerCommand request, CancellationToken cancellationToken)
        {
            bool followerExists = await _followerService.CheckIfFollowerExists(request.FollowerId!, request.FollowingId!);
            if (!followerExists)
            {
                await _followerService.AddNewFollower(new Follower()
                {
                    FollowerId = request.FollowerId,
                    FollowingId = request.FollowingId,
                    Id = ObjectId.GenerateNewId().ToString(),
                });
            }
            return Unit.Value;
        }
    }
}
