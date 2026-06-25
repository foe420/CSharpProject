using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Playlists.Queries.GetUserPlaylists;

public class GetUserPlaylistsQueryHandler : IRequestHandler<GetUserPlaylistsQuery, List<PlaylistDto>>
{
    private readonly IPlaylistRepository _playlistRepository;

    public GetUserPlaylistsQueryHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<List<PlaylistDto>> Handle(GetUserPlaylistsQuery request, CancellationToken cancellationToken)
    {
        var playlists = await _playlistRepository.GetByOwnerIdAsync(request.OwnerId, cancellationToken);
        return playlists.Select(p => new PlaylistDto
        {
            Id = p.Id,
            Title = p.Title,
            IsPublic = p.IsPublic,
            TrackCount = p.Tracks.Count
        }).ToList();
    }
}
