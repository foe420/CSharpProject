using MediatR;

namespace TuneVault.Application.Features.Favorites.ToggleFavorite;

public class ToggleFavoriteCommand : IRequest<ToggleFavoriteResponseDto>
{
    public Guid MediaItemId { get; init; }
    public Guid UserId { get; set; }
}

public class ToggleFavoriteResponseDto
{
    public bool IsFavorited { get; set; }
    public Guid MediaItemId { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
}