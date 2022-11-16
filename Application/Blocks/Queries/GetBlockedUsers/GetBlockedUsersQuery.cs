using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Blocks.Queries.GetBlockedUsers;

public class GetBlockedUsersQuery : IRequest<IList<UserVm>>
{
    public string? UserId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetBlockedUserQueryHandler : IRequestHandler<GetBlockedUsersQuery, IList<UserVm>>
{
    private readonly IBlock _blockService;

    public GetBlockedUserQueryHandler(IBlock blockService)
    {
        _blockService = blockService;
    }

    public async Task<IList<UserVm>> Handle(
        GetBlockedUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        return await _blockService.GetBlockedUsers(request.UserId!, skip, limit);
    }
}