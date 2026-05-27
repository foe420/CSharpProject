namespace TuneVault.Domain.Entities;

public class Playlist
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }

    public ICollection<PlaylistMedia> PlaylistMediaItems { get; set; } = new List<PlaylistMedia>();
}
