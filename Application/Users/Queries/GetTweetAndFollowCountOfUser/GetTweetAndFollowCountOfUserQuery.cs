using Application.Common.Interface;
using MediatR;

namespace Application.Users.Queries.GetTweetAndFollowCountOfUser
{
    public class GetTweetAndFollowCountOfUserQuery : IRequest<GetTweetAndFollowCountOfUserVm>
    {
        public string? UserId { get; set; }
    }

    public class GetTweetAndFollowCountOfUserQueryHandler
        : IRequestHandler<GetTweetAndFollowCountOfUserQuery, GetTweetAndFollowCountOfUserVm>
    {
        private readonly ITweet _tweetService;
        private readonly IFollow _followService;

        public GetTweetAndFollowCountOfUserQueryHandler(ITweet tweetService, IFollow followService)
        {
            _tweetService = tweetService;
            _followService = followService;
        }

        public async Task<GetTweetAndFollowCountOfUserVm> Handle(
            GetTweetAndFollowCountOfUserQuery request,
            CancellationToken cancellationToken
        )
        {
            return new GetTweetAndFollowCountOfUserVm()
            {
                TweetCount = await _tweetService.GetNumberOfTweetsOfUser(request.UserId!),
                FollowerCount = await _followService.GetNumberOfFollowerOfSingleUser(
                    request.UserId!
                ),
                FollowingCount = await _followService.GetNumberOfFollowingOfSingleUser(
                    request.UserId!
                )
            };
        }
    }
}
