using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Commands.UpdateTweetCommand
{
    public class UpdateTweetCommand:IRequest
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? UserId { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetId { get; set; }
    }

    public class UpdateTweetCommandHandler : IRequestHandler<UpdateTweetCommand>
    {
        private readonly ITweetService _tweetService;
        public UpdateTweetCommandHandler(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        public async Task<Unit> Handle(UpdateTweetCommand request, CancellationToken cancellationToken)
        {
            var currentTweet = await _tweetService.GetTweetById(request.Id!);
            var tweetVm = BsonSerializer.Deserialize<TweetVm>(currentTweet);
            if(currentTweet != null)
            {
                Tweet updatedTweet = new Tweet()
                {
                    Id = request.Id,
                    Text = request.Text ?? tweetVm.Text,
                    Hashtags = request.Hashtags ?? tweetVm.Hashtags,
                    UserId = ObjectId.Parse(tweetVm.UserId),
                    IsRetweet = tweetVm.IsRetweet,
                    RetweetId = ObjectId.Parse(tweetVm.RetweetId),
                    Likes = new List<string>(tweetVm.Likes!),
                    CommentId = new List<string>(tweetVm.CommentId!),
                    CreatedAt = tweetVm.CreatedAt
                };

                await _tweetService.UpdateTweet(updatedTweet);
            }
            return Unit.Value;
        }
    }
}
