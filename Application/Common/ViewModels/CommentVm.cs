namespace Application.Common.ViewModels
{
    public class CommentVm
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? UserId { get; set; }
        public UserVm? User { get; set; }
        public string? TweetId { get; set; }
    }
}
