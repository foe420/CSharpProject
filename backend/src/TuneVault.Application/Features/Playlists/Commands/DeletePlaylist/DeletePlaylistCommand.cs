using MediatR;

namespace TuneVault.Application.Features.Playlists.Commands.DeletePlaylist;

public record DeletePlaylistCommand(Guid PlaylistId, Guid OwnerId) : IRequest;
