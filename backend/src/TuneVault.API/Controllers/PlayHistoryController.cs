using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TuneVault.Application.Features.PlayHistory.Commands.RecordPlayHistory;
using TuneVault.Application.Features.PlayHistory.Queries.GetRecentHistory;

namespace TuneVault.API.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class PlayHistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlayHistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/media/{id}/play
    [HttpPost("media/{id:guid}/play")]
    public IActionResult RecordPlay(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Fire-and-forget: không await, không block response
        _ = _mediator.Send(new RecordPlayHistoryCommand(userId, id), CancellationToken.None);

        return Ok();
    }

    // GET /api/users/me/history
    [HttpGet("users/me/history")]
    public async Task<IActionResult> GetRecentHistory(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetRecentHistoryQuery(userId), cancellationToken);
        return Ok(result);
    }
}
