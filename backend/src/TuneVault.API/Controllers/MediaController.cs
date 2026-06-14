using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuneVault.Application.Features.MediaItems.Commands.CreateMediaItem;
using TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;
using TuneVault.Application.Features.MediaItems.Commands.UploadMedia;
using TuneVault.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TuneVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _db;

    public MediaController(IMediator mediator, AppDbContext db)
    {
        _mediator = mediator;
        _db = db;
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
    public async Task<IActionResult> Stream(Guid id)
    {
        var media =
            await _db.MediaItems
                .FirstOrDefaultAsync(
                    x => x.Id == id);

        if (media == null)
            return NotFound();

        var stream =
            new FileStream(
                media.FilePath,
                FileMode.Open,
                FileAccess.Read);

        var extension =
            Path.GetExtension(media.FilePath)
                .ToLowerInvariant();

        string contentType =
            extension switch
            {
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
                _ => "application/octet-stream"
            };

        return File(
            stream,
            contentType,
            enableRangeProcessing: true);
    }
}
