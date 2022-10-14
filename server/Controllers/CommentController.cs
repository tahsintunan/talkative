using Application.Comments.Commands.CreateComment;
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
            return Ok(await Mediator.Send(createCommentCommand));
        }
    }
}
