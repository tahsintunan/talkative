using Application.Tweets.Queries.GetTweetByIdQuery;
using AutoMapper;
using MediatR;
using MongoDB.Bson;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Commands.PublishTweetCommand
{
    public class PublishTweetCommand:IRequest<TweetVm?>
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public IList<string>? Hashtags { get; set; }
        public string? UserId { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetId { get; set; }
    }

    public class PublishTweetCommandHandler : IRequestHandler<PublishTweetCommand, TweetVm?>
    {
        private readonly ITweetService _tweetService;
        private readonly IMediator _mediator;
        public PublishTweetCommandHandler(ITweetService tweetService,IMediator mediator)
        {
            _tweetService = tweetService;
            _mediator = mediator;
        }
        public async Task<TweetVm?> Handle(PublishTweetCommand request, CancellationToken cancellationToken)
        {
            var generatedId = ObjectId.GenerateNewId().ToString();
            Tweet tweet = new Tweet()
            {
                Id = generatedId,
                Text = request.Text,
                UserId = ObjectId.Parse(request.UserId),
                Hashtags = new List<string>(request.Hashtags!),
                IsRetweet = request.IsRetweet,
                RetweetId = request.IsRetweet ? ObjectId.Parse(request.RetweetId) : null ,
                Likes = new List<string>(),
                Comments = new List<string>(),
                CreatedAt = DateTime.Now
            };
            await _tweetService.PublishTweet(tweet);
            return await _mediator.Send(new GetTweetByIdQuery() { Id = generatedId});
        }
    }
}
