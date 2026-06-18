using MediatR;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.Favorites.ToggleFavorite;

public class ToggleFavoriteCommandHandler : IRequestHandler<ToggleFavoriteCommand, ToggleFavoriteResponseDto>
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IMediaRepository _mediaRepository;

    public ToggleFavoriteCommandHandler(
        IFavoriteRepository favoriteRepository,
        IMediaRepository mediaRepository)
    {
        _favoriteRepository = favoriteRepository;
        _mediaRepository = mediaRepository;
    }

    public async Task<ToggleFavoriteResponseDto> Handle(ToggleFavoriteCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra media item tồn tại
        var media = await _mediaRepository.GetByIdAsync(request.MediaItemId, cancellationToken);
        if (media == null)
        {
            throw new KeyNotFoundException($"Media item with ID {request.MediaItemId} not found");
        }

        // Kiểm tra đã favorite chưa
        var existing = await _favoriteRepository.GetByUserAndMediaAsync(
            request.UserId,
            request.MediaItemId,
            cancellationToken);

        if (existing != null)
        {
            await _favoriteRepository.DeleteAsync(existing, cancellationToken);
            return new ToggleFavoriteResponseDto
            {
                IsFavorited = false,
                MediaItemId = request.MediaItemId,
                Title = media.Title,
                Artist = media.Artist
            };
        }

        var favorite = new Favorite
        {
            UserId = request.UserId,
            MediaItemId = request.MediaItemId,
            CreatedAt = DateTime.UtcNow
        };

        await _favoriteRepository.AddAsync(favorite, cancellationToken);

        return new ToggleFavoriteResponseDto
        {
            IsFavorited = true,
            MediaItemId = request.MediaItemId,
            Title = media.Title,
            Artist = media.Artist
        };
    }
}