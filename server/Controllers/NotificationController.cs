using Application.Common.ViewModels;
using Application.Notifications.Commands.DeleteNotification;
using Application.Notifications.Commands.MarkAllAsRead;
using Application.Notifications.Commands.UpdateReadStatus;
using Application.Notifications.Queries.GetNotificationOfUser;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

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

    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        MarkAllAsReadCommand markAllAsReadCommand =
            new() { UserId = HttpContext.Items["User"]!.ToString() };
        await Mediator.Send(markAllAsReadCommand);
        return NoContent();
    }

    [HttpPatch("{notificationId}")]
    public async Task<IActionResult> UpdateReadStatus(string notificationId)
    {
        UpdateReadStatusCommand updateReadStatusCommand = new() { NotificationId = notificationId };
        await Mediator.Send(updateReadStatusCommand);
        return NoContent();
    }

    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> DeleteNotification(string notificationId)
    {
        DeleteNotificationCommand deleteNotificationCommand =
            new() { NotificationId = notificationId };
        await Mediator.Send(deleteNotificationCommand);
        return NoContent();
    }
}
