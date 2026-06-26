using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuneVault.Application.Features.MediaItems.Commands.CreateMediaItem;
using TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;
using TuneVault.Application.Features.MediaItems.Commands.UploadMedia;
using TuneVault.Application.Features.MediaItems.Queries.SearchMedia;
using TuneVault.Application.Features.MediaItems.Queries.GetTrendingMedia;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Domain.Enums;
using TuneVault.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using TuneVault.Application.Features.MediaItems.Queries.GetMediaStream;

namespace TuneVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MediaItemDto>>> GetLibrary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMediaItemsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult <MediaItemDto>> Create([FromBody] CreateMediaItemCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetLibrary), new { id = result.Id }, result);
    }
    [HttpPost("upload")]
    [Authorize]
    public async Task<IActionResult> Upload(
        [FromForm] UploadMediaCommand command,
        CancellationToken cancellationToken)
    {
        var userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);
        Console.WriteLine($"=== UserId from token: {userId} ===");

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        command.OwnerId =
            Guid.Parse(userId);

        var id =
            await _mediator.Send(
                command,
                cancellationToken);

        return Ok(id);
    }

    [HttpGet("{id}/stream")]
    [AllowAnonymous]
    public async Task<IActionResult> Stream(Guid id, CancellationToken cancellationToken)
    {
        var streamDto = await _mediator.Send(new GetMediaStreamQuery(id), cancellationToken);

        var stream =
            new FileStream(
                streamDto.FilePath,
                FileMode.Open,
                FileAccess.Read);

        return File(
            stream,
            streamDto.ContentType,
            enableRangeProcessing: true);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<MediaItemSummaryDto>>> Search(
        [FromQuery] string? term,
        [FromQuery] MediaFileType? fileType,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new SearchMediaQuery(term, fileType, page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("trending")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MediaItemSummaryDto>>> GetTrending(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTrendingMediaQuery(), cancellationToken);
        return Ok(result);
    }
}
