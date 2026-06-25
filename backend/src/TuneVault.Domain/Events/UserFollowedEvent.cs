using MediatR;
using System;

namespace TuneVault.Domain.Events;

public record UserFollowedEvent(
    Guid FollowerId,
    Guid FolloweeId,
    string FollowerName
) : INotification;
