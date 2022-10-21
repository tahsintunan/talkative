using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Blocks.Command.Block;

public class BlockCommand : IRequest
{
    public string? BlockedBy { get; set; }
    public string? Blocked { get; set; }
}

public class BlockCommandHandler : IRequestHandler<BlockCommand>
{
    private readonly IUserService _userService;
    private readonly IFollow _followService;

    public BlockCommandHandler(IUserService userService, IFollow followService)
    {
        _userService = userService;
        _followService = followService;
    }

    public async Task<Unit> Handle(BlockCommand request, CancellationToken cancellationToken)
    {
        if (request.Blocked == request.BlockedBy)
            return Unit.Value;

        var blockedUser = await _userService.GetUserById(request.Blocked!);
        var blockedByUser = await _userService.GetUserById(request.BlockedBy!);
        if (blockedUser == null || blockedByUser == null)
            return Unit.Value;

        // blockedBy
        var blockedByList = blockedUser.BlockedBy;
        if (blockedByList == null)
            blockedByList = new List<string?>();

        if (!blockedByList.Contains(request.BlockedBy))
        {
            blockedByList.Add(request.BlockedBy);
            await _userService.PartialUpdate(
                request.Blocked!,
                Builders<User>.Update.Set(p => p.BlockedBy, new List<string>(blockedByList!))
            );
        }

        // blocked
        var blockedList = blockedByUser.Blocked;
        if (blockedList == null)
            blockedList = new List<string?>();
        if (!blockedList.Contains(request.Blocked))
        {
            blockedList.Add(request.Blocked);
            await _userService.PartialUpdate(
                request.BlockedBy!,
                Builders<User>.Update.Set(p => p.Blocked, new List<string>(blockedList!))
            );
        }

        await _followService.DeleteFollower(request.Blocked!, request.BlockedBy!);
        await _followService.DeleteFollower(request.BlockedBy!, request.Blocked!);

        return Unit.Value;
    }
}
