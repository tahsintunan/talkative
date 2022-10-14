using Domain.Entities;

namespace Application.Common.Interface
{
    public interface IComment
    {
        Task CreateComment(Comment comment);
        Task UpdateComment(Comment comment);
        Task DeleteComment(string id);
        void GetCommentsByTweetId(string tweetId);
        void GetCommentById(string id);
    }
}
