using Application.Common.Class;

namespace Application.Common.ViewModels
{
    public class CommentVm : Blockable
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? Username { get; set; }
        public string? TweetId { get; set; }
        public IList<string>? Likes { get; set; }
        public DateTime? Created { get; set; }
    }
}
