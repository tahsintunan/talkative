﻿using Application.Common.Interface;
using MediatR;

namespace Application.Tweets.Commands.DeleteTweet;

public class DeleteTweetCommand : IRequest
{
    public string? Id { get; set; }
}

public class DeleteTweetCommandHandler : IRequestHandler<DeleteTweetCommand>
{
    private readonly ITweet _tweetService;

    public DeleteTweetCommandHandler(ITweet tweetService)
    {
        _tweetService = tweetService;
    }

    public async Task<Unit> Handle(
        DeleteTweetCommand request,
        CancellationToken cancellationToken
    )
    {
        await _tweetService.DeleteTweet(request.Id!);
        return Unit.Value;
    }
}