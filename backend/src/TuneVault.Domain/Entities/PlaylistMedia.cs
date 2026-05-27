namespace TuneVault.Domain.Entities;

public class PlaylistMedia
{
    public Guid PlaylistId { get; set; }
    public Playlist? Playlist { get; set; }

    public Guid MediaItemId { get; set; }
    public MediaItem? MediaItem { get; set; }

    public int Order { get; set; }
}
