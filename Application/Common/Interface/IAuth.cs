using Domain.Entities;
using System.Security.Cryptography;

namespace Application.Common.Interface
{
    public interface IAuth
    {
        Task SignupUser(User user);
        Task<string?> LoginUser(string username, string password);
        Task<bool> CheckIfUserExists(string username, string email);
        Task<bool> CheckIfUsernameExists(string username);
        Task<bool> CheckIfPasswordMatches(string username, string password);
        Task UpdateUserPassword(string id, string password);
        string GetHash(HashAlgorithm hashAlgorithm, string password);
    }
}
