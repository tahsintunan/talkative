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
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class UserService : IUser
    {
        private readonly IMongoCollection<User>? _userCollection;
        private readonly IAuth _authService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(
            IOptions<UserDatabaseConfig> userDatabaseConfig,
            IAuth authService,
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

            bool usernameExists = false;

            if (updatedUser.Username != user.Username)
            {
                var existingUserWithSameUsername = await _userCollection
                    .Find(user => user.Username == updatedUser.Username)
                    .FirstOrDefaultAsync();
                usernameExists = existingUserWithSameUsername != null;
            }

            if (usernameExists)
                throw new BadRequestException("User already exists");
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
            {
                return false;
            }
            var hashedPassword = await _authService.CheckIfPasswordMatches(
                oldPassword,
                user.Password!
            );
            if (hashedPassword)
            {
                await UpdatePassword(user, password);
                return true;
            }
            return false;
        }

        public async Task ForgetPassword(string email)
        {
            string newPassword = GenerateRandomString(16);
            var user = await _userCollection
                .Find(user => user.Email == email)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                await UpdatePassword(user, newPassword);
                await SendPasswordWithMail(email, newPassword);
            }
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

        public async Task<IList<UserVm>> GetBlockedUsers(string userId, int skip, int limit)
        {
            var userVmList = await _userCollection
                .Aggregate()
                .Match(x => x.Id == userId)
                .Skip(skip)
                .Limit(limit)
                .Lookup("users", "blocked", "_id", "user")
                .Unwind("user")
                .ReplaceRoot<User>("$user")
                .Project(o => new UserVm() { UserId = o.Id, Username = o.Username })
                .ToListAsync();
            return userVmList;
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

        private async Task SendPasswordWithMail(string email, string newPassword)
        {
            var systemEmailCredentials = _configuration.GetSection("EmailCredentials");
            var systemEmail = systemEmailCredentials.GetSection("Email").Value;
            var systemEmailPassword = systemEmailCredentials.GetSection("Password").Value;
            MailMessage message = new();
            SmtpClient smtp = new();
            message.From = new MailAddress(systemEmail);
            message.To.Add(new MailAddress(email));
            message.Subject = "Your New Password";
            message.IsBodyHtml = true; //to make message body as html
            message.Body = "Your new password is " + newPassword;
            smtp.UseDefaultCredentials = true;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(systemEmail, systemEmailPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }

        public static string GenerateRandomString(int length)
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }
    }
}
