using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Common.Interface
{
    public interface ITweetService
    {
        Task PublishTweet(Tweet tweet);
        Task DeleteTweet(string id);
        Task UpdateTweet(Tweet tweet);
        Task GetTweetsOfFollowing();
        Task<BsonDocument?> GetTweetById(string id);
        Task<IList<BsonDocument>> GetTweetsOfSingleUser(string userId);
        Task PartialUpdate(string tweetId, UpdateDefinition<Tweet> update);
    }
}
