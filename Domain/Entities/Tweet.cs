﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Tweet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("hashtags")]
        public IList<string>? Hashtags { get; set; }

        [BsonElement("likes")]
        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string>? Likes { get; set; }

        [BsonElement("comments")]
        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string>? Comments { get; set; }


        [BsonElement("isRetweet")]
        public bool IsRetweet { get; set; }


        [BsonElement("retweetId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? RetweetId { get; set; }


        [BsonElement("retweetUsers")]
        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string>? RetweetUsers { get; set; }

        [BsonElement("retweetPosts")]
        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string>? RetweetPosts { get; set; }
    }
}