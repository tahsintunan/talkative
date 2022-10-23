using System.Text;
using Application.Common.Interface;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Services.RMQHandlerService;

public class RtNotificationHandlerService: IHostedService
{
    private const string RabbitmqQueueName = "RtNotificationQueue";

    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    private readonly INotificationHub _notificationHub;

    public RtNotificationHandlerService(IConfiguration configuration, INotificationHub notificationHub)
    {
        _notificationHub = notificationHub;
        var connectionFactory = new ConnectionFactory { Uri = new Uri(configuration["RabbitMQ:ConnectionString"]) };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
    }
    
    
    private async Task ProcessNotification(Notification notification)
    {
        await _notificationHub.SendNotification(notification);
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

        _channel.BasicConsume(queue: RabbitmqQueueName, autoAck: true, consumer: _consumer);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}