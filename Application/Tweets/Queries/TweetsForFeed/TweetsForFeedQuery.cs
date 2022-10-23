using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.TweetsForFeed
{
    public class TweetsForFeedQuery : IRequest<IList<TweetVm>>
    {
        public string? UserId { get; set; }
        public int? PageNumber { get; set; }
        public int? ItemCount { get; set; }
    }

    public class TweetForFeedQueryHandler : IRequestHandler<TweetsForFeedQuery, IList<TweetVm>>
    {
        private readonly ITweet _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetMapper;

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

            IList<TweetVm> result = new List<TweetVm>();

            foreach (var tweet in tweets)
            {
                result.Add(_tweetMapper.map(tweet));
            }
            return result;
        }
    }
}
