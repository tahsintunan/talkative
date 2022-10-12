using Application.ViewModels;
using MediatR;
using MongoDB.Bson;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Commands.RetweetCommand
{
    public class RetweetCommand:IRequest
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? RetweetId { get; set; }
        public bool IsRetweet { get; set; }
        public string? UserId { get; set; }
    }

    public class RetweetCommandHandler : IRequestHandler<RetweetCommand>
    {
        private readonly ITweetService _tweetService;

        public RetweetCommandHandler(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        public async Task<Unit> Handle(RetweetCommand request, CancellationToken cancellationToken)
        {
            var tweet = await _tweetService.GetTweetById(request.RetweetId!);
            TweetVm retweetVm = GetTweetFromBsonDocument(tweet!);

            if (request.IsRetweet)
            {
                var newRetweet = await CreateNewRetweet(request);
                await UpdateExistingTweet(retweetVm, newRetweet, request);
            }
            else
            {
                await _tweetService.DeleteRetweet(request.RetweetId!, request.UserId!);
                await UpdateExistingTweet(retweetVm, request.Id!, request);
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
                RetweetUsers = tweet.GetValue("retweetUsers", null)?.AsBsonArray.Select(p => p.ToString()).ToList(),
                RetweetPosts = tweet.GetValue("retweetPosts", null)?.AsBsonArray.Select(p => p.ToString()).ToList()
            };
        }


        private async Task<string> CreateNewRetweet(RetweetCommand request)
        {
            Tweet tweet = new Tweet()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = request.Text,
                Hashtags = request.Hashtags,
                RetweetId = request.RetweetId,
                IsRetweet = true,
                UserId = request.UserId,
                Likes = new List<string>(),
                Comments = new List<string>(),  
                RetweetPosts = new List<string>(),
                RetweetUsers = new List<string>(),
                CreatedAt = DateTime.Now,

            };

            await _tweetService.PublishTweet(tweet);
            return tweet.Id;
        }

        private async Task UpdateExistingTweet(TweetVm retweet, string postId, RetweetCommand request)
        {
            if (request.IsRetweet)
            {
                retweet.RetweetUsers!.Add(retweet.UserId);
                retweet.RetweetPosts!.Add(postId);
            }
            else
            {
                retweet.RetweetUsers!.Remove(retweet.UserId);
                retweet.RetweetPosts!.Remove(postId);
            }

            Tweet updatedTweet = new Tweet()
            {
                Id = retweet.Id,
                Text = retweet.Text,
                Hashtags = retweet.Hashtags,
                UserId = retweet.UserId,
                IsRetweet = retweet.IsRetweet,
                RetweetId = retweet.RetweetId,
                Likes = new List<string>(retweet.Likes!),
                Comments = new List<string>(retweet.Comments!),
                RetweetPosts = new List<string>(retweet.RetweetPosts!),
                RetweetUsers = new List<string>(retweet.RetweetUsers!),
                CreatedAt = retweet.CreatedAt
            };

            await _tweetService.UpdateTweet(updatedTweet);

        }
    }
}
