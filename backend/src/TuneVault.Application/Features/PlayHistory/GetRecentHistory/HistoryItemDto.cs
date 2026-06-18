using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.PlayHistory.GetRecentHistory;

public class HistoryItemDto
{
    public Guid MediaItemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string? Genre { get; set; }
    public MediaFileType FileType { get; set; }
    public int Duration { get; set; }
    public DateTime PlayedAt { get; set; }
}