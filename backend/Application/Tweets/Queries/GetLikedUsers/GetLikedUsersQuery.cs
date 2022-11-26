using Application.Common.Interface;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Tweets.Queries.GetLikedUsers;

public class GetLikedUsersQuery : IRequest<IList<UserVm>>
{
    public string? TweetId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetLikedUsersQueryHandler : IRequestHandler<GetLikedUsersQuery, IList<UserVm>>
{
    private readonly IMapper _mapper;
    private readonly ITweet _tweetService;

    public GetLikedUsersQueryHandler(ITweet tweetService, IMapper mapper)
    {
        _tweetService = tweetService;
        _mapper = mapper;
    }

    public async Task<IList<UserVm>> Handle(GetLikedUsersQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;

        var likedUsers = await _tweetService.GetLikedUsers(request.TweetId!, skip, limit);
        IList<UserVm> likedUserVmList = _mapper.Map<List<UserVm>>(likedUsers);
        return likedUserVmList;
    }
}