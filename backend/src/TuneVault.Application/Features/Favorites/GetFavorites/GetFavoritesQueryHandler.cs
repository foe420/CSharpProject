using MediatR;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.Favorites.GetFavorites;

public class GetFavoritesQueryHandler : IRequestHandler<GetFavoritesQuery, PagedFavoriteResult>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public GetFavoritesQueryHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<PagedFavoriteResult> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
    {
        var favorites = await _favoriteRepository.GetFavoritesByUserAsync(
            request.UserId,
            request.Page,
            request.PageSize,
            cancellationToken);

        var totalCount = await _favoriteRepository.GetTotalFavoritesByUserAsync(
            request.UserId,
            cancellationToken);

        return new PagedFavoriteResult
        {
            Items = favorites.Select(f => new FavoriteDto
            {
                MediaItemId = f.MediaItemId,
                Title = f.MediaItem?.Title ?? string.Empty,
                Artist = f.MediaItem?.Artist ?? string.Empty,
                Genre = f.MediaItem?.Genre,
                FileType = f.MediaItem?.FileType ?? MediaFileType.Audio,
                Duration = f.MediaItem?.Duration ?? 0,
                FavoritedAt = f.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}