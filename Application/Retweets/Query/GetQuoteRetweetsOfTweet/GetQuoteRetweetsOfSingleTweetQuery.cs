using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Retweets.Query.GetQuoteRetweetsOfTweet;

public class GetQuoteRetweetsOfSingleTweetQuery : IRequest<IList<TweetVm>>
{
    [JsonIgnore] public string? UserId { get; set; }

    public string? OriginalTweetId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetAllRetweetQueryHandler
    : IRequestHandler<GetQuoteRetweetsOfSingleTweetQuery, IList<TweetVm>>
{
    private readonly IBlockFilter _blockFilter;
    private readonly IBsonDocumentMapper<TweetVm> _documentMapper;
    private readonly IRetweet _retweetService;

    public GetAllRetweetQueryHandler(
        IRetweet retweetService,
        IBlockFilter blockFilter,
        IBsonDocumentMapper<TweetVm> documentMapper
    )
    {
        _blockFilter = blockFilter;
        _retweetService = retweetService;
        _documentMapper = documentMapper;
    }

    public async Task<IList<TweetVm>> Handle(
        GetQuoteRetweetsOfSingleTweetQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        var retweets = await _retweetService.GetQuoteRetweets(
            request.OriginalTweetId!,
            skip,
            limit
        );

        IList<TweetVm> tweetVmList = new List<TweetVm>();

        foreach (var tweet in retweets) tweetVmList.Add(_documentMapper.map(tweet.AsBsonDocument));

        return await _blockFilter.GetFilteredTweets(tweetVmList, request.UserId!);
    }
}