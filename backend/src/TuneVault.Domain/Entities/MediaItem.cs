using TuneVault.Domain.Enums;

namespace TuneVault.Domain.Entities;

public class MediaItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
    public MediaType MediaType { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Guid UploadedById { get; set; }
    public User? UploadedBy { get; set; }

    public ICollection<PlaylistMedia> PlaylistMediaItems { get; set; } = new List<PlaylistMedia>();
}
