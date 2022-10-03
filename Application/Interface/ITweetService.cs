using server.Domain.Entities;

namespace server.Application.Interface
{
    public interface ITweetService
    {
        Task PublishTweet(Tweet tweet);
        Task DeleteTweet(string id);
        Task UpdateTweet(Tweet tweet);
        Task GetTweetsOfFollowing();
        Task<Tweet?> GetTweetById(string id);
        Task<IList<Tweet>> GetTweetsOfSingleUser(string userId);
    }
}
