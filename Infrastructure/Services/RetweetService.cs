using Application.Common.Interface;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services
{
    public class RetweetService : IRetweetService
    {
        private readonly IMongoCollection<Tweet> _tweetCollection;
        public RetweetService(IOptions<TweetDatabaseConfig> tweetDatabaseConfig)
        {
            var mongoClient = new MongoClient(
            tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                tweetDatabaseConfig.Value.DatabaseName);

            _tweetCollection = mongoDatabase.GetCollection<Tweet>(
                tweetDatabaseConfig.Value.TweetCollectionName);

        }

        public async Task DeleteRetweet(string retweetId, string userId)
        {
            await _tweetCollection.DeleteManyAsync(tweet => tweet.RetweetId == retweetId && tweet.UserId == userId);
        }

        public async Task<BsonDocument> GetAllRetweetPosts(string tweetId)
        {

            BsonDocument pipelineStageZero = new BsonDocument("$match",
                                    new BsonDocument("_id",
                                    new ObjectId("6346f1e00d333be25af525b2")));

            BsonDocument pipelineStageOne = new BsonDocument("$lookup",
                                    new BsonDocument
                                        {
                                            { "from", "tweets" },
                                            { "localField", "retweetPosts" },
                                            { "foreignField", "_id" },
                                            { "as", "retweetPosts" }
                                    });

            BsonDocument pipelineStageTwo = new BsonDocument("$addFields",
                                    new BsonDocument("retweetPosts",
                                    new BsonDocument("$ifNull",
                                    new BsonArray
                                                {
                                                    "$retweetPosts",
                                                    new BsonArray()
                                    })));

            BsonDocument pipelineStageThree = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "users" },
                                        { "localField", "retweetPosts.userId" },
                                        { "foreignField", "_id" },
                                        { "as", "user" }
                                });

            BsonDocument pipelineStageFour = new BsonDocument("$addFields",
                                new BsonDocument("retweetPosts",
                                new BsonDocument("$map",
                                new BsonDocument
                                            {
                                                { "input", "$retweetPosts" },
                                                { "in",
                                new BsonDocument("$mergeObjects",
                                new BsonArray
                                                    {
                                                        "$$this",
                                                        new BsonDocument("user",
                                                        new BsonDocument("$arrayElemAt",
                                                        new BsonArray
                                                                {
                                                                    "$user",
                                                                    new BsonDocument("$indexOfArray",
                                                                    new BsonArray
                                                                        {
                                                                            "$user._id",
                                                                            "$$this.userId"
                                                                        })
                                                                }))
                                                }) }
                                })));
            BsonDocument pipelineStageFive = new BsonDocument("$project",
                                new BsonDocument("retweetPosts", 1));


            BsonDocument[] pipelines = new BsonDocument[] { pipelineStageOne, pipelineStageTwo, pipelineStageThree, pipelineStageFour, pipelineStageFive };

            BsonDocument tweets = await _tweetCollection.Aggregate<BsonDocument>(pipelines).FirstOrDefaultAsync();

            return tweets;
        }
    }
}
