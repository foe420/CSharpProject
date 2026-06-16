using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.Playlists.Commands.CreatePlaylist;

public class CreatePlaylistCommandHandler : IRequestHandler<CreatePlaylistCommand, PlaylistDto>
{
    private readonly IPlaylistRepository _playlistRepository;

    public CreatePlaylistCommandHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<PlaylistDto> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = new Playlist
        {
            Title = request.Title.Trim(),
            IsPublic = request.IsPublic,
            OwnerId = request.OwnerId,
            CreatedAt = DateTime.UtcNow
        };

        var createdPlaylist = await _playlistRepository.AddAsync(playlist, cancellationToken);

        return new PlaylistDto
        {
            Id = createdPlaylist.Id,
            Title = createdPlaylist.Title,
            IsPublic = createdPlaylist.IsPublic,
            TrackCount = 0
        };
    }
}
