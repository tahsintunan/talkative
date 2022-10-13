using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MediatR;

namespace Application.Retweets.Command.UndoRetweet
{
    public class UndoRetweetCommand:IRequest
    {
        public string? Id { get; set; }
        public string? RetweetId { get; set; }
        public string? UserId { get; set; }
    }

    public class UndoRetweetCommandHandler : IRequestHandler<UndoRetweetCommand>
    {
        private readonly IRetweetService _retweetService;
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetDocumentMapper;

        public UndoRetweetCommandHandler(IRetweetService retweetService, ITweetService tweetService, IBsonDocumentMapper<TweetVm> tweetDocumentMapper)
        {
            _retweetService = retweetService;
            _tweetDocumentMapper = tweetDocumentMapper;
            _tweetService = tweetService;
        }

        public async Task<Unit> Handle(UndoRetweetCommand request, CancellationToken cancellationToken)
        {
            var tweet = await _tweetService.GetTweetById(request.RetweetId!);
            TweetVm retweetVm = _tweetDocumentMapper.map(tweet!);
            await _retweetService.DeleteRetweet(request.RetweetId!, request.UserId!);
            await UpdateExistingTweet(retweetVm, request.Id!, request);
            return Unit.Value;
        }

        private async Task UpdateExistingTweet(TweetVm retweet, string postId, UndoRetweetCommand request)
        {
            retweet.RetweetUsers!.Remove(retweet.UserId);
            retweet.RetweetPosts!.Remove(postId);

            Tweet updatedTweet = new Tweet()
            {
                Id = retweet.Id,
                Text = retweet.Text,
                Hashtags = retweet.Hashtags,
                UserId = retweet.UserId,
                IsRetweet = retweet.IsRetweet,
                RetweetId = retweet.RetweetId,
                Likes = new List<string>(retweet.Likes!),
                Comments = new List<string>(retweet.Comments!),
                RetweetPosts = new List<string>(retweet.RetweetPosts!),
                RetweetUsers = new List<string>(retweet.RetweetUsers!),
                CreatedAt = retweet.CreatedAt
            };

            await _tweetService.UpdateTweet(updatedTweet);
        }
    }
}
