using MediatR;
using TuneVault.Application.Common.Exceptions;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Playlists.Commands.DeletePlaylist;

public class DeletePlaylistCommandHandler : IRequestHandler<DeletePlaylistCommand>
{
    private readonly IPlaylistRepository _playlistRepository;

    public DeletePlaylistCommandHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Unit> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(request.PlaylistId, cancellationToken);
        if (playlist == null)
        {
            throw new KeyNotFoundException("Playlist not found.");
        }

        if (playlist.OwnerId != request.OwnerId)
        {
            throw new ForbiddenException("You do not have permission to delete this playlist.");
        }

        await _playlistRepository.DeleteAsync(playlist, cancellationToken);

        return Unit.Value;
    }
}
