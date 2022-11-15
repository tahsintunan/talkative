using Application.Common.ViewModels;
using Domain.Entities;
using MongoDB.Driver;

namespace Application.Common.Interface;

public interface IUser
{
    Task<IList<UserVm>?> GetAllUsers(int skip, int limit);
    Task<UserVm?> GetUserById(string id);
    Task UpdateUserInfo(User updatedUser);
    Task DeleteUserById(string id);
    Task ForgetPassword(string email);
    Task<string> ResetPassword(string token);
    Task<Dictionary<string, bool>> GetBlockedUserIds(string userId);
    Task<IList<UserVm>> GetBlockedUsers(string userId, int skip, int limit);
    Task<bool> UpdatePassword(string userId, string oldPassword, string password);
    Task PartialUpdate(string userId, UpdateDefinition<User> update);
    Task<IList<User>> FindWithUsername(string username, int skip, int limit);
}
