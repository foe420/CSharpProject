namespace TuneVault.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public UserProfile? Profile { get; set; }
    public ICollection<MediaItem> OwnedMediaItems { get; set; } = new List<MediaItem>();
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<PlayHistory> PlayHistoryEntries { get; set; } = new List<PlayHistory>();
    public ICollection<MediaShare> SentShares { get; set; } = new List<MediaShare>();
    public ICollection<MediaShare> ReceivedShares { get; set; } = new List<MediaShare>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Follow> Following { get; set; } = new List<Follow>();
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
}
