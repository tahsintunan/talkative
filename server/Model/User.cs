using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
