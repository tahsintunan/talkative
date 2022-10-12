using Application.Interface;
using MediatR;
using MongoDB.Bson;
using server.Application.Interface;
using server.Application.ViewModels;

namespace Application.Tweets.Queries.GetTweetsOfSingleUserQuery
{
    public class GetTweetsOfSingleUserQuery:IRequest<IList<TweetVm>>
    {
        public string? UserId { get; set; }
    }

    public class GetTweetsOfSingleUserQueryHandler : IRequestHandler<GetTweetsOfSingleUserQuery, IList<TweetVm>>
    {
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetMapper;
        public GetTweetsOfSingleUserQueryHandler(ITweetService tweetService, IBsonDocumentMapper<TweetVm> tweetMapper)
        {
            _tweetService = tweetService;
            _tweetMapper = tweetMapper;
        }

        public async Task<IList<TweetVm>> Handle(GetTweetsOfSingleUserQuery request, CancellationToken cancellationToken)
        {
            var tweets = await _tweetService.GetTweetsOfSingleUser(request.UserId!);

            IList<TweetVm> result = new List<TweetVm>();

            foreach (var tweet in tweets)
            {
                result.Add(_tweetMapper.map(tweet));
            }
            return result;
        }

    }
}
