using Application.Interface;
using Application.ViewModels;
using Domain.Entities;
using MediatR;

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
        public string? RetweetUser { get; set; }
    }

    public class UpdateTweetCommandHandler : IRequestHandler<UpdateTweetCommand>
    {
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonMapper;
        public UpdateTweetCommandHandler(ITweetService tweetService,IBsonDocumentMapper<TweetVm> tweetBsonMapper)
        {
            _tweetService = tweetService;
            _tweetBsonMapper = tweetBsonMapper;
        }

        public async Task<Unit> Handle(UpdateTweetCommand request, CancellationToken cancellationToken)
        {
            var currentTweet = await _tweetService.GetTweetById(request.Id!);
            var tweetVm = _tweetBsonMapper.map(currentTweet!);
            if(currentTweet != null && request.UserId == tweetVm.UserId)
            {
                Tweet updatedTweet = new Tweet()
                {
                    Id = request.Id,
                    Text = request.Text ?? tweetVm.Text,
                    Hashtags = request.Hashtags ?? tweetVm.Hashtags,
                    UserId = request.UserId,
                    IsRetweet = tweetVm.IsRetweet,
                    RetweetId = tweetVm.IsRetweet ? request.RetweetId : null,
                    Likes = new List<string>(tweetVm.Likes!),
                    Comments = new List<string>(tweetVm.Comments!),
                    RetweetPosts = new List<string>(tweetVm.RetweetPosts!),
                    RetweetUsers = new List<string>(tweetVm.RetweetUsers!),
                    CreatedAt = tweetVm.CreatedAt
                };

                await _tweetService.UpdateTweet(updatedTweet);
            }
            return Unit.Value;
        }

    }
}
