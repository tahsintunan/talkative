using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;
using server.Infrastructure.DbConfig;

namespace server.Infrastructure.Services
{
    public class TweetService : ITweetService
    {
        private readonly IMongoCollection<Tweet> _tweetCollection;
        public TweetService(IOptions<TweetDatabaseConfig> tweetDatabaseConfig, IMapper mapper, IUserService userService)
        {
            var mongoClient = new MongoClient(
            tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                tweetDatabaseConfig.Value.DatabaseName);

            _tweetCollection = mongoDatabase.GetCollection<Tweet>(
                tweetDatabaseConfig.Value.TweetCollectionName);

        }
        public async Task PublishTweet(Tweet tweet)
        {
            await _tweetCollection.InsertOneAsync(tweet);
        }

        public async Task DeleteTweet(string id)
        {
            await _tweetCollection.DeleteOneAsync(tweet => tweet.Id == id);
        }

        public async Task<Tweet?> GetTweetById(string id)
        {
            var tweet = await _tweetCollection.Find(tweet => tweet.Id == id).FirstOrDefaultAsync();
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

        public async Task<IList<Tweet>> GetTweetsOfSingleUser(string userId)
        {
            IList<Tweet> tweets = await _tweetCollection.Find(tweet => tweet.UserId == userId).ToListAsync();
            return tweets;
        }
    }
}
