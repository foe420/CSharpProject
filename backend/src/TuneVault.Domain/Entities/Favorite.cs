namespace TuneVault.Domain.Entities;

public class Favorite
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid MediaItemId { get; set; }
    public MediaItem? MediaItem { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
