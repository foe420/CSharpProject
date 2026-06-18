using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TuneVault.Application.Features.Playlists.Commands.CreatePlaylist;
using TuneVault.Application.Features.Playlists.Commands.AddTrackToPlaylist;
using TuneVault.Application.Features.Playlists.Commands.DeletePlaylist;
using TuneVault.Application.Features.Playlists.Queries.GetPlaylistById;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Features.Playlists.Queries.GetUserPlaylists;

namespace TuneVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaylistsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlaylistsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<PlaylistDto>> Create(
        [FromBody] CreatePlaylistCommand command,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        command.OwnerId = Guid.Parse(userId);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id}/tracks")]
    public async Task<ActionResult<PlaylistDto>> AddTrack(
        Guid id,
        [FromBody] AddTrackToPlaylistCommand command,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        command.PlaylistId = id;
        command.OwnerId = Guid.Parse(userId);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _mediator.Send(new DeletePlaylistCommand(id, Guid.Parse(userId)), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<PlaylistDetailDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userId = string.IsNullOrEmpty(userIdStr) ? null : Guid.Parse(userIdStr);

        var result = await _mediator.Send(new GetPlaylistByIdQuery(id, userId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<ActionResult<IReadOnlyList<PlaylistDto>>> GetMyPlaylists(
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        var userId = Guid.Parse(userIdClaim);

        var query = new GetUserPlaylistsQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
