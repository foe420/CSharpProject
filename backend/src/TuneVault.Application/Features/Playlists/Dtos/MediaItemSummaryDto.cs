namespace TuneVault.Application.Features.Playlists.Dtos;

public class MediaItemSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string FileType { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
}
