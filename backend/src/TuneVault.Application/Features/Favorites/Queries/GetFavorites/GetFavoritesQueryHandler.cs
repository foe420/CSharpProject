using MediatR;
using TuneVault.Application.Features.Favorites.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Favorites.Queries.GetFavorites;

public class GetFavoritesQueryHandler : IRequestHandler<GetFavoritesQuery, PagedFavoritesDto>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public GetFavoritesQueryHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<PagedFavoritesDto> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _favoriteRepository.GetByUserIdAsync(
            request.UserId, request.Page, request.PageSize, cancellationToken);

        var dtos = items.Select(f => new FavoriteDto
        {
            MediaItemId = f.MediaItemId,
            Title = f.MediaItem?.Title ?? "Unknown Title",
            Artist = f.MediaItem?.Artist ?? "Unknown Artist",
            Duration = f.MediaItem?.Duration ?? 0,
            FileType = f.MediaItem?.FileType.ToString() ?? "Unknown",
            StreamUrl = $"/api/media/{f.MediaItemId}/stream",
            CreatedAt = f.CreatedAt
        }).ToList();

        return new PagedFavoritesDto
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
