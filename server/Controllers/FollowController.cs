using Application.Common.ViewModels;
using Application.Followers.Commands.AddFollower;
using Application.Followers.Commands.DeleteFollower;
using Application.Followers.Queries.GetFollowers;
using Application.Followers.Queries.GetFollowings;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class FollowController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddFollower(AddFollowerCommand addFollowerCommand)
        {
            addFollowerCommand.FollowerId = HttpContext.Items["User"]!.ToString();
            await Mediator.Send(addFollowerCommand);
            return NoContent();
        }

        [HttpGet("follower")]
        public async Task<ActionResult<IList<UserVm>>> GetFollowersOfCurrentUser(
            GetFollowersQuery getFollowersQuery
        )
        {
            getFollowersQuery.UserId = HttpContext.Items["User"]!.ToString();
            return Ok(await Mediator.Send(getFollowersQuery));
        }

        [HttpGet("following")]
        public async Task<ActionResult<IList<UserVm>>> GetFollowingsOfCurrentUser(
            [FromQuery] GetFollowingsQuery getFollowingsQuery
        )
        {
            getFollowingsQuery.UserId = HttpContext.Items["User"]!.ToString();
            return Ok(await Mediator.Send(getFollowingsQuery));
        }

        [HttpGet("follower/{id}")]
        public async Task<ActionResult<IList<UserVm>>> GetFollowersOfUser(
            string id,
            [FromQuery] GetFollowersQuery getFollowersQuery
        )
        {
            getFollowersQuery.UserId = id;
            return Ok(await Mediator.Send(getFollowersQuery));
        }

        [HttpGet("following/{id}")]
        public async Task<ActionResult<IList<UserVm>>> GetFollowingsOfUser(
            string id,
            [FromQuery] GetFollowingsQuery getFollowingsQuery
        )
        {
            getFollowingsQuery.UserId = id;
            return Ok(await Mediator.Send(getFollowingsQuery));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unfollow(string id)
        {
            string? userId = HttpContext.Items["User"]!.ToString();
            string followerId = userId!;
            await Mediator.Send(
                new DeleteFollowerCommand() { FollowerId = followerId, FollowingId = id }
            );
            return NoContent();
        }
    }
}
