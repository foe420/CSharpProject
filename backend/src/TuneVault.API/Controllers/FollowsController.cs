using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuneVault.Application.Features.Follows.Commands.ToggleFollow;

namespace TuneVault.API.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class FollowsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FollowsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("users/{id:guid}/follow")]
    public async Task<ActionResult<ToggleFollowResponseDto>> ToggleFollow(Guid id, CancellationToken cancellationToken)
    {
        var followerIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(followerIdStr))
        {
            return Unauthorized();
        }

        var command = new ToggleFollowCommand(Guid.Parse(followerIdStr), id);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
