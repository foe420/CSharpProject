using MediatR;
using TuneVault.Application.Features.PlayHistory.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.PlayHistory.Queries.GetRecentHistory;

public class GetRecentHistoryQueryHandler : IRequestHandler<GetRecentHistoryQuery, List<PlayHistoryDto>>
{
    private readonly IPlayHistoryRepository _playHistoryRepository;

    public GetRecentHistoryQueryHandler(IPlayHistoryRepository playHistoryRepository)
    {
        _playHistoryRepository = playHistoryRepository;
    }

    public async Task<List<PlayHistoryDto>> Handle(GetRecentHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _playHistoryRepository.GetRecentByUserIdAsync(request.UserId, limit: 10, cancellationToken);

        return history.Select(h => new PlayHistoryDto
        {
            MediaItemId = h.MediaItemId,
            Title = h.MediaItem?.Title ?? "Unknown Title",
            Artist = h.MediaItem?.Artist ?? "Unknown Artist",
            Duration = h.MediaItem?.Duration ?? 0,
            FileType = h.MediaItem?.FileType.ToString() ?? "Unknown",
            StreamUrl = $"/api/media/{h.MediaItemId}/stream",
            PlayedAt = h.PlayedAt
        }).ToList();
    }
}
