using Domain.Entities;

namespace Application.Common.ViewModels
{
    public class CommentVm
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? TweetId { get; set; }
        public IList<string>? Likes { get; set; }
        public DateTime? Created { get; set; }
    }
}
