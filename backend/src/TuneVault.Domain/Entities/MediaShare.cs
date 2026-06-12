namespace TuneVault.Domain.Entities;

public class MediaShare
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SenderId { get; set; }
    public User? Sender { get; set; }

    public Guid ReceiverId { get; set; }
    public User? Receiver { get; set; }

    public Guid? MediaItemId { get; set; }
    public MediaItem? MediaItem { get; set; }

    public Guid? PlaylistId { get; set; }
    public Playlist? Playlist { get; set; }

    public DateTime SharedAt { get; set; } = DateTime.UtcNow;
}
