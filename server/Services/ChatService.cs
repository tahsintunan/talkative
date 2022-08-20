using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server.Configs.DbConfig;
using server.Dto.RequestDto.ChatHistoryRequestDto;
using server.Dto.ResponseDto.ChatHistoryResponseDto;
using server.Interface;
using server.Model.Message;

namespace server.Services;

public class ChatService: IChatService
{
    private readonly IMongoCollection<Message> _messageCollection;
    public ChatService(IOptions<MessageDatabaseConfig> messageDatabaseConfig)
    {
        var client = new MongoClient(messageDatabaseConfig.Value.ConnectionString);
        var database = client.GetDatabase(messageDatabaseConfig.Value.DatabaseName);
        _messageCollection = database.GetCollection<Message>(messageDatabaseConfig.Value.MessageCollectionName);
    }

    
    public List<ChatHistoryResponseDto> GetMessageHistory(ChatHistoryRequestDto chatHistoryRequestDto)
    {
        var chatroomId = GetChatroomId(chatHistoryRequestDto.SenderId!, chatHistoryRequestDto.ReceiverId!);
        var documents = _messageCollection.Find(message => message.ChatroomId == chatroomId)
            .SortBy(message => message.Datetime)
            .ToList();
        return documents.Select(message => new ChatHistoryResponseDto()
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