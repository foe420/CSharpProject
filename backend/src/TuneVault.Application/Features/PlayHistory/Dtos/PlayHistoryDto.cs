namespace TuneVault.Application.Features.PlayHistory.Dtos;

public class PlayHistoryDto
{
    public Guid MediaItemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string FileType { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public DateTime PlayedAt { get; set; }
}
