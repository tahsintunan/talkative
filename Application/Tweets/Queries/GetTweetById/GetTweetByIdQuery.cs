using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.GetTweetById
{
    public class GetTweetByIdQuery : IRequest<TweetVm?>
    {
        public string? Id { get; set; }
    }

    public class GetTweetByIdQueryHandler : IRequestHandler<GetTweetByIdQuery, TweetVm?>
    {
        private readonly ITweet _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonMapper;

        public GetTweetByIdQueryHandler(
            ITweet tweetService,
            IBsonDocumentMapper<TweetVm> tweetBsonMapper
        )
        {
            _tweetService = tweetService;
            _tweetBsonMapper = tweetBsonMapper;
        }

        public async Task<TweetVm?> Handle(
            GetTweetByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var tweetBson = await _tweetService.GetTweetById(request.Id!);
            if (tweetBson == null)
            {
                return null;
            }
            var tweet = _tweetBsonMapper.map(tweetBson);
            return tweet;
        }
    }
}
