namespace TuneVault.Domain.Entities;

public class PlayHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid MediaItemId { get; set; }
    public MediaItem? MediaItem { get; set; }

    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
}
