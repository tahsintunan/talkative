using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server.Model;

namespace server.Services;

public class DbHandlerService: IHostedService
{
    private const string RabbitmqServerConnectionString = "amqps://ddnxoukr:qyYjYj9t0IK6zJqlZkKtMyj8eZt2dy90@mustang.rmq.cloudamqp.com/ddnxoukr";
    private const string RabbitmqQueueName = "Q1";
    
    private readonly IMongoCollection<Message> _messageCollection;
    public DbHandlerService(IOptions<MessageDatabaseConfig> messageDatabaseConfig)
    {
        var mongoClient = new MongoClient(
            messageDatabaseConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(
            messageDatabaseConfig.Value.DatabaseName);
        _messageCollection = mongoDatabase.GetCollection<Message>(
            messageDatabaseConfig.Value.MessageCollectionName);
    }
    
    private async Task ProcessMessage(Message message)
    {
        await _messageCollection.InsertOneAsync(message);
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
        return Task.FromResult<IActionResult>(new OkObjectResult("Ok DbHandlerService"));
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}