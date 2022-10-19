using Application.Common.Dto.UpdateUserDto;
using Application.Common.ViewModels;
using Domain.Entities;
using MongoDB.Driver;

namespace Application.Common.Interface
{
    public interface IUserService
    {
        Task<IList<UserVm>?> GetAllUsers();
        Task<UserVm?> GetUserById(string id);
        Task UpdateUserInfo(UpdateUserDto updateUserDto);
        Task DeleteUserById(string id);
        Task ForgetPassword(string email);
        Task<IList<UserVm>> GetBlockedUsers(string userId);
        Task UpdatePassword(User user, string password);
        Task PartialUpdate(string userId, UpdateDefinition<User> update);
        Task<IList<User>> FindWithUsername(string username);
    }
}
