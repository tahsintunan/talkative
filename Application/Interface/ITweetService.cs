using MongoDB.Bson;
using server.Domain.Entities;

namespace server.Application.Interface
{
    public interface ITweetService
    {
        Task PublishTweet(Tweet tweet);
        Task DeleteTweet(string id);
        Task DeleteRetweet(string retweetId, string userId);
        Task UpdateTweet(Tweet tweet);
        Task GetTweetsOfFollowing();
        Task<BsonDocument?> GetTweetById(string id);
        Task<IList<BsonDocument>> GetTweetsOfSingleUser(string userId);
    }
}
