using AutoMapper;
using MediatR;
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
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GetTweetsOfSingleUserQueryHandler(ITweetService tweetService,IMapper mapper, IUserService userService)
        {
            _tweetService = tweetService;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IList<TweetVm>> Handle(GetTweetsOfSingleUserQuery request, CancellationToken cancellationToken)
        {
            var tweets = await _tweetService.GetTweetsOfSingleUser(request.UserId!);
            IList<TweetVm> tweetVms = _mapper.Map<IList<TweetVm>>(tweets);

            foreach (var tweet in tweetVms)
            {
                var currentUser = await _userService.GetUserById(tweet.UserId!);
                tweet.Username = currentUser.Username;
                if (tweet.IsRetweet)
                {
                    var retweetVm = _mapper.Map<TweetVm>(await _tweetService.GetTweetById(tweet.RetweetId!));
                    var originalPoster = await _userService.GetUserById(tweet.UserId!);
                    retweetVm.Username = originalPoster.Username;
                    tweet.Retweet = retweetVm;
                }
            }
            return tweetVms;
        }
    }
}
