using Application.ViewModels;
using MediatR;
using MongoDB.Bson;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Commands.LikeTweetCommand
{
    public class LikeTweetCommand:IRequest
    {
        public string? TweetId { get; set; }
        public string? UserId { get; set; }
        public bool IsLiked { get; set; }
    }

    public class LikeTweetCommandHandler : IRequestHandler<LikeTweetCommand>
    {

        private readonly ITweetService _tweetService;
        public LikeTweetCommandHandler(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        public async Task<Unit> Handle(LikeTweetCommand request, CancellationToken cancellationToken)
        {
            var currentTweet = await _tweetService.GetTweetById(request.TweetId!);
            var tweetVm = GetTweetFromBsonDocument(currentTweet!);

            if (request.IsLiked)
            {
                tweetVm.Likes!.Add(request.UserId);
            }
            else
            {
                tweetVm.Likes!.Remove(request.UserId);
            }


            if (currentTweet != null)
            {

                Tweet updatedTweet = new Tweet()
                {
                    Id = tweetVm.Id,
                    Text =  tweetVm.Text,
                    Hashtags = tweetVm.Hashtags,
                    UserId = request.UserId,
                    IsRetweet = tweetVm.IsRetweet,
                    RetweetId = tweetVm.IsRetweet ? tweetVm.RetweetId : null,
                    Likes = tweetVm.Likes!,
                    Comments = new List<string>(tweetVm.Comments!),
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
                Comments = tweet.GetValue("comments", null)?.AsBsonArray.Select(p => p.ToString()).ToList()
            };
        }
    }
}
