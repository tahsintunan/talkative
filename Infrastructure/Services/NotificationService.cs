using Application.Comments.Commands.LikeComment;
using Application.Common.Class;
using Application.Common.Interface;
using Application.Common.ViewModels;
using Application.Followers.Commands.AddFollower;
using Application.Tweets.Commands.LikeTweet;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class NotificationService : INotification
{
    private readonly IRabbitmq _rabbitmqService;
    private readonly IUser _userService;
    private readonly IMongoCollection<Notification> _notificationCollection;

    public NotificationService(
        IRabbitmq rabbitmqService,
        IUser userService,
        IOptions<NotificationDatabaseConfig> notificationDatabaseConfig
    )
    {
        _rabbitmqService = rabbitmqService;
        _userService = userService;
        var mongoClient = new MongoClient(notificationDatabaseConfig.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(notificationDatabaseConfig.Value.DatabaseName);

        _notificationCollection = mongoDatabase.GetCollection<Notification>(
            notificationDatabaseConfig.Value.CollectionName
        );
    }

    public async Task TriggerFollowNotification(AddFollowerCommand request)
    {
        var eventTriggererUsername = await GetUsernameById(request.FollowerId!);
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = "follow",
            NotificationReceiverId = request.FollowingId,
            EventTriggererId = request.FollowerId,
            EventTriggererUsername = eventTriggererUsername,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
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

    public async Task TriggerLikeCommentNotification(
        LikeCommentCommand request,
        CommentVm commentVm
    )
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

    public async Task<IList<Notification>> GetNotifications(string userId, int skip, int limit)
    {
        return await _notificationCollection
            .Find(x => x.NotificationReceiverId == userId)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
    }
}
