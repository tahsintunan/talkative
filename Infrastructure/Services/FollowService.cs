using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Services
{
    public class FollowService : IFollow
    {
        private readonly IMongoCollection<Follower> _followerCollection;
        private readonly IMongoCollection<User> _userCollection;
        public FollowService(IOptions<FollowerDatabaseConfig> followerDatabaseConfig, IOptions<UserDatabaseConfig> userDatabaseConfig)
        {
            var mongoClient = new MongoClient(
            followerDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                followerDatabaseConfig.Value.DatabaseName);

            _followerCollection = mongoDatabase.GetCollection<Follower>(
                followerDatabaseConfig.Value.CollectionName);

            _userCollection = mongoDatabase.GetCollection<User>(userDatabaseConfig.Value.UserCollectionName);

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(
               followerDatabaseConfig.Value.ConnectionString
           );

            settings.LinqProvider = LinqProvider.V3;

        }

        public async Task AddNewFollower(Follower follower)
        {
            await _followerCollection.InsertOneAsync(follower);
        }

        public async Task DeleteFollower(string followerId, string followingId)
        {
            await _followerCollection.DeleteManyAsync(x => x.FollowingId == followingId && x.FollowerId == followerId);
        }

        public async Task<bool> CheckIfFollowerExists(string followerId, string followingId)
        {
            var follower = await _followerCollection.Find(x=>x.FollowerId==followerId && x.FollowingId==followingId).FirstOrDefaultAsync();
            return follower != null;
        }

        public async Task<IList<UserVm>> GetFollowerOfSingleUser(string userId)
        {
            var query = from follower in _followerCollection.AsQueryable()
                        where follower.FollowingId == userId
                        join userJoined in _userCollection on follower.FollowerId equals userJoined.Id into joined
                        from user in joined.DefaultIfEmpty()
                        select new UserVm()
                        {
                            Username = user.Username,
                            UserId = user.Id,
                            Email = user.Email,
                            DateOfBirth = user.DateOfBirth
                        };
            var users = await query.ToListAsync<UserVm>();

            return users;
        }
        public async Task<IList<UserVm>> GetFollowingsOfSingleUser(string userId)
        {
            var query = from follower in _followerCollection.AsQueryable()
                        where follower.FollowerId == userId
                        join userJoined in _userCollection on follower.FollowingId equals userJoined.Id into joined
                        from user in joined.DefaultIfEmpty()
                        select new UserVm()
                        {
                            Username = user.Username,
                            UserId = user.Id,
                            Email = user.Email,
                            DateOfBirth = user.DateOfBirth
                        };
            var users = await query.ToListAsync<UserVm>();

            return users;
        }

    }
}
