using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.GetTweetsByHashtag
{
    public class GetTweetsByHashtagQuery : IRequest<IList<TweetVm>>
    {
        public string? Hashtag { get; set; }
    }

    public class GetTweetsByHashtagQueryHandler
        : IRequestHandler<GetTweetsByHashtagQuery, IList<TweetVm>>
    {
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonMapper;

        public GetTweetsByHashtagQueryHandler(
            ITweetService tweetService,
            IBsonDocumentMapper<TweetVm> tweetBsonMapper
        )
        {
            _tweetService = tweetService;
            _tweetBsonMapper = tweetBsonMapper;
        }

        public async Task<IList<TweetVm>> Handle(
            GetTweetsByHashtagQuery request,
            CancellationToken cancellationToken
        )
        {
            var tweets = await _tweetService.GetTweetsByHashtag(request.Hashtag!);

            var tweetVmList = new List<TweetVm>();

            foreach (var tweet in tweets)
            {
                tweetVmList.Add(_tweetBsonMapper.map(tweet));
            }

            return tweetVmList;
        }
    }
}
