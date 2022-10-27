using System.Text;
using Application.Common.Interface;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure.Services;

public class RabbitmqService : IRabbitmq
{
    private readonly IModel _channel;
    private readonly string _rabbitmqServerExchangeName;

    public RabbitmqService(IConfiguration configuration)
    {
        _rabbitmqServerExchangeName = configuration["RabbitMQ:ExchangeName"];
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(configuration["RabbitMQ:ConnectionString"])
        };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public Task FanOut(Notification notification)
    {
        try
        {
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