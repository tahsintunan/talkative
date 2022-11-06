using Application.Comments.Commands.LikeComment;
using Application.Common.Class;
using Application.Common.Enums;
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
    private readonly IMongoCollection<Notification> _notificationCollection;
    private readonly IRabbitmq _rabbitmqService;
    private readonly IMongoCollection<User> _userCollection;

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

        var settings = MongoClientSettings.FromConnectionString(
            notificationDatabaseConfig.Value.ConnectionString
        );

        settings.LinqProvider = LinqProvider.V3;
    }

    public async Task TriggerFollowNotification(AddFollowerCommand request)
    {
        var notification = new Notification
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = NotificationType.Follow,
            NotificationReceiverId = request.FollowingId,
            EventTriggererId = request.FollowerId,
            Datetime = DateTime.Now
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerRetweetNotification(Tweet retweet, Blockable originalTweetVm)
    {
        var notification = new Notification
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = retweet.IsQuoteRetweet
                ? NotificationType.QuoteRetweet
                : NotificationType.Retweet,
            NotificationReceiverId = originalTweetVm.UserId,
            EventTriggererId = retweet.UserId,
            TweetId = retweet.IsQuoteRetweet ? retweet.Id : retweet.OriginalTweetId,
            Datetime = DateTime.Now
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerLikeTweetNotification(LikeTweetCommand request, Blockable tweetVm)
    {
        var notification = new Notification
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = NotificationType.LikeTweet,
            NotificationReceiverId = tweetVm.UserId,
            EventTriggererId = request.UserId,
            TweetId = request.TweetId,
            Datetime = DateTime.Now
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerCommentNotification(Comment comment, Blockable tweetVm)
    {
        var notification = new Notification
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = NotificationType.Comment,
            NotificationReceiverId = tweetVm.UserId,
            EventTriggererId = comment.UserId,
            TweetId = comment.TweetId,
            CommentId = comment.Id,
            Datetime = DateTime.Now
        };
        await _rabbitmqService.FanOut(notification);
    }

    public async Task TriggerLikeCommentNotification(
        LikeCommentCommand request,
        CommentVm commentVm
    )
    {
        var notification = new Notification
        {
            Id = ObjectId.GenerateNewId().ToString(),
            EventType = NotificationType.LikeComment,
            NotificationReceiverId = commentVm.UserId,
            EventTriggererId = request.UserId,
            TweetId = commentVm.TweetId,
            CommentId = commentVm.Id,
            Datetime = DateTime.Now
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
            from subO in joined.DefaultIfEmpty()
            select new NotificationVm
            {
                NotificationId = notification.Id,
                EventType = notification.EventType,
                NotificationReceiverId = notification.NotificationReceiverId,
                EventTriggererId = notification.EventTriggererId,
                EventTriggererUsername = subO.Username,
                TweetId = notification.TweetId,
                CommentId = notification.CommentId,
                DateTime = notification.Datetime,
                EventTriggererProfilePicture = subO.ProfilePicture,
                IsRead = notification.IsRead
            }
        ).Skip(skip).Take(limit);

        var notificationVms = await query.ToListAsync();

        return notificationVms;
    }

    public async Task DeleteNotification(string notificationId)
    {
        await _notificationCollection.DeleteOneAsync(x => x.Id == notificationId);
    }

    public async Task UpdateReadStatus(string notificationId)
    {
        var notification = await GetNotification(notificationId);
        await _notificationCollection.UpdateOneAsync(x => x.Id == notificationId, Builders<Notification>.Update.Set(x => x.IsRead, !notification.IsRead));
    }

    public async Task MarkAllAsRead(string userId)
    {
        await _notificationCollection.UpdateManyAsync(x => x.NotificationReceiverId == userId, Builders<Notification>.Update.Set(x => x.IsRead, true));
    }

    private async Task<Notification> GetNotification(string notificationId)
    {
        return await _notificationCollection.Find(x => x.Id == notificationId).FirstOrDefaultAsync();
    }
}