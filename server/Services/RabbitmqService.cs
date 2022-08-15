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
    
    public Task<IActionResult> FanOut(MessageDto messageDto)
    {
        try
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(RabbitmqServerConnectionString)
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            var messageObject = GetMessageObject(messageDto);
            var marshalledMessageObject = JsonConvert.SerializeObject(messageObject);
            
            var messageBuffer = Encoding.Default.GetBytes(marshalledMessageObject);
            channel.BasicPublish(exchange: RabbitmqServerExchangeName,
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
        return senderId + receiverId;
    }
}
// - erpor oita handler e pathaite hobe
// - handler pore unmarshal koira kaaj korbe
