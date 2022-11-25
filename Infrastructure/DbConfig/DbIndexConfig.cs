using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.DbConfig;

public static class DbIndexConfig
{
    public static IServiceCollection DatabaseIndexConfig(this IServiceCollection services)
    {
        // foreach (var item in Environment.GetEnvironmentVariables().Keys)
        // {
        //     Console.WriteLine(item.ToString() + " " + Environment.GetEnvironmentVariable(item.ToString() ?? ""));
        // }
        var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("ConnectionString"));

        var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));

        var _userCollection = mongoDatabase.GetCollection<User>(
            Environment.GetEnvironmentVariable("UserCollectionName")
        );

        var _tweetCollection = mongoDatabase.GetCollection<Tweet>(
            Environment.GetEnvironmentVariable("TweetCollectionName")
        );

        var userIndexModel = new CreateIndexModel<User>(
            Builders<User>.IndexKeys.Ascending(x => x.Username)
        );
        var tweetIndexModel = new CreateIndexModel<Tweet>(
            Builders<Tweet>.IndexKeys.Ascending(x => x.Hashtags)
        );

        _userCollection.Indexes.CreateOne(userIndexModel);

        _tweetCollection.Indexes.CreateOne(tweetIndexModel);

        return services;
    }
}