using System.Text;
using Application.Interface;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Services.HandlerService;

public class TextDeliveryHandlerService : IHostedService
{
    private const string RabbitmqQueueName = "TextDeliveryQueue";

    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    private readonly IChatHub _chatHub;

    public TextDeliveryHandlerService(IChatHub chatHub, IConfiguration configuration)
    {
        _chatHub = chatHub;

        var connectionFactory = new ConnectionFactory { Uri = new Uri(configuration["RabbitMQ:ConnectionString"]) };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
    }


    private async Task ProcessMessage(Message message)
    {
        await _chatHub.SendMessage(message);
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Received += async (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var marshalledMessageObject = Encoding.UTF8.GetString(body);
            var messageObject = JsonConvert.DeserializeObject<Message>(marshalledMessageObject);
            await ProcessMessage(messageObject!);
        };

        _channel.BasicConsume(queue: RabbitmqQueueName, autoAck: true, consumer: _consumer);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}