using server.Application.Dto.LoginDto;
using server.Application.Dto.SignupDto;
using System.Security.Cryptography;

namespace server.Application.Interface
{
    public interface IAuthService
    {
        Task SignupUser(SignupDto signupRequestDto);
        Task<string> LoginUser(LoginDto loginRequestDto);
        Task<bool> CheckIfUserExists(string username, string email);
        Task<bool> CheckIfUsernameExists(string username);
        Task<bool> CheckIfPasswordMatches(string username, string password);
        Task UpdateUserPassword(string id, string password);
        string GetHash(HashAlgorithm hashAlgorithm, string password);
    }
}
