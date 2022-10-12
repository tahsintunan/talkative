using Application.Interface;
using Application.ViewModels;
using MediatR;
using MongoDB.Bson;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Commands.LikeTweetCommand
{
    public class LikeTweetCommand:IRequest
    {
        public string? TweetId { get; set; }
        public string? UserId { get; set; }
        public bool IsLiked { get; set; }
    }

    public class LikeTweetCommandHandler : IRequestHandler<LikeTweetCommand>
    {

        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonDocumentMapper;
        public LikeTweetCommandHandler(ITweetService tweetService, IBsonDocumentMapper<TweetVm> tweetBsonDocumentMapper)
        {
            _tweetService = tweetService;
            _tweetBsonDocumentMapper = tweetBsonDocumentMapper;
        }

        public async Task<Unit> Handle(LikeTweetCommand request, CancellationToken cancellationToken)
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
                    Text =  tweetVm.Text,
                    Hashtags = tweetVm.Hashtags,
                    UserId = request.UserId,
                    IsRetweet = tweetVm.IsRetweet,
                    RetweetId = tweetVm.IsRetweet ? tweetVm.RetweetId : null,
                    Likes = tweetVm.Likes!,
                    Comments = new List<string>(tweetVm.Comments!),
                    CreatedAt = tweetVm.CreatedAt
                };

                await _tweetService.UpdateTweet(updatedTweet);
            }
            return Unit.Value;
        }
    }
}
