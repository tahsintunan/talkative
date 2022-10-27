using Application.Comments.Commands.LikeComment;
using Application.Common.Class;
using Application.Common.ViewModels;
using Application.Followers.Commands.AddFollower;
using Application.Tweets.Commands.LikeTweet;
using Domain.Entities;

namespace Application.Common.Interface;

public interface INotification
{
    public Task TriggerFollowNotification(AddFollowerCommand request);
    public Task TriggerRetweetNotification(Tweet retweet, Blockable originalTweetVm);
    public Task TriggerLikeTweetNotification(LikeTweetCommand request, Blockable tweetVm);
    public Task TriggerCommentNotification(Comment comment, Blockable tweetVm);
    public Task TriggerLikeCommentNotification(LikeCommentCommand request, CommentVm commentVm);
    public Task<IList<NotificationVm>> GetNotifications(string userId, int skip, int limit);
}