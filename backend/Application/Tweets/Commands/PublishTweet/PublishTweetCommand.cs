using System.Text.Json.Serialization;
using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace Application.Tweets.Commands.PublishTweet;

public class PublishTweetCommand : IRequest<PublishTweetVm>
{
    public string? Text { get; set; }
    public IList<string>? Hashtags { get; set; }

    [JsonIgnore] public string? UserId { get; set; }
}

public class PublishTweetCommandHandler : IRequestHandler<PublishTweetCommand, PublishTweetVm>
{
    private readonly ITweet _tweetService;

    public PublishTweetCommandHandler(ITweet tweetService)
    {
        _tweetService = tweetService;
    }

    public async Task<PublishTweetVm> Handle(
        PublishTweetCommand request,
        CancellationToken cancellationToken
    )
    {
        var generatedId = ObjectId.GenerateNewId().ToString();
        var tweet = new Tweet
        {
            Id = generatedId,
            Text = request.Text,
            UserId = request.UserId,
            Hashtags = request.Hashtags != null ? new List<string>(request.Hashtags) : null,
            IsRetweet = false,
            OriginalTweetId = null,
            Likes = new List<string>(),
            Comments = new List<string>(),
            CreatedAt = DateTime.Now,
            RetweetUsers = new List<string>(),
            QuoteRetweets = new List<string>()
        };
        await _tweetService.PublishTweet(tweet);

        return new PublishTweetVm { Id = generatedId };
    }
}