using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TuneVault.Application.Features.Shares.CreateShare;
using TuneVault.Application.Features.Shares.GetInboxShares;
using TuneVault.Application.Features.Shares.GetSentShares;

namespace TuneVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SharesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SharesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/shares
    [HttpPost]
    public async Task<ActionResult<ShareResponseDto>> CreateShare(
        [FromBody] CreateShareCommand command,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        var userId = Guid.Parse(userIdClaim);
        command.SenderId = userId;

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    // GET /api/shares/inbox
    [HttpGet("inbox")]
    public async Task<ActionResult<IReadOnlyList<ShareDto>>> GetInbox(
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        var userId = Guid.Parse(userIdClaim);
        var query = new GetInboxSharesQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // GET /api/shares/sent
    [HttpGet("sent")]
    public async Task<ActionResult<IReadOnlyList<ShareDto>>> GetSent(
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        var userId = Guid.Parse(userIdClaim);
        var query = new GetSentSharesQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}