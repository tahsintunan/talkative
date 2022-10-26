using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;
using MongoDB.Bson;

namespace Application.Tweets.Queries.GetTweetsOfSingleUser
{
    public class GetTweetsOfSingleUserQuery : IRequest<IList<TweetVm>>
    {
        public int? PageNumber { get; set; }
        public int? ItemCount { get; set; }
        public string? UserId { get; set; }
        [JsonIgnore]
        public string? CurrentUserId { get; set; }
    }

    public class GetTweetsOfSingleUserQueryHandler
        : IRequestHandler<GetTweetsOfSingleUserQuery, IList<TweetVm>>
    {
        private readonly ITweet _tweetService;
        private readonly IBlockFilter _blockFilter;
        private readonly IBsonDocumentMapper<TweetVm> _tweetMapper;

        public GetTweetsOfSingleUserQueryHandler(
            ITweet tweetService,
            IBlockFilter blockFilter,
            IBsonDocumentMapper<TweetVm> tweetMapper
        )
        {
            _blockFilter = blockFilter;
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

            foreach (var tweet in tweets)
            {
                result.Add(_tweetMapper.map(tweet));
            }
            if (request.CurrentUserId == request.UserId)
            {
                return result;
            }
            return await _blockFilter.GetFilteredTweets(result, request.CurrentUserId!);
        }
    }
}
