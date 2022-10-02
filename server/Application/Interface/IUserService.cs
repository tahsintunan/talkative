using server.Application.Dto.UpdateUserDto;
using server.Domain.Entities;

namespace server.Application.Interface
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
