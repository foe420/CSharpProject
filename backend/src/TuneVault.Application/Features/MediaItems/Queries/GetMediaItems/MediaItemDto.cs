using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;

public class MediaItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }
    public int DurationSeconds { get; set; }
}
