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

    [BsonElement("eventType")] public string? EventType { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("notificationReceiverId")]
    public string? NotificationReceiverId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("eventTriggererId")]
    public string? EventTriggererId { get; set; }

    [BsonElement("tweetId")] public string? TweetId { get; set; }

    [BsonElement("commentId")] public string? CommentId { get; set; }

    [BsonElement("datetime")] public DateTime Datetime { get; set; }

    [BsonElement("isRead")] public bool IsRead { get; set; }
}