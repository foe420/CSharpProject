using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TuneVault.Application.Common.Exceptions;
using TuneVault.Application.Features.Shares.Commands.ShareMedia;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Common.Behaviors;

public class ShareMediaAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMediaRepository _mediaRepository;
    private readonly IPlaylistRepository _playlistRepository;

    public ShareMediaAuthorizationBehavior(
        IMediaRepository mediaRepository,
        IPlaylistRepository playlistRepository)
    {
        _mediaRepository = mediaRepository;
        _playlistRepository = playlistRepository;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is ShareMediaCommand shareCommand)
        {
            if (shareCommand.MediaItemId.HasValue)
            {
                var mediaItem = await _mediaRepository.GetByIdAsync(shareCommand.MediaItemId.Value, cancellationToken);
                if (mediaItem == null)
                {
                    throw new NotFoundException("Media item not found.");
                }
            }

            if (shareCommand.PlaylistId.HasValue)
            {
                var playlist = await _playlistRepository.GetByIdAsync(shareCommand.PlaylistId.Value, cancellationToken);
                if (playlist == null)
                {
                    throw new NotFoundException("Playlist not found.");
                }

                // If private and not owned by sender, throw UnauthorizedException
                if (!playlist.IsPublic && playlist.OwnerId != shareCommand.SenderId)
                {
                    throw new UnauthorizedException("This playlist is private and you do not own it.");
                }
            }
        }

        return await next();
    }
}
