using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TuneVault.Application.Features.Profiles.Dtos;
using TuneVault.Application.Features.Profiles.Queries.GetUserProfile;
using TuneVault.Application.Features.Profiles.Commands.UpdateUserProfile;

namespace TuneVault.API.Controllers;

[ApiController]
[Route("api/users/me/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfileDto>> GetProfile(CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdStr);
        var profile = await _mediator.Send(new GetUserProfileQuery(userId), cancellationToken);
        return Ok(profile);
    }

    [HttpPut]
    public async Task<ActionResult<UserProfileDto>> UpdateProfile(
        [FromBody] UpdateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdStr);
        
        var existingProfile = await _mediator.Send(new GetUserProfileQuery(userId), cancellationToken);
        var displayName = existingProfile?.DisplayName ?? "User";

        var command = new UpdateUserProfileCommand(
            userId,
            displayName,
            request.Bio,
            request.AvatarPath);

        var updatedProfile = await _mediator.Send(command, cancellationToken);
        return Ok(updatedProfile);
    }
}

public record UpdateProfileRequest(string Bio, string? AvatarPath);
