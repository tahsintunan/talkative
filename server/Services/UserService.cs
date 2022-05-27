using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server.Interface;
using server.Model;

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
            await _userCollection.InsertOneAsync(user);
        }
    }
}
