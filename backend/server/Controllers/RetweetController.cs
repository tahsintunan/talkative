﻿using Application.Common.ViewModels;
using Application.Retweets.Command.DeleteQuoteRetweet;
using Application.Retweets.Command.Retweet;
using Application.Retweets.Command.UndoRetweet;
using Application.Tweets.Queries.GetTweetById;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

public class RetweetController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TweetVm>> Retweet(RetweetCommand retweetCommand)
    {
        var userId = HttpContext.Items["User"]!.ToString();
        retweetCommand.UserId = userId;
        var retweet = await Mediator.Send(retweetCommand);
        return Ok(await Mediator.Send(new GetTweetByIdQuery { Id = retweet.Id }));
    }

    [HttpDelete("{originalTweetId}")]
    public async Task<ActionResult> UndoRetweet(string originalTweetId)
    {
        var userId = HttpContext.Items["User"]!.ToString();
        var undoRetweetCommand = new UndoRetweetCommand
        {
            UserId = userId,
            OriginalTweetId = originalTweetId
        };
        await Mediator.Send(undoRetweetCommand);
        return NoContent();
    }

    [HttpDelete("quote-retweet")]
    public async Task<ActionResult> DeleteQuoteRetweet(
        [FromQuery] DeleteQuoteRetweetCommand deleteQuoteRetweetCommand
    )
    {
        var userId = HttpContext.Items["User"]!.ToString();
        deleteQuoteRetweetCommand.UserId = userId;
        await Mediator.Send(deleteQuoteRetweetCommand);
        return NoContent();
    }
}