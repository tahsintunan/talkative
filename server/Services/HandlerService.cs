using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server.Model;

namespace server.Services;

public abstract class HandlerService: IHostedService
{
    protected string RabbitmqQueueName;
    protected HandlerService()
    {
        RabbitmqQueueName = "";
    }

    private const string RabbitmqServerConnectionString = "amqps://ddnxoukr:qyYjYj9t0IK6zJqlZkKtMyj8eZt2dy90@mustang.rmq.cloudamqp.com/ddnxoukr";
    private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory()
    {
        Uri = new Uri(RabbitmqServerConnectionString)
    };


    protected abstract void ProcessMessage(Message message);
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var connection = ConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        var consumer = new EventingBasicConsumer(channel);
        
        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var marshalledMessageObject = Encoding.UTF8.GetString(body);
            var messageObject = JsonConvert.DeserializeObject<Message>(marshalledMessageObject);
            ProcessMessage(messageObject!);
        };
        
        channel.BasicConsume(queue: RabbitmqQueueName, autoAck: true, consumer: consumer);
        return Task.FromResult<IActionResult>(new OkObjectResult("Handler Running"));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}