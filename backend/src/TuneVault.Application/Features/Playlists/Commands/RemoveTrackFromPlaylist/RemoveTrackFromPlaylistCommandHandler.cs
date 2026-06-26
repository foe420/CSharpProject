using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuneVault.Application.Common.Exceptions;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Playlists.Commands.RemoveTrackFromPlaylist;

public class RemoveTrackFromPlaylistCommandHandler : IRequestHandler<RemoveTrackFromPlaylistCommand, Unit>
{
    private readonly IPlaylistRepository _playlistRepository;

    public RemoveTrackFromPlaylistCommandHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Unit> Handle(RemoveTrackFromPlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(request.PlaylistId, cancellationToken);
        if (playlist == null)
        {
            throw new KeyNotFoundException("Playlist not found.");
        }

        if (playlist.OwnerId != request.UserId)
        {
            throw new ForbiddenException("You do not have permission to remove tracks from this playlist.");
        }

        var track = playlist.Tracks.FirstOrDefault(t => t.MediaItemId == request.MediaItemId);
        if (track == null)
        {
            throw new KeyNotFoundException("Track not found in this playlist.");
        }

        playlist.Tracks.Remove(track);
        await _playlistRepository.UpdateAsync(playlist, cancellationToken);

        return Unit.Value;
    }
}
