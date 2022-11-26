using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.TweetsForFeed;

public class TweetsForFeedQuery : IRequest<IList<TweetVm>>
{
    [JsonIgnore] public string? UserId { get; set; }

    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class TweetForFeedQueryHandler : IRequestHandler<TweetsForFeedQuery, IList<TweetVm>>
{
    private readonly IBsonDocumentMapper<TweetVm> _tweetMapper;
    private readonly ITweet _tweetService;

    public TweetForFeedQueryHandler(
        ITweet tweetService,
        IBsonDocumentMapper<TweetVm> tweetMapper
    )
    {
        _tweetService = tweetService;
        _tweetMapper = tweetMapper;
    }

    public async Task<IList<TweetVm>> Handle(
        TweetsForFeedQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        var tweets = await _tweetService.GenerateFeed(request.UserId!, skip, limit);

        IList<TweetVm> tweetVmList = new List<TweetVm>();

        foreach (var tweet in tweets) tweetVmList.Add(_tweetMapper.map(tweet));
        return tweetVmList;
    }
}