using Application.Common.Dto.UpdateUserDto;
using Application.Common.Interface;
using Application.Common.ViewModels;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User>? _userCollection;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserService(IOptions<UserDatabaseConfig> userDatabaseConfig, IAuthService authService, IConfiguration configuration, IMapper mapper)
        {
            var mongoClient = new MongoClient(
            userDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userDatabaseConfig.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                userDatabaseConfig.Value.UserCollectionName);

            _mapper = mapper;

            _authService = authService;

            _configuration = configuration;
        }

        public async Task<IList<UserVm>?> GetAllUsers()
        {
            IList<User> users = await _userCollection.Find(users => true).ToListAsync();
            var usersVm = _mapper.Map<IList<UserVm>>(users);

            return usersVm;
        }

        public async Task<UserVm?> GetUserById(string id)
        {
            var user = await _userCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
            var userVm = _mapper.Map<UserVm>(user);
            return userVm;
        }

        public async Task UpdateUserInfo(UpdateUserDto updateUserDto)
        {
            var user = await _userCollection.Find(user => user.Id == updateUserDto.Id).FirstOrDefaultAsync();
            var updatedUser = new User()
            {
                Id = updateUserDto.Id,
                Username = user.Username,
                DateOfBirth = updateUserDto.DateOfBirth ?? user.DateOfBirth,
                Email = updateUserDto.Email ?? user.Email,
                Password = user.Password,
            };
            await _userCollection.ReplaceOneAsync(x => x.Id == updateUserDto.Id, updatedUser);
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

        public async Task ForgetPassword(string email)
        {
            string newPassword = GenerateRandomString(16);
            var user = await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
            if (user != null)
            {
                await UpdatePassword(user, newPassword);
                await SendPasswordWithMail(email, newPassword);
            }
        }

        public async Task UpdatePassword(User user, string password)
        {
            string hashedPassword;
            using (var sha256Hash = SHA256.Create())
            {
                hashedPassword = _authService.GetHash(sha256Hash, password);
            }

            user.Password = hashedPassword;
            await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);
        }

        private async Task SendPasswordWithMail(string email, string newPassword)
        {
            var systemEmailCredentials = _configuration.GetSection("EmailCredentials");
            var systemEmail = systemEmailCredentials.GetSection("Email").Value;
            var systemEmailPassword = systemEmailCredentials.GetSection("Password").Value;
            MailMessage message = new();
            SmtpClient smtp = new ();
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
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
