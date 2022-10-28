using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Domain.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }

    [BsonElement("blockedBy")]
    [BsonRepresentation(BsonType.ObjectId)]
    public IList<string>? BlockedBy { get; set; }

    [BsonElement("blocked")]
    [BsonRepresentation(BsonType.ObjectId)]
    public IList<string>? Blocked { get; set; }

    [BsonElement("username")] public string? Username { get; set; }

    [BsonElement("dateOfBirth")] public DateTime DateOfBirth { get; set; }

    [BsonElement("password")] public string? Password { get; set; }

    [BsonElement("email")] public string? Email { get; set; }

    [BsonElement("isBanned")] public bool? IsBanned { get; set; }

    [BsonElement("isAdmin")] public bool IsAdmin { get; set; }
}