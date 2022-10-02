using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using server.Application.Dto.TweetDto;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;
using server.Infrastructure.DbConfig;

namespace server.Infrastructure.Services
{
    public class TweetService : ITweetService
    {
        private readonly IMongoCollection<Tweet> _tweetCollection;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public TweetService(IOptions<TweetDatabaseConfig> tweetDatabaseConfig, IMapper mapper, IUserService userService)
        {
            var mongoClient = new MongoClient(
            tweetDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                tweetDatabaseConfig.Value.DatabaseName);

            _tweetCollection = mongoDatabase.GetCollection<Tweet>(
                tweetDatabaseConfig.Value.TweetCollectionName);

            _mapper = mapper;
            _userService = userService;
        }
        public async Task PublishTweet(TweetDto tweetRequestDto, string userId)
        {
            Tweet tweet = new Tweet()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = tweetRequestDto.Text,
                UserId = userId,
                Hashtags = new List<string>(tweetRequestDto.Hashtags!),
                IsRetweet = tweetRequestDto.IsRetweet,
                RetweetId = tweetRequestDto.IsRetweet ? null : tweetRequestDto.RetweetId,
                Likes = new List<string>(),
                CommentId = new List<string>(),
                CreatedAt = DateTime.Now
            };
            await _tweetCollection.InsertOneAsync(tweet);
        }

        public async Task DeleteTweet(string id)
        {
            await _tweetCollection.DeleteOneAsync(tweet => tweet.Id == id);
        }

        public async Task<TweetVm> GetTweetById(string id)
        {
            var tweet = await _tweetCollection.Find(tweet => tweet.Id == id).FirstOrDefaultAsync();
            TweetVm tweetVm = _mapper.Map<TweetVm>(tweet);
            var user = await _userService.GetUserById(tweetVm.UserId!);
            tweetVm.Username = user.Username;
            return tweetVm;
        }

        public Task GetTweetOfCurrentUser()
        {
            throw new NotImplementedException();
        }

        public Task GetTweetsOfFollowing()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTweet(TweetDto tweetRequestDto)
        {
            var tweet = await _tweetCollection.Find(tweet => tweet.Id == tweetRequestDto.Id).FirstOrDefaultAsync();
            if (tweet != null)
            {
                var updatedTweet = new Tweet()
                {
                    Id = tweetRequestDto.Id,
                    Text = tweetRequestDto.Text ?? tweet.Text,
                    UserId = tweet.UserId,
                    Hashtags = tweetRequestDto.Hashtags ?? tweet.Hashtags,
                    IsRetweet = tweet.IsRetweet,
                    RetweetId = tweet.RetweetId,
                    Likes = new List<string>(tweet.Likes!),
                    CommentId = new List<string>(tweet.CommentId!),
                    CreatedAt = tweet.CreatedAt
                };
                await _tweetCollection.ReplaceOneAsync(x => x.Id == updatedTweet.Id, updatedTweet);
            }
        }
    }
}
