using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using server.Application.Interface;
using server.Domain.Entities;
using server.Infrastructure.DbConfig;

namespace server.Infrastructure.Services
{
    public class TweetService : ITweetService
    {
        private readonly IMongoCollection<Tweet> _tweetCollection;
        public TweetService(IOptions<TweetDatabaseConfig> tweetDatabaseConfig, IOptions<UserDatabaseConfig> userDatabaseConfig)
        {
            var mongoClient = new MongoClient(
            tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                tweetDatabaseConfig.Value.DatabaseName);

            _tweetCollection = mongoDatabase.GetCollection<Tweet>(
                tweetDatabaseConfig.Value.TweetCollectionName);

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(tweetDatabaseConfig.Value.ConnectionString);

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

            BsonDocument pipelineStageZero = new BsonDocument("$match",
                new BsonDocument("_id", new ObjectId(id)));


            BsonDocument[] BaseJoinPipelines = GetBaseJoinPipelines();

            BsonDocument[] pipeline = new BsonDocument[1 + BaseJoinPipelines.Length];
            pipeline[0] = pipelineStageZero;
            BaseJoinPipelines.CopyTo(pipeline, 1);

            BsonDocument tweet = await _tweetCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
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

        public async Task<IList<BsonDocument>> GetTweetsOfSingleUser(string userId)
        {

            BsonDocument pipelineStageZero = new BsonDocument("$match",
                new BsonDocument("userId", new ObjectId(userId)));

            BsonDocument[] BaseJoinPipelines = GetBaseJoinPipelines();

            BsonDocument[] pipelines = new BsonDocument[1 + BaseJoinPipelines.Length];
            pipelines[0] = pipelineStageZero;
            BaseJoinPipelines.CopyTo(pipelines, 1);

            IList<BsonDocument> tweets = await _tweetCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return tweets;

        }

        private BsonDocument[] GetBaseJoinPipelines()
        {
            BsonDocument pipelineStageOne = new BsonDocument("$lookup",
                                            new BsonDocument
                                                {
                                                    { "from", "users" },
                                                    { "localField", "userId" },
                                                    { "foreignField", "_id" },
                                                    { "as", "user" }
                                                });

            BsonDocument pipelineStageTwo = new BsonDocument("$lookup",
                                                new BsonDocument
                                                    {
                                                        { "from", "tweets" },
                                                        { "localField", "retweetId" },
                                                        { "foreignField", "_id" },
                                                        { "as", "retweet" }
                                                    });

            BsonDocument pipelineStageThree = new BsonDocument("$project",
                                        new BsonDocument
                                            {
                                                { "_id",
                                        new BsonDocument("$toString", "$_id") },
                                                { "text", 1 },
                                                { "hashtags", 1 },
                                                { "userId", 1 },
                                                { "createdAt", 1 },
                                                { "likes", 1 },
                                                { "comments", 1 },
                                                { "isRetweet", 1 },
                                                { "retweetId", 1 },
                                                {"retweetUsers", 1 },
                                                {"retweetPosts", 1 },
                                                { "retweet",
                                        new BsonDocument("$first", "$retweet") },
                                                { "user",
                                        new BsonDocument("$first", "$user") }
                                            });

            BsonDocument pipelineStageFour = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "users" },
                                                { "localField", "retweet.userId" },
                                                { "foreignField", "_id" },
                                                { "as", "retweet.user" }
                                            });

            BsonDocument pipelineStageFive = new BsonDocument("$project",
                                        new BsonDocument
                                            {
                                                { "_id", new BsonDocument("$toString", "$_id") },
                                                { "text", 1 },
                                                { "hashtags", 1 },
                                                { "userId", 1 },
                                                { "createdAt", 1 },
                                                { "likes", 1 },
                                                { "comments", 1 },
                                                { "isRetweet", 1 },
                                                { "retweetId", 1 },
                                                {"retweetUsers", 1 },
                                                {"retweetPosts", 1 },
                                                { "retweet",
                                            new BsonDocument
                                                {
                                                    { "user" , new BsonDocument("$first", "$retweet.user") },
                                                    { "text", 1 },
                                                    { "hashtags", 1 },
                                                    { "_id",new BsonDocument("$toString", "$retweet._id") },
                                                    { "isRetweet",1 },
                                                    { "createdAt", 1 },
                                                    { "likes", 1 },
                                                    { "comments", 1 },
                                                    { "retweetPosts", 1 },
                                                    { "retweetUsers", 1 },
                                                } },
                                                { "user", 1 }
                                            });

            return new BsonDocument[] { pipelineStageOne, pipelineStageTwo, pipelineStageThree, pipelineStageFour, pipelineStageFive };
        }
    }
}
