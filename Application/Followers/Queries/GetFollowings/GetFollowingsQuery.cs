using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Followers.Queries.GetFollowings;

public class GetFollowingsQuery : IRequest<IList<UserVm>>
{
    public string? CurrentUserId { get; set; }
    public string? UserId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetFollowingsQueryHandler : IRequestHandler<GetFollowingsQuery, IList<UserVm>>
{
    private readonly IFollow _followerService;
    private readonly IBlockFilter _blockFilter;

    public GetFollowingsQueryHandler(IFollow followerService, IBlockFilter blockFilter)
    {
        _followerService = followerService;
        _blockFilter = blockFilter;
    }

    public async Task<IList<UserVm>> Handle(
        GetFollowingsQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        var followings = await _followerService.GetFollowingsOfSingleUser(request.UserId!, skip, limit);
        return await _blockFilter.GetFilteredUsers(followings, request.CurrentUserId!);
    }
}