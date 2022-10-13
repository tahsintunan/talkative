using Application.Common.ViewModels;
using Application.Tweets.Commands.DeleteTweet;
using Application.Tweets.Commands.LikeTweet;
using Application.Tweets.Commands.PublishTweet;
using Application.Tweets.Commands.UpdateTweet;
using Application.Tweets.Queries.GetTweetByIdQuery;
using Application.Tweets.Queries.GetTweetsOfSingleUserQuery;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class TweetController : ApiControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<TweetVm>> PublishTweet(PublishTweetCommand publishTweetCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            publishTweetCommand.UserId = userId;
            var tweet = await Mediator.Send(publishTweetCommand);
            return CreatedAtAction(nameof(GetTweetById), new { id = tweet.Id }, await Mediator.Send(new GetTweetByIdQuery() { Id = tweet.Id }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TweetVm>> GetTweetById(string id)
        {
            return Ok(await Mediator.Send(new GetTweetByIdQuery() { Id = id }));
        }

        [HttpGet("user/current-user")]
        public async Task<ActionResult<List<TweetVm>>> GetTweetOfCurrentUser()
        {
            var userId = HttpContext.Items["User"]!.ToString();
            return Ok(await Mediator.Send(new GetTweetsOfSingleUserQuery() { UserId = userId }));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<TweetVm>>> GetTweetOfAUser(string userId)
        {
            return Ok(await Mediator.Send(new GetTweetsOfSingleUserQuery() { UserId = userId }));
        }

        [HttpPut]
        public async Task<ActionResult<TweetVm>> UpdateTweet(UpdateTweetCommand updateTweetCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            updateTweetCommand.UserId = userId;
            await Mediator.Send(updateTweetCommand);
            return Ok(await Mediator.Send(new GetTweetByIdQuery() { Id = updateTweetCommand.Id }));
        }

        [HttpPut("like")]
        public async Task<IActionResult> LikeTweet(LikeTweetCommand likeTweetCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            likeTweetCommand.UserId = userId;
            await Mediator.Send(likeTweetCommand);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTweet(string id)
        {
            await Mediator.Send(new DeleteTweetCommand() { Id = id });
            return NoContent();
        }
    }
}
