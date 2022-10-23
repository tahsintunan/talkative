using Application.Comments.Commands.LikeComment;
using Application.Common.Class;
using Application.Common.Interface;
using Application.Common.ViewModels;
using Application.Tweets.Commands.LikeTweet;
using Domain.Entities;
using MongoDB.Bson;

namespace Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IRabbitmqService _rabbitmqService;
    private readonly IUserService _userService;
    
    public NotificationService(IRabbitmqService rabbitmqService, IUserService userService)
    {
        _rabbitmqService = rabbitmqService;
        _userService = userService;
    }
    
    
    public async Task TriggerRetweetNotification(Tweet retweet, Blockable originalTweetVm)
    {
        var eventTriggererUsername = await GetUsernameById(retweet.UserId!);
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = retweet.IsQuoteRetweet ? "quoteRetweet" : "retweet",
            NotificationReceiverId = originalTweetVm.UserId,
            EventTriggererId = retweet.UserId,
            EventTriggererUsername = eventTriggererUsername,
            TweetId = retweet.IsQuoteRetweet ? retweet.Id : retweet.OriginalTweetId,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerLikeTweetNotification(LikeTweetCommand request, Blockable tweetVm)
    {
        var eventTriggererUsername = await GetUsernameById(request.UserId!);
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = "likeTweet",
            NotificationReceiverId = tweetVm.UserId,
            EventTriggererId = request.UserId,
            EventTriggererUsername = eventTriggererUsername,
            TweetId = request.TweetId,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerCommentNotification(Comment comment, Blockable tweetVm)
    {
        var eventTriggererUsername = await GetUsernameById(comment.UserId!);
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = "createComment",
            NotificationReceiverId = tweetVm.UserId,
            EventTriggererId = comment.UserId,
            EventTriggererUsername = eventTriggererUsername,
            TweetId = comment.TweetId,
            CommentId = comment.Id,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerLikeCommentNotification(LikeCommentCommand request, CommentVm commentVm)
    {
        var eventTriggererUsername = await GetUsernameById(request.UserId!);
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = "likeComment",
            NotificationReceiverId = commentVm.UserId,
            EventTriggererId = request.UserId,
            EventTriggererUsername = eventTriggererUsername,
            TweetId = commentVm.TweetId,
            CommentId = commentVm.Id,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
    }

    private async Task<string> GetUsernameById(string userId)
    {
        var user = await _userService.GetUserById(userId);
        return user == null ? "Unknown" : user.Username!;
    }
}