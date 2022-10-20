using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public class TweetService : ITweetService
    {
        private readonly IMongoCollection<Tweet> _tweetCollection;

        public TweetService(
            IOptions<TweetDatabaseConfig> tweetDatabaseConfig,
            IOptions<UserDatabaseConfig> userDatabaseConfig
        )
        {
            var mongoClient = new MongoClient(tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(tweetDatabaseConfig.Value.DatabaseName);

            _tweetCollection = mongoDatabase.GetCollection<Tweet>(
                tweetDatabaseConfig.Value.TweetCollectionName
            );

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(
                tweetDatabaseConfig.Value.ConnectionString
            );

            settings.LinqProvider = LinqProvider.V3;
        }

        public async Task PublishTweet(Tweet tweet)
        {
            await _tweetCollection.InsertOneAsync(tweet);
        }

        public async Task DeleteTweet(string id)
        {
            await _tweetCollection.DeleteOneAsync(tweet => tweet.Id == id);
        }

        public async Task<BsonDocument?> GetTweetById(string id)
        {
            var tweet = await _tweetCollection
                .Aggregate()
                .Match(x => x.Id == id)
                .Lookup("users", "userId", "_id", "user")
                .Lookup("tweets", "retweetId", "_id", "retweet")
                .Unwind(
                    "retweet",
                    new AggregateUnwindOptions<TweetVm>() { PreserveNullAndEmptyArrays = true }
                )
                .Unwind("user")
                .Lookup("users", "retweet.userId", "_id", "retweet.user")
                .Unwind(
                    "retweet.user",
                    new AggregateUnwindOptions<BsonDocument>() { PreserveNullAndEmptyArrays = true }
                )
                .FirstOrDefaultAsync();

            return tweet;
        }

        public Task GetTweetsOfFollowing()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTweet(Tweet updatedTweet)
        {
            await _tweetCollection.ReplaceOneAsync(x => x.Id == updatedTweet.Id, updatedTweet);
        }

        public async Task PartialUpdate(string tweetId, UpdateDefinition<Tweet> update)
        {
            await _tweetCollection.UpdateOneAsync(
                p => p.Id == tweetId,
                update,
                new UpdateOptions { IsUpsert = true }
            );
        }

        public async Task<IList<BsonDocument>> GetTweetsOfSingleUser(string userId)
        {
            var tweets = await _tweetCollection
                .Aggregate()
                .Match(x => x.UserId == userId)
                .Lookup("users", "userId", "_id", "user")
                .Lookup("tweets", "retweetId", "_id", "retweet")
                .Unwind(
                    "retweet",
                    new AggregateUnwindOptions<TweetVm>() { PreserveNullAndEmptyArrays = true }
                )
                .Unwind("user")
                .Lookup("users", "retweet.userId", "_id", "retweet.user")
                .Unwind(
                    "retweet.user",
                    new AggregateUnwindOptions<BsonDocument>() { PreserveNullAndEmptyArrays = true }
                )
                .ToListAsync();

            return tweets;
        }
    }
}
