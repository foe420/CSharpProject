using MediatR;
using TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;
using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.MediaItems.Commands.CreateMediaItem;

public class CreateMediaItemCommand : IRequest<MediaItemDto>
{
    public string Title { get; init; } = string.Empty;
    public string Artist { get; init; } = string.Empty;
    public string? Genre { get; init; }
    public string FilePath { get; init; } = string.Empty;
    public MediaFileType FileType { get; init; }
    public int Duration { get; init; }
    public string? Description { get; init; }
}
