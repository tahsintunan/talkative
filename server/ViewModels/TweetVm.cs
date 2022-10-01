﻿namespace server.ViewModels
{
    public class TweetVm
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public IList<string>? Hashtags { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetId { get; set; }
        public IList<string?>? Likes { get; set; }
        public IList<string>? CommentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
