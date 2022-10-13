using MongoDB.Bson;

namespace Application.Common.Interface
{
    public interface IRetweetService
    {
        Task DeleteRetweet(string retweetId, string userId);
        Task<BsonDocument> GetAllRetweetPosts(string tweetId);
    }
}
