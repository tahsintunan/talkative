using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }
    
    [BsonElement("eventType")]
    public string? EventType { get; set; }
    
    [BsonElement("notificationReceiverId")]
    public string? NotificationReceiverId { get; set; }

    [BsonElement("eventTriggererId")]
    public string? EventTriggererId { get; set; }

    [BsonElement("eventTriggererUsername")]
    public string? EventTriggererUsername { get; set; }
    
    [BsonElement("tweetId")]
    public string? TweetId { get; set; }
    
    [BsonElement("commentId")]
    public string? CommentId { get; set; }

    [BsonElement("datetime")]
    public DateTime Datetime { get; set; }
}