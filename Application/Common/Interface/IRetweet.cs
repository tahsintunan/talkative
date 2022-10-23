using Application.Common.ViewModels;
using MongoDB.Bson;

namespace Application.Common.Interface
{
    public interface IRetweet
    {
        Task UndoRetweet(string originalTweetId, string userId);
        Task<IList<BsonDocument>> GetQuoteRetweets(string tweetId, int skip, int limit);
        Task<IList<UserVm>> GetRetweetUsers(string tweetId, int skip, int limit);
    }
}
