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
using MongoDB.Driver.Linq;

namespace Infrastructure.Services;

public class NotificationService : INotification
{
    private readonly IRabbitmq _rabbitmqService;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Notification> _notificationCollection;

    public NotificationService(
        IRabbitmq rabbitmqService,
        IOptions<NotificationDatabaseConfig> notificationDatabaseConfig,
        IOptions<UserDatabaseConfig> userDatabaseConfig
    )
    {
        _rabbitmqService = rabbitmqService;
        var mongoClient = new MongoClient(notificationDatabaseConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(notificationDatabaseConfig.Value.DatabaseName);

        _notificationCollection = mongoDatabase.GetCollection<Notification>(
            notificationDatabaseConfig.Value.CollectionName
        );

        _userCollection = mongoDatabase.GetCollection<User>(
               userDatabaseConfig.Value.UserCollectionName
           );

        MongoClientSettings settings = MongoClientSettings.FromConnectionString(
                notificationDatabaseConfig.Value.ConnectionString
            );

        settings.LinqProvider = LinqProvider.V3;
    }

    public async Task TriggerFollowNotification(AddFollowerCommand request)
    {
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = "follow",
            NotificationReceiverId = request.FollowingId,
            EventTriggererId = request.FollowerId,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerRetweetNotification(Tweet retweet, Blockable originalTweetVm)
    {
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = retweet.IsQuoteRetweet ? "quoteRetweet" : "retweet",
            NotificationReceiverId = originalTweetVm.UserId,
            EventTriggererId = retweet.UserId,
            TweetId = retweet.IsQuoteRetweet ? retweet.Id : retweet.OriginalTweetId,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerLikeTweetNotification(LikeTweetCommand request, Blockable tweetVm)
    {
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = "likeTweet",
            NotificationReceiverId = tweetVm.UserId,
            EventTriggererId = request.UserId,
            TweetId = request.TweetId,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerCommentNotification(Comment comment, Blockable tweetVm)
    {
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = "createComment",
            NotificationReceiverId = tweetVm.UserId,
            EventTriggererId = comment.UserId,
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
        var notification = new Notification()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = "likeComment",
            NotificationReceiverId = commentVm.UserId,
            EventTriggererId = request.UserId,
            TweetId = commentVm.TweetId,
            CommentId = commentVm.Id,
            Datetime = DateTime.Now,
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task<IList<NotificationVm>> GetNotifications(string userId, int skip, int limit)
    {
        var query = (
            from notification in _notificationCollection.AsQueryable()
            where notification.NotificationReceiverId == userId
            orderby notification.Datetime descending
            join o in _userCollection on notification.EventTriggererId equals o.Id into joined
            from sub_o in joined.DefaultIfEmpty()
            select new NotificationVm
            {
                EventType = notification.EventType,
                EventTriggererId = notification.EventTriggererId,
                EventTriggererUsername = sub_o.Username,
                TweetId = notification.TweetId,
                CommentId = notification.CommentId,
                DateTime = notification.Datetime
            }
            ).Skip(skip).Take(limit);

        var notificationVms = await query.ToListAsync();

        return notificationVms;
    }
}
