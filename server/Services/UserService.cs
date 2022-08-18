using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server.Configs.DbConfig;
using server.Dto.UserDto.UpdateUserDto;
using server.Interface;
using server.Model.User;

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

        public async Task<IList<User>> GetAllUsers()
        {
            IList<User> users = await _userCollection.Find(users => true).ToListAsync();
            return users;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _userCollection.Find(user=>user.Id==id).FirstOrDefaultAsync();
            return user;
        }

        public async Task UpdateUserInfo(UpdateUserDto updateUserDto)
        {
            var user = await _userCollection.Find(user => user.Id == updateUserDto.Id).FirstOrDefaultAsync();
            var updatedUser = new User()
            {
                Id = updateUserDto.Id,
                Username = user.Username,
                DateOfBirth = updateUserDto.DateOfBirth?? user.DateOfBirth,
                Email = updateUserDto.Email?? user.Email,
                Password = user.Password,
            };
            await _userCollection.ReplaceOneAsync(x => x.Id == updateUserDto.Id,updatedUser );
        }

        public async Task DeleteUserById(string id)
        {
            await _userCollection.DeleteOneAsync(user => user.Id == id);
        }
    }
}
