using Application.Common.ViewModels;
using Application.Tweets.Commands.DeleteTweet;
using Application.Tweets.Commands.LikeTweet;
using Application.Tweets.Commands.PublishTweet;
using Application.Tweets.Commands.UpdateTweet;
using Application.Tweets.Queries.GetTrendingHashtags;
using Application.Tweets.Queries.GetTweetById;
using Application.Tweets.Queries.GetTweetsOfSingleUser;
using Application.Tweets.Queries.TweetsForFeed;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

public class TweetController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TweetVm>> PublishTweet(
        PublishTweetCommand publishTweetCommand
    )
    {
        var userId = HttpContext.Items["User"]!.ToString();
        publishTweetCommand.UserId = userId;
        var tweet = await Mediator.Send(publishTweetCommand);
        return CreatedAtAction(
            nameof(GetTweetById),
            new { id = tweet.Id },
            await Mediator.Send(new GetTweetByIdQuery { Id = tweet.Id })
        );
    }

    [HttpGet("feed")]
    public async Task<ActionResult<IList<TweetVm>>> GetTweetForFeed(
        [FromQuery] TweetsForFeedQuery tweetsForFeedQuery
    )
    {
        tweetsForFeedQuery.UserId = HttpContext.Items["User"]!.ToString();
        return Ok(await Mediator.Send(tweetsForFeedQuery));
    }

    [HttpGet("trending-hashtags")]
    public async Task<ActionResult<IList<TrendingHashtagVm>>> GetTrendingHashtags()
    {
        return Ok(await Mediator.Send(new GetTrendingHashtagsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TweetVm>> GetTweetById(string id)
    {
        GetTweetByIdQuery getTweetByIdQuery = new() { Id = id, UserId = HttpContext.Items["User"]!.ToString() };
        return Ok(await Mediator.Send(getTweetByIdQuery));
    }

    [HttpGet("user/current-user")]
    public async Task<ActionResult<List<TweetVm>>> GetTweetOfCurrentUser(
        [FromQuery] GetTweetsOfSingleUserQuery getTweetsOfSingleUserQuery
    )
    {
        getTweetsOfSingleUserQuery.UserId = HttpContext.Items["User"]!.ToString();
        getTweetsOfSingleUserQuery.CurrentUserId = HttpContext.Items["User"]!.ToString();
        return Ok(await Mediator.Send(getTweetsOfSingleUserQuery));
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<TweetVm>>> GetTweetOfAUser(
        string userId,
        [FromQuery] GetTweetsOfSingleUserQuery getTweetsOfSingleUserQuery
    )
    {
        getTweetsOfSingleUserQuery.CurrentUserId = HttpContext.Items["User"]!.ToString();
        getTweetsOfSingleUserQuery.UserId = userId;
        return Ok(await Mediator.Send(getTweetsOfSingleUserQuery));
    }

    [HttpPut]
    public async Task<ActionResult<TweetVm>> UpdateTweet(UpdateTweetCommand updateTweetCommand)
    {
        var userId = HttpContext.Items["User"]!.ToString();
        updateTweetCommand.UserId = userId;
        await Mediator.Send(updateTweetCommand);
        return Ok(await Mediator.Send(new GetTweetByIdQuery { Id = updateTweetCommand.Id }));
    }

    [HttpPut("like")]
    public async Task<IActionResult> LikeTweet(LikeTweetCommand likeTweetCommand)
    {
        var userId = HttpContext.Items["User"]!.ToString();
        likeTweetCommand.UserId = userId;
        await Mediator.Send(likeTweetCommand);
        return Ok(
            await Mediator.Send(new GetTweetByIdQuery { Id = likeTweetCommand.TweetId })
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTweet(string id)
    {
        await Mediator.Send(new DeleteTweetCommand { Id = id });
        return NoContent();
    }
}