using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? UserId { get; set; }

    [BsonElement("tweetId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TweetId { get; set; }

    [BsonElement("text")] public string? Text { get; set; }

    [BsonElement("createdAt")] public DateTime? CreatedAt { get; set; }
    [BsonElement("lastModified")] public DateTime? LastModified { get; set; }

    [BsonElement("likes")]
    [BsonRepresentation(BsonType.ObjectId)]
    public IList<string>? Likes { get; set; }
}