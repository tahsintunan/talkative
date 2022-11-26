using Application.Common.Interface;
using Application.Common.ViewModels;
using MongoDB.Bson;

namespace Application.Common.Mapper;

public class TweetBsonDocumentMapper : IBsonDocumentMapper<TweetVm?>
{
    private readonly IBsonDocumentMapper<UserVm> _userMapper;

    public TweetBsonDocumentMapper(IBsonDocumentMapper<UserVm> userMapper)
    {
        _userMapper = userMapper;
    }

    public TweetVm? map(BsonDocument tweet)
    {
        if (!tweet.Contains("_id") || tweet["_id"].BsonType == BsonType.Null) return null;
        return new TweetVm
        {
            Id = CheckIfDocumentExists(tweet, "_id") ? tweet["_id"].ToString() : null,
            Text = CheckIfDocumentExists(tweet, "text") ? tweet["text"].ToString() : null,
            Hashtags = CheckIfDocumentExists(tweet, "hashtags")
                ? tweet["hashtags"].AsBsonArray.Select(p => p.AsString).ToArray()
                : null,
            IsRetweet = CheckIfDocumentExists(tweet, "isRetweet")
                ? tweet["isRetweet"].AsBoolean
                : false,
            IsQuoteRetweet = CheckIfDocumentExists(tweet, "isQuoteRetweet")
                ? tweet["isQuoteRetweet"].AsBoolean
                : false,
            OriginalTweet = CheckIfDocumentExists(tweet, "originalTweet")
                ? map(tweet["originalTweet"].AsBsonDocument)
                : null,
            UserId = CheckIfDocumentExists(tweet, "userId") ? tweet["userId"].ToString() : null,
            OriginalTweetId = CheckIfDocumentExists(tweet, "originalTweetId")
                ? tweet["originalTweetId"].ToString()
                : null,
            User = CheckIfDocumentExists(tweet, "user")
                ? _userMapper.map(tweet["user"].AsBsonDocument)
                : null,
            CreatedAt = CheckIfDocumentExists(tweet, "createdAt")
                ? tweet["createdAt"].ToUniversalTime()
                : null,
            LastModified = CheckIfDocumentExists(tweet, "lastModified")
                ? tweet["lastModified"].ToUniversalTime()
                : null,
            Likes = CheckIfDocumentExists(tweet, "likes")
                ? tweet.GetValue("likes", null)?.AsBsonArray.Select(p => p.ToString()).ToList()
                : null,
            Comments = CheckIfDocumentExists(tweet, "comments")
                ? tweet
                    .GetValue("comments", null)
                    ?.AsBsonArray.Select(p => p.ToString())
                    .ToList()
                : null,
            QuoteRetweets = CheckIfDocumentExists(tweet, "quoteRetweets")
                ? tweet
                    .GetValue("quoteRetweets", null)
                    ?.AsBsonArray.Select(p => p.ToString())
                    .ToList()
                : null,
            RetweetUsers = CheckIfDocumentExists(tweet, "retweetUsers")
                ? tweet
                    .GetValue("retweetUsers", null)
                    ?.AsBsonArray.Select(p => p.ToString())
                    .ToList()
                : null
        };
    }

    public bool CheckIfDocumentExists(BsonDocument document, string documentKey)
    {
        return document.Contains(documentKey)
               && document[documentKey].BsonType != BsonType.Null;
    }
}