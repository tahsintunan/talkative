using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server.Application.Dto.ChatHistoryDto;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;
using server.Infrastructure.DbConfig;

namespace server.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly IMongoCollection<Message> _messageCollection;
    public ChatService(IOptions<MessageDatabaseConfig> messageDatabaseConfig)
    {
        var client = new MongoClient(messageDatabaseConfig.Value.ConnectionString);
        var database = client.GetDatabase(messageDatabaseConfig.Value.DatabaseName);
        _messageCollection = database.GetCollection<Message>(messageDatabaseConfig.Value.MessageCollectionName);
    }


    public List<ChatHistoryVm> GetMessageHistory(ChatHistoryDto chatHistoryRequestDto)
    {
        var chatroomId = GetChatroomId(chatHistoryRequestDto.SenderId!, chatHistoryRequestDto.ReceiverId!);
        var documents = _messageCollection.Find(message => message.ChatroomId == chatroomId)
            .SortBy(message => message.Datetime)
            .ToList();
        return documents.Select(message => new ChatHistoryVm()
        {
            SenderId = message.SenderId,
            MessageText = message.MessageText,
            Datetime = message.Datetime,
        }).ToList();
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