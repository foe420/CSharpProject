using MediatR;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.Favorites.Commands.ToggleFavorite;

public class ToggleFavoriteCommandHandler : IRequestHandler<ToggleFavoriteCommand, ToggleFavoriteResult>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public ToggleFavoriteCommandHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<ToggleFavoriteResult> Handle(ToggleFavoriteCommand request, CancellationToken cancellationToken)
    {
        var existing = await _favoriteRepository.GetAsync(request.UserId, request.MediaItemId, cancellationToken);

        if (existing != null)
        {
            await _favoriteRepository.DeleteAsync(existing, cancellationToken);
            return new ToggleFavoriteResult(IsFavorited: false);
        }

        var favorite = new Favorite
        {
            UserId = request.UserId,
            MediaItemId = request.MediaItemId,
            CreatedAt = DateTime.UtcNow
        };

        await _favoriteRepository.AddAsync(favorite, cancellationToken);
        return new ToggleFavoriteResult(IsFavorited: true);
    }
}
