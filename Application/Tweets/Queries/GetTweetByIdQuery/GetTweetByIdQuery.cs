using Application.ViewModels;
using AutoMapper;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

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
            var tweetBson = await _tweetService.GetTweetById(request.Id!);
            if(tweetBson == null)
            {
                return null;
            }
            var tweet = GetTweetFromBsonDocument(tweetBson);
            return tweet;
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
            
            return new TweetVm()
            {
                Id = tweet["_id"].ToString(),
                Text = tweet["text"].ToString(),
                Hashtags = tweet["hashtags"].AsBsonArray.Select(p => p.AsString).ToArray(),
                IsRetweet = tweet["isRetweet"].AsBoolean,
                Retweet = tweet["isRetweet"].AsBoolean ? GetTweetFromBsonDocument(tweet["retweet"].AsBsonDocument):null,
                User = GetUserFromBsonValue(tweet["user"].AsBsonDocument),
                CreatedAt = tweet["createdAt"].ToUniversalTime(),
                Likes = tweet.GetValue("likes", null)?.AsBsonArray.Select(p => p.AsString).ToArray(),
                Comments = tweet.GetValue("comments", null)?.AsBsonArray.Select(p => p.AsString).ToArray()
            };
        }
    }
}
