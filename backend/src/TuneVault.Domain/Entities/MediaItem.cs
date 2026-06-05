using TuneVault.Domain.Enums;

namespace TuneVault.Domain.Entities;

public class MediaItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string? Genre { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public MediaFileType FileType { get; set; }
    public int Duration { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<PlayHistory> PlayHistoryEntries { get; set; } = new List<PlayHistory>();
    public ICollection<MediaShare> MediaShares { get; set; } = new List<MediaShare>();
}
