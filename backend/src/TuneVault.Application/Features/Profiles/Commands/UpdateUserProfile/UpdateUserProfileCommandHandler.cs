using MediatR;
using TuneVault.Application.Features.Profiles.Dtos;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.Profiles.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileDto>
{
    private readonly IUserProfileRepository _profileRepository;

    public UpdateUserProfileCommandHandler(IUserProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<UserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (profile == null)
        {
            profile = new UserProfile
            {
                UserId = request.UserId,
                DisplayName = request.DisplayName,
                Bio = request.Bio,
                AvatarPath = request.AvatarPath
            };
            await _profileRepository.AddAsync(profile, cancellationToken);
        }
        else
        {
            profile.DisplayName = request.DisplayName;
            profile.Bio = request.Bio;
            profile.AvatarPath = request.AvatarPath;
            await _profileRepository.UpdateAsync(profile, cancellationToken);
        }

        return new UserProfileDto
        {
            UserId = profile.UserId,
            DisplayName = profile.DisplayName,
            Bio = profile.Bio,
            AvatarPath = profile.AvatarPath
        };
    }
}
