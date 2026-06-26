using MediatR;
using TuneVault.Application.Features.Profiles.Dtos;

namespace TuneVault.Application.Features.Profiles.Queries.GetUserProfile;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto>;
