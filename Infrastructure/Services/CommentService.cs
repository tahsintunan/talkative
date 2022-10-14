using Application.Common.Interface;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Services
{
    public class CommentService : IComment
    {
        private readonly IMongoCollection<Comment> _commentCollection;

        public CommentService(IOptions<CommentDatabaseConfig> tweetDatabaseConfig)
        {
            var mongoClient = new MongoClient(tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(tweetDatabaseConfig.Value.DatabaseName);

            _commentCollection = mongoDatabase.GetCollection<Comment>(
                tweetDatabaseConfig.Value.CollectionName
            );

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(
                tweetDatabaseConfig.Value.ConnectionString
            );

            settings.LinqProvider = LinqProvider.V3;
        }

        public async Task CreateComment(Comment comment)
        {
            await _commentCollection.InsertOneAsync(comment);
        }

        public async Task DeleteComment(string id)
        {
            await _commentCollection.DeleteOneAsync(comment => comment.Id == id);
        }

        public void GetCommentById(string id)
        {
            //var comment =
        }

        public void GetCommentsByTweetId(string tweetId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateComment(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
