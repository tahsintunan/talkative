using Application.Tweets.Commands.DeleteTweetCommand;
using Application.Tweets.Commands.LikeTweetCommand;
using Application.Tweets.Commands.PublishTweetCommand;
using Application.Tweets.Commands.UpdateTweetCommand;
using Application.Tweets.Queries.GetTweetByIdQuery;
using Application.Tweets.Queries.GetTweetsOfSingleUserQuery;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> PublishTweet(PublishTweetCommand publishTweetCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            publishTweetCommand.UserId = userId;
            var tweet = await Mediator.Send(publishTweetCommand);
            return CreatedAtAction(nameof(GetTweetById), new { id = tweet.Id }, tweet);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTweetById(string id)
        {
            return Ok(await Mediator.Send(new GetTweetByIdQuery() { Id = id }));
        }

        [HttpGet("user/current-user")]
        public async Task<ActionResult> GetTweetOfCurrentUser()
        {
            var userId = HttpContext.Items["User"]!.ToString();
            return Ok(await Mediator.Send(new GetTweetsOfSingleUserQuery() { UserId = userId }));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetTweetOfAUser(string userId)
        {
            return Ok(await Mediator.Send(new GetTweetsOfSingleUserQuery() { UserId = userId }));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTweet(UpdateTweetCommand updateTweetCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            updateTweetCommand.UserId = userId;
            return Ok(await Mediator.Send(updateTweetCommand));
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
