using MongoDB.Bson;

namespace Application.Interface
{
    public interface IRetweetService
    {
        Task DeleteRetweet(string retweetId, string userId);
        Task<BsonDocument> GetAllRetweetPosts(string tweetId);
    }
}
