using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TuneVault.Application.Features.Favorites.Commands.ToggleFavorite;
using TuneVault.Application.Features.Favorites.Queries.GetFavorites;

namespace TuneVault.API.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/media/{id}/favorite
    [HttpPost("media/{id:guid}/favorite")]
    public async Task<IActionResult> ToggleFavorite(Guid id, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new ToggleFavoriteCommand(userId, id), cancellationToken);
        return Ok(new { isFavorited = result.IsFavorited });
    }

    // GET /api/users/me/favorites
    [HttpGet("users/me/favorites")]
    public async Task<IActionResult> GetFavorites([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetFavoritesQuery(userId, page, pageSize), cancellationToken);
        return Ok(result);
    }
}
