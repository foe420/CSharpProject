using MediatR;
using TuneVault.Application.Common.Exceptions;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.Playlists.Commands.AddTrackToPlaylist;

public class AddTrackToPlaylistCommandHandler : IRequestHandler<AddTrackToPlaylistCommand, PlaylistDto>
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IMediaRepository _mediaRepository;

    public AddTrackToPlaylistCommandHandler(
        IPlaylistRepository playlistRepository,
        IMediaRepository mediaRepository)
    {
        _playlistRepository = playlistRepository;
        _mediaRepository = mediaRepository;
    }

    public async Task<PlaylistDto> Handle(AddTrackToPlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(request.PlaylistId, cancellationToken);
        if (playlist == null)
        {
            throw new KeyNotFoundException("Playlist not found.");
        }

        if (playlist.OwnerId != request.OwnerId)
        {
            throw new ForbiddenException("You do not have permission to add tracks to this playlist.");
        }

        var mediaItem = await _mediaRepository.GetByIdAsync(request.MediaItemId, cancellationToken);
        if (mediaItem == null)
        {
            throw new KeyNotFoundException("Media item not found.");
        }

        var trackExists = playlist.Tracks.Any(t => t.MediaItemId == request.MediaItemId);
        if (trackExists)
        {
            throw new InvalidOperationException("Track is already in the playlist.");
        }

        var maxPosition = playlist.Tracks.Any() ? playlist.Tracks.Max(t => t.Position) : 0;

        var playlistTrack = new PlaylistTrack
        {
            PlaylistId = request.PlaylistId,
            MediaItemId = request.MediaItemId,
            Position = maxPosition + 1
        };

        playlist.Tracks.Add(playlistTrack);
        await _playlistRepository.UpdateAsync(playlist, cancellationToken);

        return new PlaylistDto
        {
            Id = playlist.Id,
            Title = playlist.Title,
            IsPublic = playlist.IsPublic,
            TrackCount = playlist.Tracks.Count
        };
    }
}
