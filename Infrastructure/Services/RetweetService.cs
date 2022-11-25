using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class RetweetService : IRetweet
{
    private readonly IMongoCollection<Tweet> _tweetCollection;

    public RetweetService()
    {
        var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("ConnectionString"));
        var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));

        _tweetCollection = mongoDatabase.GetCollection<Tweet>(
            Environment.GetEnvironmentVariable("TweetCollectionName")
        );
    }

    public async Task UndoRetweet(string originalTweetId, string userId)
    {
        await _tweetCollection.DeleteManyAsync(
            tweet =>
                tweet.IsRetweet
                && tweet.OriginalTweetId == originalTweetId
                && tweet.UserId == userId
        );
    }

    public async Task<IList<UserVm>> GetRetweetUsers(string tweetId, int skip, int limit)
    {
        return await _tweetCollection
            .Aggregate()
            .Match(x => x.Id == tweetId)
            .Limit(limit)
            .Skip(skip)
            .Lookup("users", "retweetUsers", "_id", "users")
            .Unwind("users")
            .ReplaceRoot<User>("$users")
            .Project(user => new UserVm
            { UserId = user.Id, Username = user.Username, ProfilePicture = user.ProfilePicture })
            .ToListAsync();
    }

    public async Task<IList<BsonDocument>> GetQuoteRetweets(string tweetId, int skip, int limit)
    {
        return await _tweetCollection
            .Aggregate()
            .Match(x => x.Id == tweetId)
            .Limit(limit)
            .Skip(skip)
            .Lookup("tweets", "quoteRetweets", "_id", "tweets")
            .Unwind("tweets")
            .ReplaceRoot<Tweet>("$tweets")
            .SortByDescending(x => x.CreatedAt)
            .Limit(limit)
            .Skip(skip)
            .Lookup("users", "userId", "_id", "user")
            .Lookup("tweets", "originalTweetId", "_id", "originalTweet")
            .Unwind(
                "originalTweet",
                new AggregateUnwindOptions<TweetVm> { PreserveNullAndEmptyArrays = true }
            )
            .Unwind("user")
            .Lookup("users", "originalTweet.userId", "_id", "originalTweet.user")
            .Unwind(
                "originalTweet.user",
                new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true }
            )
            .Lookup(
                "tweets",
                "originalTweet.originalTweetId",
                "_id",
                "originalTweet.originalTweet"
            )
            .Unwind(
                "originalTweet.originalTweet",
                new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true }
            )
            .Lookup(
                "users",
                "originalTweet.originalTweet.userId",
                "_id",
                "originalTweet.originalTweet.user"
            )
            .Unwind(
                "originalTweet.originalTweet.user",
                new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true }
            )
            .ToListAsync();
    }
}