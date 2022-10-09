using Application.Tweets.Queries.GetTweetByIdQuery;
using Application.ViewModels;
using AutoMapper;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Commands.UpdateTweetCommand
{
    public class UpdateTweetCommand:IRequest<TweetVm?>
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? UserId { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetId { get; set; }
    }

    public class UpdateTweetCommandHandler : IRequestHandler<UpdateTweetCommand, TweetVm?>
    {
        private readonly ITweetService _tweetService;
        private readonly IMediator _mediator;
        public UpdateTweetCommandHandler(ITweetService tweetService, IMediator mediator)
        {
            _tweetService = tweetService;
            _mediator = mediator;
        }

        public async Task<TweetVm?> Handle(UpdateTweetCommand request, CancellationToken cancellationToken)
        {
            var currentTweet = await _tweetService.GetTweetById(request.Id!);
            var tweetVm = GetTweetFromBsonDocument(currentTweet!);
            if(currentTweet != null && request.UserId == tweetVm.UserId)
            {
                Tweet updatedTweet = new Tweet()
                {
                    Id = request.Id,
                    Text = request.Text ?? tweetVm.Text,
                    Hashtags = request.Hashtags ?? tweetVm.Hashtags,
                    UserId = ObjectId.Parse(request.UserId),
                    IsRetweet = tweetVm.IsRetweet,
                    RetweetId = tweetVm.IsRetweet ? ObjectId.Parse(tweetVm.RetweetId): null,
                    Likes = new List<string>(tweetVm.Likes!),
                    Comments = new List<string>(tweetVm.Comments!),
                    CreatedAt = tweetVm.CreatedAt
                };

                await _tweetService.UpdateTweet(updatedTweet);
                return await _mediator.Send(new GetTweetByIdQuery() { Id = request.Id});
            }
            return tweetVm;
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
                Retweet = tweet["isRetweet"].AsBoolean ? GetTweetFromBsonDocument(tweet["retweet"].AsBsonDocument) : null,
                User = GetUserFromBsonValue(tweet["user"].AsBsonDocument),
                UserId = tweet["userId"].ToString(),
                RetweetId = tweet["isRetweet"].AsBoolean ? tweet["retweetId"].ToString() : null,
                CreatedAt = tweet["createdAt"].ToUniversalTime(),
                Likes = tweet.GetValue("likes", null)?.AsBsonArray.Select(p => p.AsString).ToArray(),
                Comments = tweet.GetValue("comments", null)?.AsBsonArray.Select(p => p.AsString).ToArray()
            };
        }
    }
}
