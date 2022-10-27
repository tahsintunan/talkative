using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Followers.Queries.GetFollowings;

public class GetFollowingsQuery : IRequest<IList<UserVm>>
{
    public string? UserId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetFollowingsQueryHandler : IRequestHandler<GetFollowingsQuery, IList<UserVm>>
{
    private readonly IFollow _followerService;

    public GetFollowingsQueryHandler(IFollow followerService)
    {
        _followerService = followerService;
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
        return await _followerService.GetFollowingsOfSingleUser(request.UserId!, skip, limit);
    }
}