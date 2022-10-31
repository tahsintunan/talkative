using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Followers.Queries.GetFollowers;

public class GetFollowersQuery : IRequest<IList<UserVm>>
{
    public string? CurrentUserId { get; set; }
    public string? UserId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, IList<UserVm>>
{
    private readonly IFollow _followerService;
    private readonly IBlockFilter _blockFilter;

    public GetFollowersQueryHandler(IFollow followerService, IBlockFilter blockFilter)
    {
        _followerService = followerService;
        _blockFilter = blockFilter;
    }

    public async Task<IList<UserVm>> Handle(
        GetFollowersQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        var followers = await _followerService.GetFollowerOfSingleUser(request.UserId!, skip, limit);
        return await _blockFilter.GetFilteredUsers(followers, request.CurrentUserId!);
    }
}