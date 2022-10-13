using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace Application.Retweets.Command.Retweet
{
    public class RetweetCommand : IRequest<RetweetVm>
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? RetweetId { get; set; }
        public string? UserId { get; set; }
    }

    public class RetweetCommandHandler : IRequestHandler<RetweetCommand, RetweetVm>
    {
        private readonly ITweetService _tweetService;
        private readonly IRetweetService _retweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetDocumentMapper;

        public RetweetCommandHandler(ITweetService tweetService, IBsonDocumentMapper<TweetVm> tweetDocumentMapper, IRetweetService retweetService)
        {
            _tweetService = tweetService;
            _tweetDocumentMapper = tweetDocumentMapper;
            _retweetService = retweetService;
        }

        public async Task<RetweetVm> Handle(RetweetCommand request, CancellationToken cancellationToken)
        {
            var tweet = await _tweetService.GetTweetById(request.RetweetId!);
            TweetVm retweetVm = _tweetDocumentMapper.map(tweet!);

            var newRetweet = await CreateNewRetweet(request);
            await UpdateExistingTweet(retweetVm, newRetweet, request);

            return new RetweetVm() { Id = newRetweet };
        }

        private async Task<string> CreateNewRetweet(RetweetCommand request)
        {
            Tweet tweet = new Tweet()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = request.Text,
                Hashtags = request.Hashtags != null ? new List<string>(request.Hashtags) : new List<string>(),
                RetweetId = request.RetweetId,
                IsRetweet = true,
                UserId = request.UserId,
                Likes = new List<string>(),
                Comments = new List<string>(),
                RetweetPosts = new List<string>(),
                RetweetUsers = new List<string>(),
                CreatedAt = DateTime.Now,

            };

            await _tweetService.PublishTweet(tweet);
            return tweet.Id;
        }

        private async Task UpdateExistingTweet(TweetVm retweet, string postId, RetweetCommand request)
        {
            retweet.RetweetUsers!.Add(retweet.UserId);
            retweet.RetweetPosts!.Add(postId);

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
