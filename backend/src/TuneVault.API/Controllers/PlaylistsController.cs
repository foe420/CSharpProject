using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TuneVault.Application.Features.Playlists.Commands.CreatePlaylist;
using TuneVault.Application.Features.Playlists.Commands.AddTrackToPlaylist;
using TuneVault.Application.Features.Playlists.Commands.DeletePlaylist;
using TuneVault.Application.Features.Playlists.Commands.RemoveTrackFromPlaylist;
using TuneVault.Application.Features.Playlists.Queries.GetPlaylistById;
using TuneVault.Application.Features.Playlists.Queries.GetUserPlaylists;
using TuneVault.Application.Features.Playlists.Dtos;

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

    [HttpDelete("{id}/tracks/{trackId}")]
    public async Task<IActionResult> RemoveTrack(
        Guid id,
        Guid trackId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _mediator.Send(new RemoveTrackFromPlaylistCommand(id, trackId, Guid.Parse(userId)), cancellationToken);
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

    [HttpGet("/api/users/me/playlists")]
    public async Task<ActionResult<List<PlaylistDto>>> GetUserPlaylists(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var query = new GetUserPlaylistsQuery(Guid.Parse(userId));
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
