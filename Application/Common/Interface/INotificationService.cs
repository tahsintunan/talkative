using Application.Comments.Commands.LikeComment;
using Application.Common.Class;
using Application.Common.ViewModels;
using Application.Tweets.Commands.LikeTweet;
using Domain.Entities;

namespace Application.Common.Interface;

public interface INotificationService
{
    public Task TriggerRetweetNotification(Tweet retweet, Blockable originalTweetVm);
    public Task TriggerLikeTweetNotification(LikeTweetCommand request, Blockable tweetVm);
    public Task TriggerCommentNotification(Comment comment, Blockable tweetVm);
    public Task TriggerLikeCommentNotification(LikeCommentCommand request, CommentVm commentVm);
}