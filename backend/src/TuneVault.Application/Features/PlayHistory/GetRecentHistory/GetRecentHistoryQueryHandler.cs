using MediatR;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.PlayHistory.GetRecentHistory;

public class GetRecentHistoryQueryHandler : IRequestHandler<GetRecentHistoryQuery, IReadOnlyList<HistoryItemDto>>
{
    private readonly IPlayHistoryRepository _playHistoryRepository;
    private const int LIMIT = 10;

    public GetRecentHistoryQueryHandler(IPlayHistoryRepository playHistoryRepository)
    {
        _playHistoryRepository = playHistoryRepository;
    }

    public async Task<IReadOnlyList<HistoryItemDto>> Handle(GetRecentHistoryQuery request, CancellationToken cancellationToken)
    {
        var histories = await _playHistoryRepository.GetRecentHistoryByUserAsync(
            request.UserId,
            LIMIT,
            cancellationToken);

        var distinctMedia = histories
            .GroupBy(x => x.MediaItemId)
            .Select(g => g.First())
            .ToList();

        return distinctMedia.Select(h => new HistoryItemDto
        {
            MediaItemId = h.MediaItemId,
            Title = h.MediaItem?.Title ?? string.Empty,
            Artist = h.MediaItem?.Artist ?? string.Empty,
            Genre = h.MediaItem?.Genre,
            FileType = h.MediaItem?.FileType ?? MediaFileType.Audio,
            Duration = h.MediaItem?.Duration ?? 0,
            PlayedAt = h.PlayedAt
        }).ToList();
    }
}