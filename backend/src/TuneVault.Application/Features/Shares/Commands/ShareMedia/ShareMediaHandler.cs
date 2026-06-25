using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TuneVault.Application.Common.Exceptions;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Events;

namespace TuneVault.Application.Features.Shares.Commands.ShareMedia;

public class ShareMediaHandler : IRequestHandler<ShareMediaCommand, ShareResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediaShareRepository _mediaShareRepository;
    private readonly IPublisher _publisher;

    public ShareMediaHandler(
        UserManager<ApplicationUser> userManager,
        IMediaShareRepository mediaShareRepository,
        IPublisher publisher)
    {
        _userManager = userManager;
        _mediaShareRepository = mediaShareRepository;
        _publisher = publisher;
    }

    public async Task<ShareResponseDto> Handle(ShareMediaCommand request, CancellationToken cancellationToken)
    {
        // Check if receiver exists
        var receiver = await _userManager.FindByIdAsync(request.ReceiverId.ToString());
        if (receiver == null)
        {
            throw new NotFoundException($"Receiver user with ID {request.ReceiverId} was not found.");
        }

        // Check if share already exists (same sender+receiver+mediaId/playlistId) - idempotent
        var existingShare = await _mediaShareRepository.GetExistingShareAsync(
            request.SenderId,
            request.ReceiverId,
            request.MediaItemId,
            request.PlaylistId,
            cancellationToken);

        if (existingShare != null)
        {
            return new ShareResponseDto(existingShare.Id, receiver.Email ?? string.Empty, existingShare.SharedAt);
        }

        // Create MediaShare record
        var mediaShare = new MediaShare
        {
            Id = Guid.NewGuid(),
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            MediaItemId = request.MediaItemId,
            PlaylistId = request.PlaylistId,
            SharedAt = DateTime.UtcNow
        };

        var savedShare = await _mediaShareRepository.AddAsync(mediaShare, cancellationToken);

        // Get sender name for the event
        var sender = await _userManager.FindByIdAsync(request.SenderId.ToString());
        var senderName = sender != null 
            ? (!string.IsNullOrWhiteSpace(sender.DisplayName) ? sender.DisplayName : sender.Email) 
            : "Someone";

        // Publish MediaSharedEvent via MediatR IPublisher
        var mediaSharedEvent = new MediaSharedEvent(
            request.SenderId,
            request.ReceiverId,
            request.MediaItemId,
            request.PlaylistId,
            senderName ?? "Someone"
        );

        await _publisher.Publish(mediaSharedEvent, cancellationToken);

        return new ShareResponseDto(savedShare.Id, receiver.Email ?? string.Empty, savedShare.SharedAt);
    }
}
