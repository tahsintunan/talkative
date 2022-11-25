using System.Text;
using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Services.RMQHandlerService;

public class RtNotificationHandlerService : IHostedService
{
    private const string RabbitmqQueueName = "RtNotificationQueue";
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    private readonly INotificationHub _notificationHub;
    private readonly IUser _userService;

    public RtNotificationHandlerService(INotificationHub notificationHub,
        IUser userService)
    {
        _userService = userService;
        _notificationHub = notificationHub;
        var connectionFactory = new ConnectionFactory { Uri = new Uri(Environment.GetEnvironmentVariable("RabbitMQ__ConnectionString") ?? "") };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Received += async (_, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var marshalledNotificationObject = Encoding.UTF8.GetString(body);
            var notificationObject = JsonConvert.DeserializeObject<Notification>(marshalledNotificationObject);
            await ProcessNotification(notificationObject!);
        };

        _channel.BasicConsume(RabbitmqQueueName, true, _consumer);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }


    private async Task ProcessNotification(Notification notification)
    {
        var (eventTriggererUsername, eventTriggererProfilePicture) =
            await GetUsernameAndProfilePictureById(notification.EventTriggererId!);
        var notificationVm = new NotificationVm
        {
            NotificationId = notification.Id,
            EventType = notification.EventType,
            NotificationReceiverId = notification.NotificationReceiverId,
            EventTriggererId = notification.EventTriggererId,
            EventTriggererUsername = eventTriggererUsername,
            EventTriggererProfilePicture = eventTriggererProfilePicture,
            TweetId = notification.TweetId,
            CommentId = notification.CommentId,
            DateTime = DateTime.Now,
            IsRead = notification.IsRead
        };
        await _notificationHub.SendNotification(notificationVm);
    }

    private async Task<(string?, string?)> GetUsernameAndProfilePictureById(string userId)
    {
        var user = await _userService.GetUserById(userId);
        if (user == null) return ("Unknown", null);
        return (user.Username, user.ProfilePicture);
    }
}