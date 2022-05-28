using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using server.Interface;
using server.Model;
using server.Model.User;
using System.Security.Cryptography;
using System.Text;

namespace server.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User>? _userCollection;
        public UserService(IOptions<UserDatabaseConfig> userDatabaseConfig)
        {
            var mongoClient = new MongoClient(
            userDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userDatabaseConfig.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                userDatabaseConfig.Value.UserCollectionName);
        }

        public async Task signupUser(User user)
        {
            string? hashedPassword;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, user?.Password);
            }
            User newUser = new User()
            {   
                Id=ObjectId.GenerateNewId(),
                Username = user.Username,
                Password = hashedPassword,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth
            };
            await _userCollection.InsertOneAsync(newUser);
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string password)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public async Task<User> findUser(User user)
        {
            var foundUser=await _userCollection.Find(existingUser=>existingUser.Username == user.Username).FirstOrDefaultAsync();
            return foundUser;
        }
    }
}
