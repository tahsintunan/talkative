using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tweets.Queries.GetTweetById
{
    public class GetTweetByIdQuery : IRequest<TweetVm?>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public string? Id { get; set; }
    }

    public class GetTweetByIdQueryHandler : IRequestHandler<GetTweetByIdQuery, TweetVm?>
    {
        private readonly ITweet _tweetService;
        private readonly IBlockFilter _blockFilter;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonMapper;

        public GetTweetByIdQueryHandler(
            ITweet tweetService,
            IBlockFilter blockFilter,
            IBsonDocumentMapper<TweetVm> tweetBsonMapper
        )
        {
            _blockFilter = blockFilter;
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
            var tweetVm = _tweetBsonMapper.map(tweetBson);
            var blockedUsers = await _blockFilter.GetBlockedUserIds(request.UserId!);
            if (tweetVm == null || _blockFilter.IsBlocked(tweetVm, blockedUsers))
            {
                return null;
            }
            return tweetVm;
        }
    }
}
