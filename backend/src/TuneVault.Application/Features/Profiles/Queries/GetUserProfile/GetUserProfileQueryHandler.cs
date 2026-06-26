using MediatR;
using TuneVault.Application.Features.Profiles.Dtos;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.Profiles.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    private readonly IUserProfileRepository _profileRepository;

    public GetUserProfileQueryHandler(IUserProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (profile == null)
        {
            profile = new UserProfile
            {
                UserId = request.UserId,
                DisplayName = "User",
                Bio = string.Empty
            };
            await _profileRepository.AddAsync(profile, cancellationToken);
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
