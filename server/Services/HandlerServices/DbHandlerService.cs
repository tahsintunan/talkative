using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server.Configs.DbConfig;
using server.Model.Message;

namespace server.Services.HandlerServices;

public class DbHandlerService: IHostedService
{
    private const string RabbitmqQueueName = "MongoDbQueue";
    
    private readonly IMongoCollection<Message> _messageCollection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    
    public DbHandlerService(IConfiguration configuration, IOptions<MessageDatabaseConfig> messageDatabaseConfig)
    {
        var connectionFactory = new ConnectionFactory { Uri = new Uri(configuration["RabbitMQ:ConnectionString"]) };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
        
        var mongoClient = new MongoClient(messageDatabaseConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(messageDatabaseConfig.Value.DatabaseName);
        _messageCollection = mongoDatabase.GetCollection<Message>(messageDatabaseConfig.Value.MessageCollectionName);
    }
    
    
    private async Task ProcessMessage(Message message)
    {
        await _messageCollection.InsertOneAsync(message);
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
        return Task.FromResult<IActionResult>(new OkObjectResult("Ok DbHandlerService"));
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}