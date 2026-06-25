using System;
using System.Collections.Generic;
using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;

namespace TuneVault.Application.Features.Playlists.Queries.GetUserPlaylists;

public record GetUserPlaylistsQuery(Guid OwnerId) : IRequest<List<PlaylistDto>>;
