using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Retweets.Query.GetRetweetUsers;

public class GetRetweetUsersQuery : IRequest<IList<UserVm>>
{
    [JsonIgnore] public string? UserId { get; set; }

    public string? OriginalTweetId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetRetweetUsersQueryHandler : IRequestHandler<GetRetweetUsersQuery, IList<UserVm>>
{
    private readonly IBlockFilter _blockFilter;
    private readonly IRetweet _retweetService;

    public GetRetweetUsersQueryHandler(IRetweet retweetService, IBlockFilter blockFilter)
    {
        _retweetService = retweetService;
        _blockFilter = blockFilter;
    }

    public async Task<IList<UserVm>> Handle(
        GetRetweetUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;

        var retweetUsers = await _retweetService.GetRetweetUsers(request.OriginalTweetId!, skip, limit);

        return await _blockFilter.GetFilteredUsers(retweetUsers, request.UserId!);
    }
}