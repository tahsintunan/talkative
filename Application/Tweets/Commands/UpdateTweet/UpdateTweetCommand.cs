using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Tweets.Commands.UpdateTweet
{
    public class UpdateTweetCommand : IRequest
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }

    public class UpdateTweetCommandHandler : IRequestHandler<UpdateTweetCommand>
    {
        private readonly ITweet _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonMapper;

        public UpdateTweetCommandHandler(
            ITweet tweetService,
            IBsonDocumentMapper<TweetVm> tweetBsonMapper
        )
        {
            _tweetService = tweetService;
            _tweetBsonMapper = tweetBsonMapper;
        }

        public async Task<Unit> Handle(
            UpdateTweetCommand request,
            CancellationToken cancellationToken
        )
        {
            var currentTweet = await _tweetService.GetTweetById(request.Id!);
            var tweetVm = _tweetBsonMapper.map(currentTweet!);
            if (currentTweet != null && request.UserId == tweetVm.UserId)
            {
                var existingTweet = await _tweetService.GetTweetById(request.Id!);

                if (existingTweet == null)
                    return Unit.Value;

                var existingTweetVm = _tweetBsonMapper.map(existingTweet);

                if (existingTweetVm.IsRetweet)
                    return Unit.Value;

                await _tweetService.PartialUpdate(
                    request.Id!,
                    Builders<Tweet>.Update
                        .Set(p => p.Text, request.Text)
                        .Set(p => p.Hashtags, request.Hashtags)
                );
            }
            return Unit.Value;
        }
    }
}
