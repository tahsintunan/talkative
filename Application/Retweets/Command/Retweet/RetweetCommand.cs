using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Retweets.Command.Retweet
{
    public class RetweetCommand : IRequest<RetweetVm>
    {
        public string? Text { get; set; }
        public bool IsQuoteRetweet { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? OriginalTweetId { get; set; }
        public string? UserId { get; set; }
    }

    public class RetweetCommandHandler : IRequestHandler<RetweetCommand, RetweetVm>
    {
        private readonly ITweet _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetDocumentMapper;
        private readonly Common.Interface.INotification _notificationService;

        public RetweetCommandHandler(
            ITweet tweetService,
            IBsonDocumentMapper<TweetVm> tweetDocumentMapper,
            Common.Interface.INotification notificationService
        )
        {
            _tweetService = tweetService;
            _tweetDocumentMapper = tweetDocumentMapper;
            _notificationService = notificationService;
        }

        public async Task<RetweetVm> Handle(
            RetweetCommand request,
            CancellationToken cancellationToken
        )
        {
            var originalTweet = await _tweetService.GetTweetById(request.OriginalTweetId!);
            var originalTweetVm = _tweetDocumentMapper.map(originalTweet!);
            var newRetweet = await CreateNewRetweet(request);
            await UpdateOriginalTweet(originalTweetVm, newRetweet.Id!, request);
            await _notificationService.TriggerRetweetNotification(newRetweet, originalTweetVm);
            return new RetweetVm() { Id = newRetweet.Id };
        }

        private async Task<Tweet> CreateNewRetweet(RetweetCommand request)
        {
            var retweet = new Tweet()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = request.IsQuoteRetweet ? request.Text : null,
                Hashtags =
                    request.Hashtags == null || !request.IsQuoteRetweet
                        ? new List<string>()
                        : new List<string>(request.Hashtags),
                OriginalTweetId = request.OriginalTweetId,
                IsRetweet = !request.IsQuoteRetweet,
                IsQuoteRetweet = request.IsQuoteRetweet,
                UserId = request.UserId,
                Likes = new List<string>(),
                Comments = new List<string>(),
                QuoteRetweets = new List<string>(),
                RetweetUsers = new List<string>(),
                CreatedAt = DateTime.Now,
            };

            await _tweetService.PublishTweet(retweet);
            return retweet;
        }

        private async Task UpdateOriginalTweet(
            TweetVm retweet,
            string postId,
            RetweetCommand request
        )
        {
            if (request.IsQuoteRetweet)
            {
                retweet.QuoteRetweets!.Add(postId);
            }
            else
            {
                retweet.RetweetUsers!.Add(request.UserId);
            }

            await _tweetService.PartialUpdate(
                retweet.Id!,
                Builders<Tweet>.Update
                    .Set(x => x.QuoteRetweets, retweet.QuoteRetweets!)
                    .Set(x => x.RetweetUsers, retweet.RetweetUsers!)
            );
        }
    }
}
