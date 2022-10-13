using Application.Common.ViewModels;
using Application.Retweets.Command.Retweet;
using Application.Retweets.Command.UndoRetweet;
using Application.Retweets.Query.GetAllRetweetQuery;
using Application.Tweets.Queries.GetTweetByIdQuery;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class RetweetController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<List<TweetVm>>> GetRetweetsOfTweet(string id)
        {
            return Ok(await Mediator.Send(new GetAllRetweetQuery() { RetweetId = id }));
        }

        [HttpPost]
        public async Task<ActionResult<TweetVm>> Retweet(RetweetCommand retweetCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            retweetCommand.UserId = userId;
            var retweet = await Mediator.Send(retweetCommand);
            return Ok(await Mediator.Send(new GetTweetByIdQuery() { Id = retweet.Id }));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(UndoRetweetCommand undoRetweetCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            undoRetweetCommand.UserId = userId;
            await Mediator.Send(undoRetweetCommand);
            return NoContent();
        }
    }
}
