using Application.Common.ViewModels;
using Application.Tweets.Queries.SearchHashtags;
using Application.Tweets.Queries.SearchTweetsByHashtag;
using Application.Users.Queries.SearchUsers;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class SearchController : ApiControllerBase
    {
        [HttpGet("tweet/{hashtag}")]
        public async Task<ActionResult<IList<TweetVm>>> SearchHastag(
            string hashtag,
            [FromQuery] SearchTweetsByHashtagQuery searchTweetsByHashtagQuery
        )
        {
            searchTweetsByHashtagQuery.Hashtag = hashtag;
            return Ok(await Mediator.Send(searchTweetsByHashtagQuery));
        }

        [HttpGet("hashtag/{hashtag}")]
        public async Task<ActionResult<SearchHashtagsVm>> GetHashtags(
            string hashtag,
            [FromQuery] SearchHashtagsQuery searchHashtagsQuery
        )
        {
            searchHashtagsQuery.Hashtag = hashtag;
            return Ok(await Mediator.Send(searchHashtagsQuery));
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<IList<UserVm>>> SearchUser(
            string username,
            [FromQuery] SearchUserQuery searchUserQuery
        )
        {
            searchUserQuery.Username = username;
            return Ok(await Mediator.Send(searchUserQuery));
        }
    }
}
