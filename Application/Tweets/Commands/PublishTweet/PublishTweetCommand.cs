using Application.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace Application.Tweets.Commands.PublishTweetCommand
{
    public class PublishTweetCommand:IRequest<PublishTweetVm>
    {
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? UserId { get; set; }
    }

    public class PublishTweetCommandHandler : IRequestHandler<PublishTweetCommand, PublishTweetVm>
    {
        private readonly ITweetService _tweetService;
        public PublishTweetCommandHandler(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }
        public async Task<PublishTweetVm> Handle(PublishTweetCommand request, CancellationToken cancellationToken)
        {
            var generatedId = ObjectId.GenerateNewId().ToString();
            Tweet tweet = new Tweet()
            {
                Id = generatedId,
                Text = request.Text,
                UserId = request.UserId,
                Hashtags = new List<string>(request.Hashtags!),
                IsRetweet = false,
                RetweetId = null,
                Likes = new List<string>(),
                Comments = new List<string>(),
                CreatedAt = DateTime.Now,
                RetweetUsers = new List<string>(),
                RetweetPosts = new List<string>()
            };
            await _tweetService.PublishTweet(tweet);

            return new PublishTweetVm() { Id = generatedId };
        }
    }
}
