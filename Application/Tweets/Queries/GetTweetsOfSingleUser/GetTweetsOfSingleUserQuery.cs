using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.GetTweetsOfSingleUser;

public class GetTweetsOfSingleUserQuery : IRequest<IList<TweetVm>>
{
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
    public string? UserId { get; set; }
}

public class GetTweetsOfSingleUserQueryHandler
    : IRequestHandler<GetTweetsOfSingleUserQuery, IList<TweetVm>>
{
    private readonly IBsonDocumentMapper<TweetVm> _tweetMapper;
    private readonly ITweet _tweetService;

    public GetTweetsOfSingleUserQueryHandler(
        ITweet tweetService,
        IBsonDocumentMapper<TweetVm> tweetMapper
    )
    {
        _tweetService = tweetService;
        _tweetMapper = tweetMapper;
    }

    public async Task<IList<TweetVm>> Handle(
        GetTweetsOfSingleUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;

        var tweets = await _tweetService.GetTweetsOfSingleUser(request.UserId!, skip, limit);

        IList<TweetVm> result = new List<TweetVm>();

        foreach (var tweet in tweets) result.Add(_tweetMapper.map(tweet));
        return result;
    }
}