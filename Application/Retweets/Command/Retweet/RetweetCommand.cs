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
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetDocumentMapper;

        public RetweetCommandHandler(
            ITweetService tweetService,
            IBsonDocumentMapper<TweetVm> tweetDocumentMapper
        )
        {
            _tweetService = tweetService;
            _tweetDocumentMapper = tweetDocumentMapper;
        }

        public async Task<RetweetVm> Handle(
            RetweetCommand request,
            CancellationToken cancellationToken
        )
        {
            var tweet = await _tweetService.GetTweetById(request.OriginalTweetId!);

            TweetVm retweetVm = _tweetDocumentMapper.map(tweet!);

            var newRetweet = await CreateNewRetweet(request);
            await UpdateOriginalTweet(retweetVm, newRetweet, request);

            return new RetweetVm() { Id = newRetweet };
        }

        private async Task<string> CreateNewRetweet(RetweetCommand request)
        {
            Tweet tweet = new Tweet()
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

            await _tweetService.PublishTweet(tweet);
            return tweet.Id;
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
