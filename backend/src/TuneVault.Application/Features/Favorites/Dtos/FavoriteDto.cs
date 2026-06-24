namespace TuneVault.Application.Features.Favorites.Dtos;

public class FavoriteDto
{
    public Guid MediaItemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string FileType { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PagedFavoritesDto
{
    public List<FavoriteDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
