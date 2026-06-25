using System;
using TuneVault.Application.Features.Playlists.Dtos;

namespace TuneVault.Application.Features.Shares.Dtos;

public class ShareDto
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public DateTime SharedAt { get; set; }
    public MediaItemSummaryDto? MediaItem { get; set; }
    public Guid? PlaylistId { get; set; }
    public string? PlaylistTitle { get; set; }
}
