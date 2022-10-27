using Application.Common.Class;

namespace Application.Common.ViewModels;

public class TweetVm : Blockable
{
    public string? Id { get; set; }
    public string? Text { get; set; }
    public UserVm? User { get; set; }
    public IList<string>? Hashtags { get; set; }
    public bool IsRetweet { get; set; }
    public bool IsQuoteRetweet { get; set; }
    public string? OriginalTweetId { get; set; }
    public TweetVm? OriginalTweet { get; set; }
    public IList<string?>? RetweetUsers { get; set; }
    public IList<string?>? QuoteRetweets { get; set; }
    public IList<string?>? Likes { get; set; }
    public IList<string?>? Comments { get; set; }
    public DateTime? CreatedAt { get; set; }
}