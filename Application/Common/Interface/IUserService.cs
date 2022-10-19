using Application.Common.Dto.UpdateUserDto;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Interface
{
    public interface IUserService
    {
        Task<IList<UserVm>> GetAllUsers();
        Task<UserVm> GetUserById(string id);
        Task UpdateUserInfo(UpdateUserDto updateUserDto);
        Task DeleteUserById(string id);
        Task ForgetPassword(string email);
        Task UpdatePassword(User user, string password);
    }
}
