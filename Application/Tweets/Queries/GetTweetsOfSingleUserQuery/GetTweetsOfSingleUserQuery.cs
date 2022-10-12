using Application.ViewModels;
using AutoMapper;
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

            IList<TweetVm> result = new List<TweetVm>();

            foreach (var tweet in tweets)
            {
                result.Add(GetTweetFromBsonDocument(tweet));
            }
            return result;
        }

        private UserVm GetUserFromBsonValue(BsonDocument user)
        {
            return new UserVm()
            {
                Id = user["_id"].ToString(),
                Username = user["username"].ToString(),
                Email = user["email"].ToString(),
            };
        }

        private TweetVm GetTweetFromBsonDocument(BsonDocument tweet)
        {

            try
            {
                return new TweetVm()
                {
                    Id = tweet["_id"].ToString(),
                    Text = tweet["text"].ToString(),
                    Hashtags = tweet["hashtags"].AsBsonArray.Select(p => p.AsString).ToArray(),
                    IsRetweet = tweet["isRetweet"].AsBoolean,
                    Retweet = tweet["isRetweet"].AsBoolean ? GetTweetFromBsonDocument(tweet["retweet"].AsBsonDocument) : null,
                    User = GetUserFromBsonValue(tweet["user"].AsBsonDocument),
                    CreatedAt = tweet["createdAt"].ToUniversalTime(),
                    Likes = tweet.GetValue("likes",null)?.AsBsonArray.Select(p => p.AsString).ToArray(),
                    Comments = tweet.GetValue("comments",null)?.AsBsonArray.Select(p => p.AsString).ToArray(),
                    RetweetPosts = tweet.GetValue("retweetPosts", null)?.AsBsonArray.Select(p => p.ToString()).ToArray(),
                    RetweetUsers = tweet.GetValue("retweetUsers", null)?.AsBsonArray.Select(p => p.ToString()).ToArray(),
                };
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
