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

        public FollowService(
            IOptions<FollowerDatabaseConfig> followerDatabaseConfig,
            IOptions<UserDatabaseConfig> userDatabaseConfig
        )
        {
            var mongoClient = new MongoClient(followerDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(followerDatabaseConfig.Value.DatabaseName);

            _followerCollection = mongoDatabase.GetCollection<Follower>(
                followerDatabaseConfig.Value.CollectionName
            );

            _userCollection = mongoDatabase.GetCollection<User>(
                userDatabaseConfig.Value.UserCollectionName
            );

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
            await _followerCollection.DeleteManyAsync(
                x => x.FollowingId == followingId && x.FollowerId == followerId
            );
        }

        public async Task<bool> CheckIfFollowerExists(string followerId, string followingId)
        {
            var follower = await _followerCollection
                .Find(x => x.FollowerId == followerId && x.FollowingId == followingId)
                .FirstOrDefaultAsync();
            return follower != null;
        }

        public async Task<IList<UserVm>> GetFollowerOfSingleUser(string userId, int skip, int limit)
        {
            var followerList = await _followerCollection
                .Aggregate()
                .Match(x => x.FollowingId == userId)
                .Skip(skip)
                .Limit(limit)
                .Lookup("users", "followerId", "_id", "user")
                .Unwind("user")
                .ReplaceRoot<User>("$user")
                .SortByDescending(x => x.Username)
                .Project(user => new UserVm() { UserId = user.Id, Username = user.Username, })
                .ToListAsync();

            return followerList;
        }

        public async Task<IList<UserVm>> GetFollowingsOfSingleUser(
            string userId,
            int skip,
            int limit
        )
        {
            var followingList = await _followerCollection
                .Aggregate()
                .Match(x => x.FollowerId == userId)
                .Skip(skip)
                .Limit(limit)
                .Lookup("users", "followingId", "_id", "user")
                .Unwind("user")
                .ReplaceRoot<User>("$user")
                .SortByDescending(x => x.Username)
                .Project(user => new UserVm() { UserId = user.Id, Username = user.Username, })
                .ToListAsync();

            return followingList;
        }
    }
}
