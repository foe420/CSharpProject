using MediatR;
using TuneVault.Application.Features.Shares.GetInboxShares;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Shares.GetSentShares;

public class GetSentSharesQueryHandler : IRequestHandler<GetSentSharesQuery, IReadOnlyList<ShareDto>>
{
    private readonly IShareRepository _shareRepository;

    public GetSentSharesQueryHandler(IShareRepository shareRepository)
    {
        _shareRepository = shareRepository;
    }

    public async Task<IReadOnlyList<ShareDto>> Handle(GetSentSharesQuery request, CancellationToken cancellationToken)
    {
        var shares = await _shareRepository.GetSentByUserIdAsync(request.UserId, cancellationToken);

        return shares.Select(s => new ShareDto
        {
            Id = s.Id,
            SenderName = s.Sender?.UserName ?? "Unknown",
            ReceiverName = s.Receiver?.UserName ?? "Unknown",
            SharedAt = s.SharedAt,
            MediaItem = s.MediaItem != null ? new MediaItemInfo
            {
                Id = s.MediaItem.Id,
                Title = s.MediaItem.Title,
                Artist = s.MediaItem.Artist,
                Duration = s.MediaItem.Duration,
                FileType = s.MediaItem.FileType.ToString()
            } : null,
            Playlist = s.Playlist != null ? new PlaylistInfo
            {
                Id = s.Playlist.Id,
                Title = s.Playlist.Title,
                TrackCount = s.Playlist.Tracks?.Count ?? 0
            } : null
        }).ToList();
    }
}