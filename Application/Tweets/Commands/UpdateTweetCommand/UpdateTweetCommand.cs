using MediatR;
using server.Application.Interface;
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
            if(currentTweet != null)
            {
                Tweet updatedTweet = new Tweet()
                {
                    Id = request.Id,
                    Text = request.Text ?? currentTweet.Text,
                    UserId = currentTweet.UserId,
                    Hashtags = request.Hashtags ?? currentTweet.Hashtags,
                    IsRetweet = currentTweet.IsRetweet,
                    RetweetId = currentTweet.RetweetId,
                    Likes = new List<string>(currentTweet.Likes!),
                    CommentId = new List<string>(currentTweet.CommentId!),
                    CreatedAt = currentTweet.CreatedAt
                };

                await _tweetService.UpdateTweet(updatedTweet);
            }
            return Unit.Value;
        }
    }
}
