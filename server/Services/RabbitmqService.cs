using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using server.Dto.MessageDto;
using server.Interface;
using server.Model.Message;

namespace server.Services;

public class RabbitmqService: IRabbitmqService
{
    private readonly string _rabbitmqServerExchangeName;
    private readonly IModel _channel;
    public RabbitmqService(IConfiguration configuration)
    {
        _rabbitmqServerExchangeName = configuration["RabbitMQ:ExchangeName"];
        var connectionFactory = new ConnectionFactory() { Uri = new Uri(configuration["RabbitMQ:ConnectionString"]) };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public Task<IActionResult> FanOut(MessageDto messageDto)
    {
        try
        {
            var messageObject = GetMessageObject(messageDto);
            var marshalledMessageObject = JsonConvert.SerializeObject(messageObject);
            
            var messageBuffer = Encoding.Default.GetBytes(marshalledMessageObject);
            _channel.BasicPublish(exchange: _rabbitmqServerExchangeName,
                routingKey: "",
                basicProperties: null,
                body: messageBuffer);
            
            return Task.FromResult<IActionResult>(new OkResult());
        }
        catch(Exception e)
        {
            return Task.FromResult<IActionResult>(new BadRequestObjectResult(e.Message));
        }
    }
    
    private static Message GetMessageObject(MessageDto messageDto)
    {
        var message = new Message
        {
            ChatroomId = GetChatroomId(messageDto.SenderId!, messageDto.ReceiverId!),
            SenderId = messageDto.SenderId,
            ReceiverId = messageDto.ReceiverId,
            Datetime = DateTime.Now,
            MessageText = messageDto.MessageText
        };
        return message;
    }
    
    private static string GetChatroomId(string senderId, string receiverId)
    {
        string a, b;
        if (string.Compare(senderId, receiverId, StringComparison.Ordinal) < 0)
        {
            a = senderId;
            b = receiverId;
        }
        else
        {
            a = receiverId;
            b = senderId;
        }
        return a + b;
    }
}
