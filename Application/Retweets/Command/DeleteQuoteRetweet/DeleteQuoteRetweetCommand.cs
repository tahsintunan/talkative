using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Retweets.Command.DeleteQuoteRetweet
{
    public class DeleteQuoteRetweetCommand : IRequest
    {
        public string? TweetId { get; set; }
        public string? OriginalTweetId { get; set; }
        public string? UserId { get; set; }
    }

    public class DeleteQuoteRetweetCommandHandler : IRequestHandler<DeleteQuoteRetweetCommand>
    {
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonMapper;

        public DeleteQuoteRetweetCommandHandler(
            ITweetService tweetService,
            IBsonDocumentMapper<TweetVm> tweetBsonMapper
        )
        {
            _tweetService = tweetService;
            _tweetBsonMapper = tweetBsonMapper;
        }

        public async Task<Unit> Handle(
            DeleteQuoteRetweetCommand request,
            CancellationToken cancellationToken
        )
        {
            await UpdateOriginalTweet(request);
            await _tweetService.DeleteTweet(request.TweetId!);
            return Unit.Value;
        }

        public async Task UpdateOriginalTweet(DeleteQuoteRetweetCommand request)
        {
            var originalTweet = await _tweetService.GetTweetById(request.OriginalTweetId!);

            if (originalTweet == null)
                return;

            var originalTweetVm = _tweetBsonMapper.map(originalTweet);

            originalTweetVm.QuoteRetweets!.Remove(request.TweetId);

            await _tweetService.PartialUpdate(
                originalTweetVm.Id!,
                Builders<Tweet>.Update.Set(x => x.QuoteRetweets, originalTweetVm.QuoteRetweets!)
            );
        }
    }
}
