using MediatR;
using TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;
using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.MediaItems.Commands.CreateMediaItem;

public class CreateMediaItemCommand : IRequest<MediaItemDto>
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string MediaUrl { get; init; } = string.Empty;
    public string ThumbnailUrl { get; init; } = string.Empty;
    public int DurationSeconds { get; init; }
    public MediaType MediaType { get; init; }
}
