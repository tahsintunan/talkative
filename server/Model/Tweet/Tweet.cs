using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace server.Model.Tweet
{
    public class Tweet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [BsonElement("userId")]
        public string? UserId{ get; set; }

        [BsonElement("text")]
        public string? Text{ get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("hashtags")]
        public IList<string>? Hashtags { get; set; }
        [BsonElement("likes")]
        public IList<string>? Likes { get; set; }
        [BsonElement("comments")]
        public IList<string>? CommentId { get; set; }
        [BsonElement("isRetweet")]
        public bool IsRetweet { get; set; }

        [BsonElement("retweetId")]
        public string? RetweetId { get; set; }
    }
}
