using Application.Common.Interface;
using Application.Common.ViewModels;
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
        private readonly IMongoCollection<User> _userCollection;

        public CommentService(
            IOptions<CommentDatabaseConfig> tweetDatabaseConfig,
            IOptions<UserDatabaseConfig> userDatabaseConfig
        )
        {
            var mongoClient = new MongoClient(tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(tweetDatabaseConfig.Value.DatabaseName);

            _commentCollection = mongoDatabase.GetCollection<Comment>(
                tweetDatabaseConfig.Value.CollectionName
            );

            _userCollection = mongoDatabase.GetCollection<User>(
                userDatabaseConfig.Value.UserCollectionName
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

        public async Task<CommentVm> GetCommentById(string id)
        {
            var query =
                from p in _commentCollection.AsQueryable()
                where p.Id == id
                join o in _userCollection on p.UserId equals o.Id into joined
                from sub_o in joined.DefaultIfEmpty()
                select new CommentVm
                {
                    Text = p.Text,
                    Id = p.Id!.ToString(),
                    TweetId = p.TweetId!.ToString(),
                    UserId = p.UserId!.ToString(),
                    Created = p.CreatedAt,
                    Likes = p.Likes,
                    Username = sub_o.Username
                };

            var commentVm = await query.FirstOrDefaultAsync();

            return commentVm;
        }

        public async Task<IList<CommentVm>> GetCommentsByTweetId(
            string tweetId,
            int skip,
            int limit
        )
        {
            var query = (
                from p in _commentCollection.AsQueryable()
                where p.TweetId == tweetId
                orderby p.CreatedAt descending
                join o in _userCollection on p.UserId equals o.Id into joined
                from sub_o in joined.DefaultIfEmpty()
                select new CommentVm
                {
                    Text = p.Text,
                    Id = p.Id!.ToString(),
                    TweetId = p.TweetId!.ToString(),
                    UserId = p.UserId!.ToString(),
                    Created = p.CreatedAt,
                    Likes = p.Likes,
                    Username = sub_o.Username
                }
            ).Skip(skip).Take(limit);

            var commentVmList = await query.ToListAsync();

            return commentVmList;
        }

        public async Task PartialUpdate(string commentId, UpdateDefinition<Comment> update)
        {
            await _commentCollection.UpdateOneAsync(
                p => p.Id == commentId,
                update,
                new UpdateOptions { IsUpsert = true }
            );
        }
    }
}
