using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuneVault.Application.Features.MediaItems.Commands.CreateMediaItem;
using TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;

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
    public async Task<ActionResult<MediaItemDto>> Create([FromBody] CreateMediaItemCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetLibrary), new { id = result.Id }, result);
    }
}
