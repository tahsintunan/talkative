using Application.Common.ViewModels;
using Application.Tweets.Queries.SearchTweetsByHashtag;
using Application.Users.Queries.SearchUsers;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class SearchController : ApiControllerBase
    {
        [HttpGet("/hashtag/{hashtag}")]
        public async Task<ActionResult<IList<TweetVm>>> SearchHastag(string hashtag)
        {
            return Ok(await Mediator.Send(new SearchTweetsByHashtagQuery() { Hashtag = hashtag }));
        }

        [HttpGet("/user/{username}")]
        public async Task<ActionResult<IList<UserVm>>> SearchUser(string username)
        {
            return Ok(await Mediator.Send(new SearchUserQuery() { Username = username }));
        }
    }
}
