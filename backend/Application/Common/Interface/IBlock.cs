using Application.Common.ViewModels;

namespace Application.Common.Interface;

public interface IBlock
{
    Task<Dictionary<string, bool>> GetBlockedUserIds(string userId);
    Task<IList<UserVm>> GetBlockedUsers(string userId, int skip, int limit);
}