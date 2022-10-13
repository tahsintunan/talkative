using Application.Interface;
using Application.ViewModels;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace Application.Tweets.Commands.LikeTweetCommand
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

        public LikeTweetCommandHandler(
            ITweetService tweetService,
            IBsonDocumentMapper<TweetVm> tweetBsonDocumentMapper
        )
        {
            _tweetService = tweetService;
            _tweetBsonDocumentMapper = tweetBsonDocumentMapper;
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
            }
            else
            {
                tweetVm.Likes!.Remove(request.UserId);
            }

            if (currentTweet != null)
            {
                Tweet updatedTweet = new Tweet()
                {
                    Id = tweetVm.Id,
                    Text = tweetVm.Text,
                    Hashtags = tweetVm.Hashtags,
                    UserId = tweetVm.UserId,
                    IsRetweet = tweetVm.IsRetweet,
                    RetweetId = tweetVm.RetweetId,
                    Likes = tweetVm.Likes!,
                    Comments = new List<string>(tweetVm.Comments!),
                    CreatedAt = tweetVm.CreatedAt,
                    RetweetPosts = new List<string>(tweetVm.RetweetPosts!),
                    RetweetUsers = new List<string>(tweetVm.RetweetUsers!),
                };

                await _tweetService.UpdateTweet(updatedTweet);
            }
            return Unit.Value;
        }
    }
}
