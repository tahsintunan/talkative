using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class BlockService : IBlock
{
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<User>? _userCollection;

    public BlockService(
        IOptions<UserDatabaseConfig> userDatabaseConfig,
        IConfiguration configuration
    )
    {
        var mongoClient = new MongoClient(userDatabaseConfig.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(userDatabaseConfig.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userDatabaseConfig.Value.UserCollectionName
        );

        _configuration = configuration;
    }

    public async Task<Dictionary<string, bool>> GetBlockedUserIds(string userId)
    {
        var blockedHashmap = new Dictionary<string, bool>();
        var userVmList = await _userCollection
            .Aggregate()
            .Match(x => x.Id == userId)
            .Lookup("users", "blocked", "_id", "user")
            .Unwind("user")
            .ReplaceRoot<User>("$user")
            .ToListAsync();

        foreach (var userVm in userVmList)
            blockedHashmap.Add(userVm.Id!, true);
        return blockedHashmap;
    }

    public async Task<IList<UserVm>> GetBlockedUsers(string userId, int skip, int limit)
    {
        var userVmList = await _userCollection
            .Aggregate()
            .Match(x => x.Id == userId)
            .Limit(limit)
            .Skip(skip)
            .Lookup("users", "blocked", "_id", "user")
            .Unwind("user")
            .ReplaceRoot<User>("$user")
            .Project(user => new UserVm
                { UserId = user.Id, Username = user.Username, ProfilePicture = user.ProfilePicture })
            .ToListAsync();
        return userVmList;
    }
}