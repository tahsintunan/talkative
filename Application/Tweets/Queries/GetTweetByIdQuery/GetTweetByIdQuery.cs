using AutoMapper;
using MediatR;
using server.Application.Interface;
using server.Application.ViewModels;

namespace Application.Tweets.Queries.GetTweetByIdQuery
{
    public class GetTweetByIdQuery:IRequest<TweetVm?>
    {
        public string? Id { get; set; }
    }

    public class GetTweetByIdQueryHandler : IRequestHandler<GetTweetByIdQuery, TweetVm?>
    {
        private readonly ITweetService _tweetService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public GetTweetByIdQueryHandler(ITweetService tweetService, IUserService userService, IMapper mapper)
        {
            _tweetService = tweetService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<TweetVm?> Handle(GetTweetByIdQuery request, CancellationToken cancellationToken)
        {
            var tweet = await _tweetService.GetTweetById(request.Id!);
            if(tweet != null)
            {
                var tweetVm = _mapper.Map<TweetVm>(tweet);
                var user = await _userService.GetUserById(tweet.UserId!);
                tweetVm.Username = user.Username;
                if (tweetVm.IsRetweet)
                {
                    var retweet = await _tweetService.GetTweetById(tweet.RetweetId!);
                    var retweetVm = _mapper.Map<TweetVm>(retweet);
                    var originalPoster = await _userService.GetUserById(retweetVm.UserId!);
                    retweetVm.Username = originalPoster.Username;
                    tweetVm.Retweet = retweetVm;
                }
                return tweetVm;
            }
            return null;
        }
    }
}
