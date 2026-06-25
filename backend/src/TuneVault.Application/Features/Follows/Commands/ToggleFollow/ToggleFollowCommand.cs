using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TuneVault.Application.Common.Exceptions;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Events;

namespace TuneVault.Application.Features.Follows.Commands.ToggleFollow;

public record ToggleFollowCommand(Guid FollowerId, Guid FolloweeId) : IRequest<ToggleFollowResponseDto>;

public record ToggleFollowResponseDto(bool IsFollowing);

public class ToggleFollowCommandHandler : IRequestHandler<ToggleFollowCommand, ToggleFollowResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFollowRepository _followRepository;
    private readonly IPublisher _publisher;

    public ToggleFollowCommandHandler(
        UserManager<ApplicationUser> userManager,
        IFollowRepository followRepository,
        IPublisher publisher)
    {
        _userManager = userManager;
        _followRepository = followRepository;
        _publisher = publisher;
    }

    public async Task<ToggleFollowResponseDto> Handle(ToggleFollowCommand request, CancellationToken cancellationToken)
    {
        if (request.FollowerId == request.FolloweeId)
        {
            throw new InvalidOperationException("You cannot follow yourself.");
        }

        // Verify if followee exists
        var followee = await _userManager.FindByIdAsync(request.FolloweeId.ToString());
        if (followee == null)
        {
            throw new NotFoundException($"User with ID {request.FolloweeId} was not found.");
        }

        var existingFollow = await _followRepository.GetAsync(request.FollowerId, request.FolloweeId, cancellationToken);
        if (existingFollow != null)
        {
            await _followRepository.DeleteAsync(existingFollow, cancellationToken);
            return new ToggleFollowResponseDto(false);
        }

        // Create new follow relationship
        var follow = new Follow
        {
            FollowerId = request.FollowerId,
            FolloweeId = request.FolloweeId,
            FollowedAt = DateTime.UtcNow
        };

        await _followRepository.AddAsync(follow, cancellationToken);

        // Get follower name
        var follower = await _userManager.FindByIdAsync(request.FollowerId.ToString());
        var followerName = follower != null
            ? (!string.IsNullOrWhiteSpace(follower.DisplayName) ? follower.DisplayName : follower.Email)
            : "Someone";

        // Publish event
        var followEvent = new UserFollowedEvent(request.FollowerId, request.FolloweeId, followerName ?? "Someone");
        await _publisher.Publish(followEvent, cancellationToken);

        return new ToggleFollowResponseDto(true);
    }
}
