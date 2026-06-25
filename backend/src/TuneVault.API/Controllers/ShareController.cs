using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Features.Shares.Commands.ShareMedia;
using TuneVault.Application.Features.Shares.Dtos;
using TuneVault.Application.Features.Shares.Queries.GetReceivedShares;
using TuneVault.Application.Features.Shares.Queries.GetSentShares;

namespace TuneVault.API.Controllers;

[ApiController]
[Route("api/shares")]
[Authorize]
public class ShareController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShareController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ShareResponseDto>> Share(
        [FromBody] ShareMediaCommand command,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        command.SenderId = Guid.Parse(userId);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("inbox")]
    public async Task<ActionResult<PagedResult<ShareDto>>> GetInbox(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var query = new GetReceivedSharesQuery(Guid.Parse(userId), page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("sent")]
    public async Task<ActionResult<PagedResult<ShareDto>>> GetSent(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var query = new GetSentSharesQuery(Guid.Parse(userId), page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
