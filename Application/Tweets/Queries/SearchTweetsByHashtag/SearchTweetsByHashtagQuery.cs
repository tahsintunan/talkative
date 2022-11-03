using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.SearchTweetsByHashtag;

public class SearchTweetsByHashtagQuery : IRequest<IList<TweetVm>>
{
    public string? Hashtag { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetTweetsByHashtagQueryHandler
    : IRequestHandler<SearchTweetsByHashtagQuery, IList<TweetVm>>
{
    private readonly IBsonDocumentMapper<TweetVm> _tweetBsonMapper;
    private readonly ITweet _tweetService;

    public GetTweetsByHashtagQueryHandler(
        ITweet tweetService,
        IBsonDocumentMapper<TweetVm> tweetBsonMapper
    )
    {
        _tweetService = tweetService;
        _tweetBsonMapper = tweetBsonMapper;
    }

    public async Task<IList<TweetVm>> Handle(
        SearchTweetsByHashtagQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        var tweets = await _tweetService.GetTweetsByHashtag(request.Hashtag!, skip, limit);

        var tweetVmList = new List<TweetVm>();

        foreach (var tweet in tweets) tweetVmList.Add(_tweetBsonMapper.map(tweet));

        return tweetVmList;
    }
}