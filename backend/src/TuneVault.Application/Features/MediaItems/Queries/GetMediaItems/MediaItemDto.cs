using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;

public class MediaItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string? Genre { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public MediaFileType FileType { get; set; }
    public int Duration { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
