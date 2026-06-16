using MediatR;
using TuneVault.Application.Common.Exceptions;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Playlists.Queries.GetPlaylistById;

public class GetPlaylistByIdQueryHandler : IRequestHandler<GetPlaylistByIdQuery, PlaylistDetailDto>
{
    private readonly IPlaylistRepository _playlistRepository;

    public GetPlaylistByIdQueryHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<PlaylistDetailDto> Handle(GetPlaylistByIdQuery request, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(request.Id, cancellationToken);
        if (playlist == null)
        {
            throw new KeyNotFoundException("Playlist not found.");
        }

        if (!playlist.IsPublic && playlist.OwnerId != request.CurrentUserId)
        {
            throw new ForbiddenException("This playlist is private.");
        }

        var ownerName = playlist.Owner != null
            ? (playlist.Owner.Profile?.DisplayName ?? playlist.Owner.UserName)
            : "Unknown Owner";

        var tracks = playlist.Tracks
            .OrderBy(t => t.Position)
            .Select(t => new MediaItemSummaryDto
            {
                Id = t.MediaItemId,
                Title = t.MediaItem?.Title ?? "Unknown Title",
                Artist = t.MediaItem?.Artist ?? "Unknown Artist",
                Duration = t.MediaItem?.Duration ?? 0,
                FileType = t.MediaItem?.FileType.ToString() ?? "Unknown",
                StreamUrl = $"/api/media/{t.MediaItemId}/stream"
            })
            .ToList();

        return new PlaylistDetailDto
        {
            Id = playlist.Id,
            Title = playlist.Title,
            IsPublic = playlist.IsPublic,
            OwnerName = ownerName,
            Tracks = tracks
        };
    }
}
