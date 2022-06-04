using server.Dto.UserDto.UpdateUserDto;
using server.Model.User;

namespace server.Interface
{
    public interface IUserService
    {
        Task<IList<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task UpdateUserInfo(UpdateUserDto updateUserDto);
        Task DeleteUserById(string id);
    }
}
