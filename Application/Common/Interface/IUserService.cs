using Application.Common.Dto.UpdateUserDto;
using Domain.Entities;

namespace Application.Common.Interface
{
    public interface IUserService
    {
        Task<IList<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task UpdateUserInfo(UpdateUserDto updateUserDto);
        Task DeleteUserById(string id);
        Task ForgetPassword(string email);
        Task UpdatePassword(User user, string password);
    }
}
