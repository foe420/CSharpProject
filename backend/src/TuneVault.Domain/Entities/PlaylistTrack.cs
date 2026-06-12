namespace TuneVault.Domain.Entities;

public class PlaylistTrack
{
    public Guid PlaylistId { get; set; }
    public Playlist? Playlist { get; set; }

    public Guid MediaItemId { get; set; }
    public MediaItem? MediaItem { get; set; }

    public int Position { get; set; }
}
