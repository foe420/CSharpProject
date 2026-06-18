namespace TuneVault.Application.Features.Shares.GetInboxShares;

public class ShareDto
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public DateTime SharedAt { get; set; }
    public MediaItemInfo? MediaItem { get; set; }
    public PlaylistInfo? Playlist { get; set; }
}

public class MediaItemInfo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string FileType { get; set; } = string.Empty;
}

public class PlaylistInfo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int TrackCount { get; set; }
}