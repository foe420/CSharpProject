using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuneVault.Application.Features.Notifications.Commands.MarkAllRead;
using TuneVault.Application.Features.Notifications.Commands.MarkRead;
using TuneVault.Application.Features.Notifications.Dtos;
using TuneVault.Application.Features.Notifications.Queries.GetNotifications;

namespace TuneVault.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<NotificationsPageDto>> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
        {
            return Unauthorized();
        }

        var query = new GetNotificationsQuery(Guid.Parse(userIdStr), page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
        {
            return Unauthorized();
        }

        var command = new MarkNotificationReadCommand(Guid.Parse(userIdStr), id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
        {
            return Unauthorized();
        }

        var command = new MarkAllReadCommand(Guid.Parse(userIdStr));
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
