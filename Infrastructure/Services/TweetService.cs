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
    public class TweetService : ITweet
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
                .Lookup("tweets", "originalTweetId", "_id", "originalTweet")
                .Unwind(
                    "originalTweet",
                    new AggregateUnwindOptions<TweetVm>() { PreserveNullAndEmptyArrays = true }
                )
                .Unwind("user")
                .Lookup("users", "originalTweet.userId", "_id", "originalTweet.user")
                .Unwind(
                    "originalTweet.user",
                    new AggregateUnwindOptions<BsonDocument>() { PreserveNullAndEmptyArrays = true }
                )
                .FirstOrDefaultAsync();

            return tweet;
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

        public async Task<IList<BsonDocument>> GetTweetsOfSingleUser(
            string userId,
            int skip,
            int limit
        )
        {
            var tweets = await _tweetCollection
                .Aggregate()
                .Match(x => x.UserId == userId)
                .SortBy(x => x.CreatedAt)
                .ThenByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Limit(limit)
                .Lookup("users", "userId", "_id", "user")
                .Lookup("tweets", "originalTweetId", "_id", "originalTweet")
                .Unwind(
                    "originalTweet",
                    new AggregateUnwindOptions<TweetVm>() { PreserveNullAndEmptyArrays = true }
                )
                .Unwind("user")
                .Lookup("users", "originalTweet.userId", "_id", "originalTweet.user")
                .Unwind(
                    "originalTweet.user",
                    new AggregateUnwindOptions<BsonDocument>() { PreserveNullAndEmptyArrays = true }
                )
                .ToListAsync();

            return tweets;
        }

        public async Task<IList<BsonDocument>> GenerateFeed(string userId, int skip, int limit)
        {
            var tweets = await _followerCollection
                .Aggregate()
                .Match(x => x.FollowerId == userId)
                .Lookup("tweets", "followingId", "userId", "tweet")
                .Unwind("tweet")
                .ReplaceRoot<Tweet>("$tweet")
                .SortBy(x => x.CreatedAt)
                .ThenByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Limit(limit)
                .Lookup("users", "userId", "_id", "user")
                .Lookup("tweets", "originalTweetId", "_id", "originalTweet")
                .Unwind(
                    "originalTweet",
                    new AggregateUnwindOptions<TweetVm>() { PreserveNullAndEmptyArrays = true }
                )
                .Unwind("user")
                .Lookup("users", "originalTweet.userId", "_id", "originalTweet.user")
                .Unwind(
                    "originalTweet.user",
                    new AggregateUnwindOptions<BsonDocument>() { PreserveNullAndEmptyArrays = true }
                )
                .ToListAsync();

            return tweets;
        }

        public async Task<IList<TrendingHashtagVm>> GetTrendingHashtags()
        {
            BsonDocument pipelineStageOne = new BsonDocument(
                "$project",
                new BsonDocument("hashtags", 1)
            );
            BsonDocument pipelineStageTwo = new BsonDocument(
                "$unwind",
                new BsonDocument("path", "$hashtags")
            );
            BsonDocument pipelineStageThree = new BsonDocument(
                "$group",
                new BsonDocument
                {
                    { "_id", "$hashtags" },
                    { "count", new BsonDocument("$sum", 1) }
                }
            );
            BsonDocument pipelineStageFour = new BsonDocument(
                "$sort",
                new BsonDocument("count", -1)
            );

            var hashtags = await _tweetCollection
                .Aggregate<BsonDocument>(
                    new BsonDocument[]
                    {
                        pipelineStageOne,
                        pipelineStageTwo,
                        pipelineStageThree,
                        pipelineStageFour
                    }
                )
                .ToListAsync();
            IList<TrendingHashtagVm> trendingHashtagVms = new List<TrendingHashtagVm>();

            foreach (var hashtag in hashtags)
            {
                trendingHashtagVms.Add(
                    new TrendingHashtagVm()
                    {
                        Hashtag = hashtag.AsBsonDocument.GetElement("_id").Value.ToString(),
                        Count = hashtag.AsBsonDocument.GetElement("count").Value.ToInt64(),
                    }
                );
            }

            return trendingHashtagVms;
        }

        public async Task<IList<string>> SearchHashtags(string hashtag, int skip, int limit)
        {
            var hashtagList = new List<string>();
            var hashtags = await _tweetCollection
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
                .SortBy(x => x.CreatedAt)
                .ThenByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();

            foreach (var tweet in hashtags)
            {
                hashtagList.AddRange(tweet.Hashtags!);
            }
            return hashtagList;
        }

        public async Task<IList<BsonDocument>> GetTweetsByHashtag(
            string hashtag,
            int skip,
            int limit
        )
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
                .SortBy(x => x.CreatedAt)
                .ThenByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Limit(limit)
                .Lookup("users", "userId", "_id", "user")
                .Lookup("tweets", "originalTweetId", "_id", "originalTweet")
                .Unwind(
                    "originalTweet",
                    new AggregateUnwindOptions<TweetVm>() { PreserveNullAndEmptyArrays = true }
                )
                .Unwind("user")
                .Lookup("users", "originalTweet.userId", "_id", "originalTweet.user")
                .Unwind(
                    "originalTweet.user",
                    new AggregateUnwindOptions<BsonDocument>() { PreserveNullAndEmptyArrays = true }
                )
                .ToListAsync();
            return tweets;
        }
    }
}
