using server.Application.ViewModels;
using server.Application.Dto;
using server.Application.Dto.TweetDto;

namespace server.Application.Interface
{
    public interface ITweetService
    {
        Task PublishTweet(TweetDto tweetDto, string userId);
        Task DeleteTweet(string id);
        Task UpdateTweet(TweetDto tweetDto);
        Task GetTweetOfCurrentUser();
        Task GetTweetsOfFollowing();
        Task<TweetVm> GetTweetById(string id);

    }
}
