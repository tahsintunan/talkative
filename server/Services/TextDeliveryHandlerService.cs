using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server.Interface;
using server.Model;

namespace server.Services;

public class TextDeliveryHandlerService: IHostedService
{
    private const string RabbitmqServerConnectionString = "amqps://ddnxoukr:qyYjYj9t0IK6zJqlZkKtMyj8eZt2dy90@mustang.rmq.cloudamqp.com/ddnxoukr";
    private const string RabbitmqQueueName = "Q2";
    
    private readonly IChatHub _chatHub;
    public TextDeliveryHandlerService(IChatHub chatHub)
    {
        _chatHub = chatHub;
    }
    
    private async Task ProcessMessage(Message message)
    {
        await _chatHub.SendMessage(message);
    }
    
    private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory() { Uri = new Uri(RabbitmqServerConnectionString) };
    private static readonly IConnection Connection = ConnectionFactory.CreateConnection();
    private static readonly IModel Channel = Connection.CreateModel();
    private static readonly EventingBasicConsumer Consumer = new EventingBasicConsumer(Channel);
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Consumer.Received += async (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var marshalledMessageObject = Encoding.UTF8.GetString(body);
            var messageObject = JsonConvert.DeserializeObject<Message>(marshalledMessageObject);
            await ProcessMessage(messageObject!);
        };
        
        Channel.BasicConsume(queue: RabbitmqQueueName, autoAck: true, consumer: Consumer);
        return Task.FromResult<IActionResult>(new OkObjectResult("Ok TextDeliveryHandlerService"));
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}