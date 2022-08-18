using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace server.Model.User
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }
    }
}
