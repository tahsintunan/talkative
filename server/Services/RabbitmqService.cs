using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using server.Dto.MessageDto;
using server.Interface;
using server.Model;

namespace server.Services;

public class RabbitmqService: IRabbitmqService
{
    private const string RabbitmqServerConnectionString = "amqps://ddnxoukr:qyYjYj9t0IK6zJqlZkKtMyj8eZt2dy90@mustang.rmq.cloudamqp.com/ddnxoukr";
    private const string RabbitmqServerExchangeName = "test";
    
    private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory() { Uri = new Uri(RabbitmqServerConnectionString) };
    private static readonly IConnection Connection = ConnectionFactory.CreateConnection();
    private static readonly IModel Channel = Connection.CreateModel();
    
    
    
    public Task<IActionResult> FanOut(MessageDto messageDto)
    {
        try
        {
            var messageObject = GetMessageObject(messageDto);
            var marshalledMessageObject = JsonConvert.SerializeObject(messageObject);
            
            var messageBuffer = Encoding.Default.GetBytes(marshalledMessageObject);
            Channel.BasicPublish(exchange: RabbitmqServerExchangeName,
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
        var message = new Message()
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
