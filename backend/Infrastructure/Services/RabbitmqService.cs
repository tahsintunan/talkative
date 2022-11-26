using System.Text;
using Application.Common.Interface;
using Domain.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure.Services;

public class RabbitmqService : IRabbitmq
{
    private readonly IModel _channel;
    private readonly string _rabbitmqServerExchangeName;

    public RabbitmqService()
    {
        _rabbitmqServerExchangeName = Environment.GetEnvironmentVariable("RabbitMQ__ExchangeName") ?? "";
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(Environment.GetEnvironmentVariable("RabbitMQ__ConnectionString") ?? "")
        };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public Task FanOut(Notification notification)
    {
        try
        {
            if (notification.EventTriggererId == notification.NotificationReceiverId)
                return Task.CompletedTask;

            var marshalledNotification = JsonConvert.SerializeObject(notification);
            var notificationBytes = Encoding.Default.GetBytes(marshalledNotification);
            _channel.BasicPublish(
                _rabbitmqServerExchangeName,
                "",
                null,
                notificationBytes
            );

            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}