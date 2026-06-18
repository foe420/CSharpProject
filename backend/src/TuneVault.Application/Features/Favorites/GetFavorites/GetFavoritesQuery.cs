using MediatR;

namespace TuneVault.Application.Features.Favorites.GetFavorites;

public class GetFavoritesQuery : IRequest<PagedFavoriteResult>
{
    public Guid UserId { get; set; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}

public class PagedFavoriteResult
{
    public IReadOnlyList<FavoriteDto> Items { get; set; } = new List<FavoriteDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}