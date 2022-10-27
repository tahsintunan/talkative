using Application.Comments.Commands.LikeComment;
using Application.Common.Class;
using Application.Common.ViewModels;
using Application.Followers.Commands.AddFollower;
using Application.Tweets.Commands.LikeTweet;
using Domain.Entities;

namespace Application.Common.Interface;

public interface INotification
{
    Task TriggerFollowNotification(AddFollowerCommand request);
    Task TriggerRetweetNotification(Tweet retweet, Blockable originalTweetVm);
    Task TriggerLikeTweetNotification(LikeTweetCommand request, Blockable tweetVm);
    Task TriggerCommentNotification(Comment comment, Blockable tweetVm);
    Task TriggerLikeCommentNotification(LikeCommentCommand request, CommentVm commentVm);
    Task<IList<NotificationVm>> GetNotifications(string userId, int skip, int limit);
    Task DeleteNotification(string notificationId);
    Task UpdateReadStatus(string notificationId);
}