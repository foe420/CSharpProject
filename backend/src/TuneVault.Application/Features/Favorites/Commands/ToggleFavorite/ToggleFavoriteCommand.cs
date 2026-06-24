using MediatR;

namespace TuneVault.Application.Features.Favorites.Commands.ToggleFavorite;

public record ToggleFavoriteCommand(Guid UserId, Guid MediaItemId) : IRequest<ToggleFavoriteResult>;

public record ToggleFavoriteResult(bool IsFavorited);
