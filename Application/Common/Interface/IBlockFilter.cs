using Application.Common.ViewModels;

namespace Application.Common.Interface;

public interface IBlockFilter
{
    public Task<List<UserVm>> GetFilteredUsers(IEnumerable<UserVm> userVms, string userId);
    public Task<List<CommentVm>> GetFilteredComments(IEnumerable<CommentVm> commentVms, string userId);
    public Task<List<TweetVm>> GetFilteredTweets(IEnumerable<TweetVm> tweetVms, string userId);
}
