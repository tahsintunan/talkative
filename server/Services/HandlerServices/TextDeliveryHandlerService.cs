using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server.Interface;
using server.Model.Message;

namespace server.Services.HandlerServices;

public class TextDeliveryHandlerService: IHostedService
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
        return Task.FromResult<IActionResult>(new OkObjectResult("Ok TextDeliveryHandlerService"));
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}