using Application.Common.ViewModels;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Common.Interface;

public interface ITweet
{
    Task PublishTweet(Tweet tweet);
    Task DeleteTweet(string id);
    Task UpdateTweet(Tweet tweet);
    Task<BsonDocument?> GetTweetById(string id);
    Task<IList<string>> SearchHashtags(string hashtag, int skip, int limit);
    Task<IList<BsonDocument>> GetTweetsOfSingleUser(string userId, int skip, int take);
    Task PartialUpdate(string tweetId, UpdateDefinition<Tweet> update);
    Task<IList<BsonDocument>> GenerateFeed(string userId, int skip, int take);
    Task<IList<BsonDocument>> GetTweetsByHashtag(string hashtag, int skip, int take);
    Task<IList<TrendingHashtagVm>> GetTrendingHashtags();
    Task<long> GetNumberOfTweetsOfUser(string userId);
    Task<IList<User>> GetTopActiveUsers(int skip, int limit);
}