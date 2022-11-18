using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.DbConfig
{
    public static class DbIndexConfig
    {
        public static IServiceCollection DatabaseIndexConfig(this IServiceCollection services)
        {
            var userDatabaseConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<UserDatabaseConfig>>()
                .Value;
            var tweetDatabaseConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<TweetDatabaseConfig>>()
                .Value;

            var mongoClient = new MongoClient(userDatabaseConfig.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(userDatabaseConfig.DatabaseName);

            var _userCollection = mongoDatabase.GetCollection<User>(
                userDatabaseConfig.UserCollectionName
            );

            var _tweetCollection = mongoDatabase.GetCollection<Tweet>(
                tweetDatabaseConfig.TweetCollectionName
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
}
