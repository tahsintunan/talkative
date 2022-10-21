using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.TweetsForFeed
{
    public class TweetsForFeedQuery : IRequest<IList<TweetVm>>
    {
        public string? UserId { get; set; }
    }

    public class TweetForFeedQueryHandler : IRequestHandler<TweetsForFeedQuery, IList<TweetVm>>
    {
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetMapper;

        public TweetForFeedQueryHandler(
            ITweetService tweetService,
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
            var tweets = await _tweetService.GenerateFeed(request.UserId!);

            IList<TweetVm> result = new List<TweetVm>();

            foreach (var tweet in tweets)
            {
                result.Add(_tweetMapper.map(tweet));
            }
            return result;
        }
    }
}
