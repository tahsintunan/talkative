using Application.Common.Interface;
using MediatR;

namespace Application.Followers.Commands.DeleteFollower;

public class DeleteFollowerCommand : IRequest
{
    public string? FollowerId { get; set; }
    public string? FollowingId { get; set; }
}

public class DeleteFollowerCommandHandler : IRequestHandler<DeleteFollowerCommand>
{
    public IFollow _followerService;

    public DeleteFollowerCommandHandler(IFollow followerService)
    {
        _followerService = followerService;
    }

    public async Task<Unit> Handle(DeleteFollowerCommand request, CancellationToken cancellationToken)
    {
        await _followerService.DeleteFollower(request.FollowerId!, request.FollowingId!);
        return Unit.Value;
    }
}