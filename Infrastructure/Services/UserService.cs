using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.Common.ViewModels;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class UserService : IUser
{
    private readonly IAuth _authService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IToken _tokenService;
    private readonly IMongoCollection<User>? _userCollection;

    public UserService(
        IOptions<UserDatabaseConfig> userDatabaseConfig,
        IAuth authService,
        IToken tokenService,
        IConfiguration configuration,
        IMapper mapper
    )
    {
        var mongoClient = new MongoClient(userDatabaseConfig.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(userDatabaseConfig.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userDatabaseConfig.Value.UserCollectionName
        );

        _mapper = mapper;

        _authService = authService;

        _tokenService = tokenService;

        _configuration = configuration;
    }

    public async Task<IList<UserVm>?> GetAllUsers(int skip, int limit)
    {
        IList<User> users = await _userCollection
            .Find(users => true)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
        var usersVm = _mapper.Map<IList<UserVm>>(users);

        return usersVm;
    }

    public async Task<UserVm?> GetUserById(string id)
    {
        var user = await _userCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
        var userVm = _mapper.Map<UserVm>(user);
        return userVm;
    }

    public async Task UpdateUserInfo(User updatedUser)
    {
        var user = await _userCollection
            .Find(user => user.Id == updatedUser.Id)
            .FirstOrDefaultAsync();

        var usernameExists = false;
        var emailExists = false;

        if (updatedUser.Username != user.Username)
        {
            var existingUserWithSameUsername = await _userCollection
                .Find(user => user.Username == updatedUser.Username)
                .FirstOrDefaultAsync();
            usernameExists = existingUserWithSameUsername != null;
        }

        if (updatedUser.Email != user.Email)
        {
            var existingUserWithEmail = await _userCollection
                .Find(user => user.Email == updatedUser.Email)
                .FirstOrDefaultAsync();
            emailExists = existingUserWithEmail != null;
        }

        if (usernameExists)
            throw new BadRequestException("User already exists");

        if (emailExists)
            throw new BadRequestException("Email already exists");

        await PartialUpdate(
            updatedUser.Id!,
            Builders<User>.Update
                .Set(x => x.Username, updatedUser.Username)
                .Set(x => x.Email, updatedUser.Email)
                .Set(x => x.DateOfBirth, updatedUser.DateOfBirth)
        );
    }

    public async Task PartialUpdate(string userId, UpdateDefinition<User> update)
    {
        await _userCollection.UpdateOneAsync(
            p => p.Id == userId,
            update,
            new UpdateOptions { IsUpsert = true }
        );
    }

    public async Task DeleteUserById(string id)
    {
        await _userCollection.DeleteOneAsync(user => user.Id == id);
    }

    public async Task<bool> UpdatePassword(string userId, string oldPassword, string password)
    {
        var user = await _userCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
        if (user == null)
            throw new BadRequestException("User doesn't exist");

        var passwordMatch = await _authService.CheckIfPasswordMatches(user.Username!, oldPassword);

        if (!passwordMatch)
            throw new BadRequestException("Old password doesn't match");

        await UpdatePassword(user, password);
        return true;
    }

    public async Task ForgetPassword(string email)
    {
        var user = await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null)
            throw new NotFoundException("User does not exist with such email.");

        var token = _tokenService.GenerateAccessToken(
            user.Id!,
            user.Username!,
            user.Email!,
            user.IsAdmin ? Role.ADMIN.ToString() : Role.USER.ToString(),
            5
        );

        var link = _configuration["AppUrl"] + "/Auth/reset-password/" + token;

        var messageBody =
            $@"
                    <h4>Forgot Password Verification</h4>
                    <p>Click the link below to reset your password.</p>
                    <a href=""{link}"">Reset Password Link</a>
                ";

        await SendMail(email, "Forgot Password Verification", messageBody);
    }

    public async Task<string> ResetPassword(string token)
    {
        var userId = _tokenService.ValidateAccessToken(token);

        var user = await _userCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
        if (user == null)
            throw new BadRequestException("Invalid Request.");

        var newPassword = GeneratePassword(16);

        await UpdatePassword(user, newPassword);

        var messageBody =
            $@"
                    <h4>Your Password has been reset</h4>
                    <p>Your new password is: <strong>{newPassword}</strong></p>
                ";

        await SendMail(user.Email!, "Password Reset Successful", messageBody);

        return newPassword;
    }

    public async Task<IList<User>> FindWithUsername(string username, int skip, int limit)
    {
        var user = await _userCollection
            .Find(
                new BsonDocument
                {
                    {
                        "username",
                        new BsonDocument { { "$regex", username }, { "$options", "i" } }
                    }
                }
            )
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
        return user;
    }

    private async Task UpdatePassword(User user, string password)
    {
        string hashedPassword;
        using (var sha256Hash = SHA256.Create())
        {
            hashedPassword = _authService.GetHash(sha256Hash, password);
        }

        user.Password = hashedPassword;
        await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);
    }

    private async Task SendMail(string email, string subject, string messageBody)
    {
        var systemEmailCredentials = _configuration.GetSection("EmailCredentials");
        var systemEmail = systemEmailCredentials.GetSection("Email").Value;
        var systemEmailPassword = systemEmailCredentials.GetSection("Password").Value;
        MailMessage message = new();
        SmtpClient smtp = new();
        message.From = new MailAddress(systemEmail);
        message.To.Add(new MailAddress(email));
        message.Subject = subject;
        message.IsBodyHtml = true; //to make message body as html
        message.Body = messageBody;
        smtp.UseDefaultCredentials = true;
        smtp.Port = 587;
        smtp.Host = "smtp.gmail.com"; //for gmail host
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(systemEmail, systemEmailPassword);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        await smtp.SendMailAsync(message);
    }

    public static string GeneratePassword(int length)
    {
        const string chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";

        Random random = new();

        return new string(
            Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()
        );
    }
}