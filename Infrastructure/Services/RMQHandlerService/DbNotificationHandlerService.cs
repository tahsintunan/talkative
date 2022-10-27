using System.Text;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Services.RMQHandlerService;

public class DbNotificationHandlerService : IHostedService
{
    private const string RabbitmqQueueName = "DbNotificationQueue";
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    private readonly IMongoCollection<Notification> _notificationCollection;

    public DbNotificationHandlerService(
        IConfiguration configuration,
        IOptions<NotificationDatabaseConfig> notificationDatabaseConfig
    )
    {
        var mongoClient = new MongoClient(notificationDatabaseConfig.Value.ConnectionString);
        var notificationDatabase = mongoClient.GetDatabase(
            notificationDatabaseConfig.Value.DatabaseName
        );
        _notificationCollection = notificationDatabase.GetCollection<Notification>(
            notificationDatabaseConfig.Value.CollectionName
        );

        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(configuration["RabbitMQ:ConnectionString"])
        };
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
            var notificationObject = JsonConvert.DeserializeObject<Notification>(
                marshalledNotificationObject
            );
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
        await _notificationCollection.InsertOneAsync(notification);
    }
}