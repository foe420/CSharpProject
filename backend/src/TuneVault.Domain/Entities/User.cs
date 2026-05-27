namespace TuneVault.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<MediaItem> UploadedMediaItems { get; set; } = new List<MediaItem>();
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
