using server.Model.User;

namespace server.Interface
{
    public interface IUserService
    {
        Task signupUser(User user);
        Task<User> findUser(User user);
    }
}
