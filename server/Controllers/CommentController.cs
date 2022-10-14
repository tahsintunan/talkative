using Application.Comments.Commands.CreateComment;
using Application.Comments.Commands.DeleteComment;
using Application.Comments.Commands.LikeComment;
using Application.Comments.Commands.UpdateComment;
using Application.Comments.Queries.GetCommentById;
using Application.Comments.Queries.GetCommentsByTweetId;
using Application.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class CommentController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentCommand createCommentCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            createCommentCommand.UserId = userId;
            var createCommandVm = await Mediator.Send(createCommentCommand);
            return Ok(await Mediator.Send(new GetCommentByIdQuery() { Id = createCommandVm.Id }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetCommentByIdQuery() { Id = id }));
        }

        [HttpGet("tweet/{id}")]
        public async Task<ActionResult<IList<CommentVm>>> GetByTweetId(string id)
        {
            return Ok(await Mediator.Send(new GetCommentsByTweetIdQuery() { TweetId = id }));
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(UpdateCommentCommand updateCommentCommand)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            updateCommentCommand.UserId = userId;
            await Mediator.Send(updateCommentCommand);
            return Ok(
                await Mediator.Send(new GetCommentByIdQuery() { Id = updateCommentCommand.Id })
            );
        }

        [HttpPatch("like/{id}")]
        public async Task<IActionResult> LikeComment(string id)
        {
            var userId = HttpContext.Items["User"]!.ToString();
            await Mediator.Send(new LikeCommentCommand() { Id = id, UserId = userId });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteCommentCommand() { Id = id });
            return NoContent();
        }
    }
}
