using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Interface
{
    public interface IFollow
    {
        Task AddNewFollower(Follower follower);
        Task<IList<UserVm>> GetFollowerOfSingleUser(string userId);
        Task<IList<UserVm>> GetFollowingsOfSingleUser(string userId);
        Task<bool> CheckIfFollowerExists(string followerId, string followingId);
        Task DeleteFollower(string followerId,string followingId);
    }
}
