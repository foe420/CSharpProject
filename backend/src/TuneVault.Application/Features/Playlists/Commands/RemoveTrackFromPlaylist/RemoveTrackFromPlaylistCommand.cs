using MediatR;
using System;

namespace TuneVault.Application.Features.Playlists.Commands.RemoveTrackFromPlaylist;

public record RemoveTrackFromPlaylistCommand(Guid PlaylistId, Guid MediaItemId, Guid UserId) : IRequest<Unit>;
