using MediatR;
using TuneVault.Application.Features.Profiles.Dtos;

namespace TuneVault.Application.Features.Profiles.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(
    Guid UserId,
    string DisplayName,
    string Bio,
    string? AvatarPath) : IRequest<UserProfileDto>;
