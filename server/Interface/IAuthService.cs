using server.Model.User;

namespace server.Interface
{
    public interface IAuthService
    {
        Task signupUser(User user);
        Task<bool> checkIfUsernameExists(string username);
    }
}
