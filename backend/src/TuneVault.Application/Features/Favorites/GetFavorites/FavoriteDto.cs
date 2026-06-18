using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.Favorites.GetFavorites;

public class FavoriteDto
{
    public Guid Id { get; set; }
    public Guid MediaItemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string? Genre { get; set; }
    public MediaFileType FileType { get; set; }
    public int Duration { get; set; }
    public DateTime FavoritedAt { get; set; }
}