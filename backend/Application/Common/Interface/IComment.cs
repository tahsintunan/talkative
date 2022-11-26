using Application.Common.ViewModels;
using Domain.Entities;
using MongoDB.Driver;

namespace Application.Common.Interface;

public interface IComment
{
    Task CreateComment(Comment comment);
    Task PartialUpdate(string commentId, UpdateDefinition<Comment> update);
    Task DeleteComment(string id);
    Task<IList<CommentVm>> GetCommentsByTweetId(string tweetId, int skip, int limit);
    Task<CommentVm> GetCommentById(string id);
}