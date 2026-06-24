using MediatR;
using TuneVault.Application.Features.Favorites.Dtos;

namespace TuneVault.Application.Features.Favorites.Queries.GetFavorites;

public record GetFavoritesQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<PagedFavoritesDto>;
