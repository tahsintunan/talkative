using MediatR;
using MongoDB.Bson;
using server.Application.Interface;
using server.Domain.Entities;

namespace Application.Tweets.Commands.PublishTweetCommand
{
    public class PublishTweetCommand:IRequest
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? UserId { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetId { get; set; }
    }

    public class PublishTweetCommandHandler : IRequestHandler<PublishTweetCommand>
    {
        private readonly ITweetService _tweetService;
        public PublishTweetCommandHandler(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }
        public async Task<Unit> Handle(PublishTweetCommand request, CancellationToken cancellationToken)
        {
            Tweet tweet = new Tweet()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = request.Text,
                UserId = request.UserId,
                Hashtags = new List<string>(request.Hashtags!),
                IsRetweet = request.IsRetweet,
                RetweetId = request.IsRetweet ? null : request.RetweetId,
                Likes = new List<string>(),
                CommentId = new List<string>(),
                CreatedAt = DateTime.Now
            };
            await _tweetService.PublishTweet(tweet);   
            return Unit.Value;
        }
    }
}
