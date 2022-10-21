using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Services
{
    public class TweetService : ITweetService
    {
        private readonly IMongoCollection<Tweet> _tweetCollection;
        private readonly IMongoCollection<Follower> _followerCollection;

        public TweetService(
            IOptions<TweetDatabaseConfig> tweetDatabaseConfig,
            IOptions<FollowerDatabaseConfig> followerDatabaseConfig
        )
        {
            var mongoClient = new MongoClient(tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(tweetDatabaseConfig.Value.DatabaseName);

            _tweetCollection = mongoDatabase.GetCollection<Tweet>(
                tweetDatabaseConfig.Value.TweetCollectionName
            );

            _followerCollection = mongoDatabase.GetCollection<Follower>(
                followerDatabaseConfig.Value.CollectionName
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

        public async Task<IList<BsonDocument>> GenerateFeed(string userId)
        {
            var tweets = await _followerCollection
                .Aggregate()
                .Match(x => x.FollowerId == userId)
                .Lookup("tweets", "followingId", "userId", "tweet")
                .Unwind("tweet")
                .ReplaceRoot<Tweet>("$tweet")
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

        public async Task<IList<BsonDocument>> GetTweetsByHashtag(string hashtag)
        {
            var tweets = await _tweetCollection
                .Aggregate()
                .Match(
                    new BsonDocument
                    {
                        {
                            "hashtags",
                            new BsonDocument { { "$regex", hashtag }, { "$options", "i" } }
                        }
                    }
                )
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
