using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Tweets.Commands.LikeTweet
{
    public class LikeTweetCommand : IRequest
    {
        public string? TweetId { get; set; }
        public string? UserId { get; set; }
        public bool IsLiked { get; set; }
    }

    public class LikeTweetCommandHandler : IRequestHandler<LikeTweetCommand>
    {
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonDocumentMapper;
        private readonly INotificationService _notificationService;

        public LikeTweetCommandHandler(
            ITweetService tweetService,
            IBsonDocumentMapper<TweetVm> tweetBsonDocumentMapper,
            INotificationService notificationService
        )
        {
            _tweetService = tweetService;
            _tweetBsonDocumentMapper = tweetBsonDocumentMapper;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(
            LikeTweetCommand request,
            CancellationToken cancellationToken
        )
        {
            var currentTweet = await _tweetService.GetTweetById(request.TweetId!);
            var tweetVm = _tweetBsonDocumentMapper.map(currentTweet!);

            if (request.IsLiked)
            {
                tweetVm.Likes!.Add(request.UserId);
                await _notificationService.TriggerLikeTweetNotification(request, tweetVm);
            }
            else
            {
                tweetVm.Likes!.Remove(request.UserId);
            }

            await _tweetService.PartialUpdate(
                tweetVm!.Id!,
                Builders<Tweet>.Update.Set(p => p.Likes, new List<string>(tweetVm.Likes!))
            );

            return Unit.Value;
        }
    }
}
