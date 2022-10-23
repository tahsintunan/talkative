using Application.Common.ViewModels;
using Application.Notifications.Queries.GetNotificationOfUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class NotificationController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<NotificationVm>>> GetNotificationOfUser(
            [FromQuery] GetNotificationOfUserQuery getNotificationOfUserQuery
        )
        {
            getNotificationOfUserQuery.UserId = HttpContext.Items["User"]!.ToString();
            return Ok(await Mediator.Send(getNotificationOfUserQuery));
        }
    }
}
