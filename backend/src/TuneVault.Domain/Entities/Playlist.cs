namespace TuneVault.Domain.Entities;

public class Playlist
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }

    public ICollection<PlaylistTrack> Tracks { get; set; } = new List<PlaylistTrack>();
    public ICollection<MediaShare> MediaShares { get; set; } = new List<MediaShare>();
}
