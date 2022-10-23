using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services
{
    public class RetweetService : IRetweet
    {
        private readonly IMongoCollection<Tweet> _tweetCollection;

        public RetweetService(IOptions<TweetDatabaseConfig> tweetDatabaseConfig)
        {
            var mongoClient = new MongoClient(tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(tweetDatabaseConfig.Value.DatabaseName);

            _tweetCollection = mongoDatabase.GetCollection<Tweet>(
                tweetDatabaseConfig.Value.TweetCollectionName
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
                .Skip(skip)
                .Limit(limit)
                .Lookup("users", "retweetUsers", "_id", "users")
                .Unwind("users")
                .ReplaceRoot<User>("$users")
                .Project(user => new UserVm() { UserId = user.Id, Username = user.Username })
                .ToListAsync();
        }

        public async Task<IList<BsonDocument>> GetQuoteRetweets(string tweetId, int skip, int limit)
        {
            return await _tweetCollection
                .Aggregate()
                .Match(x => x.Id == tweetId)
                .Skip(skip)
                .Limit(limit)
                .Lookup("tweets", "quoteRetweets", "_id", "tweets")
                .Unwind("tweets")
                .ReplaceRoot<BsonDocument>("$tweets")
                .ToListAsync();
        }
    }
}
