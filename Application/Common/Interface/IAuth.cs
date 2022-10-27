using System.Security.Cryptography;
using Domain.Entities;

namespace Application.Common.Interface;

public interface IAuth
{
    Task SignupUser(User user);
    Task<string?> LoginUser(string username, string password);
    Task<bool> CheckIfPasswordMatches(string username, string password);
    string GetHash(HashAlgorithm hashAlgorithm, string password);
}