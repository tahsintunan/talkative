using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Interface
{
    public interface IFollow
    {
        Task AddNewFollower(Follower follower);
        Task<IList<UserVm>> GetFollowerOfSingleUser(string userId, int skip, int limit);
        Task<IList<UserVm>> GetFollowingsOfSingleUser(string userId, int skip, int limit);
        Task<bool> CheckIfFollowerExists(string followerId, string followingId);
        Task<Dictionary<string, bool>> GetFollowingOfCurrentUser(string userId);
        Task DeleteFollower(string followerId, string followingId);
        Task<long> GetNumberOfFollowingOfSingleUser(string userId);
        Task<long> GetNumberOfFollowerOfSingleUser(string userId);
    }
}
