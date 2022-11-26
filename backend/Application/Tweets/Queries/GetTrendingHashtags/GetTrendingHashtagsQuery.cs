using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.GetTrendingHashtags;

public class GetTrendingHashtagsQuery : IRequest<IList<TrendingHashtagVm>>
{
}

public class GetTrendingHashtagsQueryHandler
    : IRequestHandler<GetTrendingHashtagsQuery, IList<TrendingHashtagVm>>
{
    private readonly ITweet _tweetService;

    public GetTrendingHashtagsQueryHandler(ITweet tweetService)
    {
        _tweetService = tweetService;
    }

    public Task<IList<TrendingHashtagVm>> Handle(
        GetTrendingHashtagsQuery request,
        CancellationToken cancellationToken
    )
    {
        return _tweetService.GetTrendingHashtags();
    }
}