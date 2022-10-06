using AutoMapper;
using MediatR;
using MongoDB.Bson;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Commands.PublishTweetCommand
{
    public class PublishTweetCommand:IRequest<TweetVm>
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? UserId { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetId { get; set; }
    }

    public class PublishTweetCommandHandler : IRequestHandler<PublishTweetCommand, TweetVm>
    {
        private readonly ITweetService _tweetService;
        private readonly IMapper _mapper;
        public PublishTweetCommandHandler(ITweetService tweetService,IMapper mapper)
        {
            _tweetService = tweetService;
            _mapper = mapper;
        }
        public async Task<TweetVm> Handle(PublishTweetCommand request, CancellationToken cancellationToken)
        {
            Tweet tweet = new Tweet()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = request.Text,
                UserId = request.UserId,
                Hashtags = new List<string>(request.Hashtags!),
                IsRetweet = request.IsRetweet,
                RetweetId = request.IsRetweet ? request.RetweetId :null ,
                Likes = new List<string>(),
                CommentId = new List<string>(),
                CreatedAt = DateTime.Now
            };
            await _tweetService.PublishTweet(tweet);
            return _mapper.Map<TweetVm>(tweet);
        }
    }
}
