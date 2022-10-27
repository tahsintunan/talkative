using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Follower
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }

    [BsonElement("followerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? FollowerId { get; set; }

    [BsonElement("followingId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? FollowingId { get; set; }
}