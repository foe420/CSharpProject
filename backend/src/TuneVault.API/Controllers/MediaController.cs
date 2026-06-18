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
using TuneVault.Application.Features.Favorites.ToggleFavorite;
using TuneVault.Application.Features.Favorites.GetFavorites;
using TuneVault.Application.Features.PlayHistory.RecordPlayHistory;
using TuneVault.Application.Features.PlayHistory.GetRecentHistory;

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

    // ========== FAVORITES ==========

    [HttpPost("{id}/favorite")]
    [Authorize]
    public async Task<ActionResult<ToggleFavoriteResponseDto>> ToggleFavorite(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        var userId = Guid.Parse(userIdClaim);
        
        var command = new ToggleFavoriteCommand
        {
            MediaItemId = id,  
            UserId = userId
        };
        
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("users/me/favorites")]
    [Authorize]
    public async Task<ActionResult<PagedFavoriteResult>> GetFavorites(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        var userId = Guid.Parse(userIdClaim);
        
        var query = new GetFavoritesQuery
        {
            UserId = userId,
            Page = page,
            PageSize = pageSize
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // ========== PLAY HISTORY ==========

    [HttpPost("{id}/play")]
    [Authorize]
    public async Task<IActionResult> RecordPlay(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        var userId = Guid.Parse(userIdClaim);
        
        var command = new RecordPlayHistoryCommand
        {
            MediaItemId = id,
            UserId = userId
        };
        
        await _mediator.Send(command, cancellationToken);
        return Ok(new { message = "Play recorded successfully" });
    }

    [HttpGet("users/me/history")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<HistoryItemDto>>> GetRecentHistory(
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        var userId = Guid.Parse(userIdClaim);
        
        var query = new GetRecentHistoryQuery
        {
            UserId = userId
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
