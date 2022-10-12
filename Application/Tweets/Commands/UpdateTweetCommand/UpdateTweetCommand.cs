using Application.Tweets.Queries.GetTweetByIdQuery;
using Application.ViewModels;
using MediatR;
using MongoDB.Bson;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Commands.UpdateTweetCommand
{
    public class UpdateTweetCommand:IRequest
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? UserId { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetId { get; set; }
        public string? RetweetUser { get; set; }
    }

    public class UpdateTweetCommandHandler : IRequestHandler<UpdateTweetCommand>
    {
        private readonly ITweetService _tweetService;
        public UpdateTweetCommandHandler(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        public async Task<Unit> Handle(UpdateTweetCommand request, CancellationToken cancellationToken)
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
                    UserId = request.UserId,
                    IsRetweet = tweetVm.IsRetweet,
                    RetweetId = tweetVm.IsRetweet ? request.UserId : null,
                    Likes = new List<string>(tweetVm.Likes!),
                    Comments = new List<string>(tweetVm.Comments!),
                    RetweetPosts = new List<string>(tweetVm.RetweetPosts!),
                    RetweetUsers = new List<string>(tweetVm.RetweetUsers!),
                    CreatedAt = tweetVm.CreatedAt
                };

                await _tweetService.UpdateTweet(updatedTweet);
            }
            return Unit.Value;
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
                Likes = tweet.GetValue("likes", null)?.AsBsonArray.Select(p => p.ToString()).ToList(),
                Comments = tweet.GetValue("comments", null)?.AsBsonArray.Select(p => p.ToString()).ToList(),
                RetweetUsers = tweet.GetValue("retweetUsers", null)?.AsBsonArray.Select(p => p.ToString()).ToList()
            };
        }
    }
}
