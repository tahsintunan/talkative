using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace server.Model.Message;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }
    
    [BsonElement("chatroomId")]
    public string? ChatroomId { get; set; }
    
    [BsonElement("senderId")]
    public string? SenderId { get; set; }
    
    [BsonElement("receiverId")]
    public string? ReceiverId { get; set; }
    
    [BsonElement("datetime")]
    public DateTime Datetime { get; set; }
    
    [BsonElement("messageText")]
    public string? MessageText { get; set; }
}
