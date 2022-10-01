using server.Dto.RequestDto.TweetRequestDto;
using server.ViewModels;

namespace server.Interface
{
    public interface ITweetService
    {
        Task PublishTweet(TweetRequestDto tweetRequestDto,string userId);
        Task DeleteTweet(string id);
        Task UpdateTweet(TweetRequestDto tweetRequestDto);
        Task GetTweetOfCurrentUser();
        Task GetTweetsOfFollowing();
        Task<TweetVm> GetTweetById(string id);

    }
}
