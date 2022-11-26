using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interface;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class AuthService : IAuth
{
    private readonly IToken _tokenService;
    private readonly IMongoCollection<User> _userCollection;

    public AuthService(
        IToken tokenService
    )
    {

        _tokenService = tokenService;

        var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("ConnectionString"));
        var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));

        _userCollection = mongoDatabase.GetCollection<User>(
            Environment.GetEnvironmentVariable("UserCollectionName")
        );
    }

    public async Task SignupUser(User user)
    {
        if (await CheckIfUserExists(user.Username!, user.Email!))
            throw new BadRequestException("User already exists.");

        string hashedPassword;
        using (var sha256Hash = SHA256.Create())
        {
            hashedPassword = GetHash(sha256Hash, user.Password!);
        }

        user.Password = hashedPassword;
        await _userCollection.InsertOneAsync(user);
    }

    public async Task<string?> LoginUser(string username, string password)
    {
        if (!await CheckIfUsernameExists(username))
            throw new BadRequestException("Username doesn't exist.");

        if (!await CheckIfPasswordMatches(username, password))
            throw new BadRequestException("Password doesn't match.");

        var user = await _userCollection
            .Find(
                user =>
                    user.Username == username && (user.IsBanned == null || user.IsBanned == false)
            )
            .FirstOrDefaultAsync();

        if (user == null)
            throw new BadRequestException("Invalid Request.");

        var accessToken = _tokenService.GenerateAccessToken(
            user.Id!,
            user.Username!,
            user.Email!,
            user.IsAdmin ? Role.ADMIN.ToString() : Role.USER.ToString(),
            60 * 24 * 7
        );
        var value = new AuthenticationHeaderValue("Bearer", accessToken);
        return accessToken;
    }

    public async Task<bool> CheckIfPasswordMatches(string username, string password)
    {
        var user = await _userCollection
            .Find(user => user.Username == username)
            .FirstOrDefaultAsync();
        string hashedPassword;
        using (var sha256Hash = SHA256.Create())
        {
            hashedPassword = GetHash(sha256Hash, password);
        }

        return hashedPassword == user.Password;
    }

    public string GetHash(HashAlgorithm hashAlgorithm, string password)
    {
        var data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
        var sBuilder = new StringBuilder();
        foreach (var t in data)
            sBuilder.Append(t.ToString("x2"));
        return sBuilder.ToString();
    }

    private async Task<bool> CheckIfUserExists(string username, string email)
    {
        var findUserByUsername = _userCollection
            .Find(user => user.Username == username)
            .FirstOrDefaultAsync();
        var findUserByEmail = _userCollection
            .Find(user => user.Email == email)
            .FirstOrDefaultAsync();

        await Task.WhenAll(findUserByUsername, findUserByEmail);

        var userByUsername = await findUserByUsername;
        var userByEmail = await findUserByEmail;

        return userByUsername != null || userByEmail != null;
    }

    private async Task<bool> CheckIfUsernameExists(string username)
    {
        var foundUser = await _userCollection
            .Find(user => user.Username == username)
            .FirstOrDefaultAsync();
        return foundUser != null;
    }
}