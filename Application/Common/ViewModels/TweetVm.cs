using MongoDB.Bson;

namespace Application.ViewModels
{
    public class TweetVm
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public UserVm? User { get; set; }
        public string? UserId { get; set; }
        public IList<string>? Hashtags { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetId { get; set; }
        public TweetVm? Retweet { get; set; }
        public IList<string?>? RetweetUsers { get; set; }
        public IList<string?>? RetweetPosts { get; set; }
        public IList<string?>? Likes { get; set; }
        public IList<string?>? Comments { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
