using server.Model;

namespace server.Interface
{
    public interface IUserService
    {
        Task signupUser(User user);
    }
}
