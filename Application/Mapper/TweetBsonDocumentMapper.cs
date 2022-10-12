using Application.Interface;
using Application.ViewModels;
using MongoDB.Bson;
using server.Application.ViewModels;

namespace Application.Mapper
{
    public class TweetBsonDocumentMapper : IBsonDocumentMapper<TweetVm>
    {
        private readonly IBsonDocumentMapper<UserVm> _userMapper;
        public TweetBsonDocumentMapper(IBsonDocumentMapper<UserVm> userMapper)
        {
            _userMapper = userMapper;
        }

        public TweetVm map(BsonDocument tweet)
        {
            if (!tweet.Contains("_id"))
            {
                return null;
            }
            return new TweetVm()
            {
                Id = tweet.Contains("_id") ? tweet["_id"].ToString() : null,
                Text = tweet.Contains("text") ? tweet["text"].ToString() : null,
                Hashtags = tweet.Contains("hashtags") ? tweet["hashtags"].AsBsonArray.Select(p => p.AsString).ToArray() : null,
                IsRetweet = tweet.Contains("isRetweet") ? tweet["isRetweet"].AsBoolean : false,
                Retweet = tweet.Contains("retweet") ? tweet["isRetweet"].AsBoolean ? map(tweet["retweet"].AsBsonDocument) : null : null,
                User = tweet.Contains("user") ? _userMapper.map(tweet["user"].AsBsonDocument) : null,
                CreatedAt = tweet.Contains("createdAt") ? tweet["createdAt"].ToUniversalTime() : null,
                Likes = tweet.Contains("likes") ? tweet.GetValue("likes", null)?.AsBsonArray.Select(p => p.AsString).ToArray() : null,
                Comments = tweet.Contains("comments") ? tweet.GetValue("comments", null)?.AsBsonArray.Select(p => p.AsString).ToArray() : null,
                RetweetPosts = tweet.Contains("retweetPosts") ? tweet.GetValue("retweetPosts", null)?.AsBsonArray.Select(p => p.ToString()).ToArray() : null,
                RetweetUsers = tweet.Contains("retweetUsers") ? tweet.GetValue("retweetUsers", null)?.AsBsonArray.Select(p => p.ToString()).ToArray() : null,
            };
        }
    }
}
