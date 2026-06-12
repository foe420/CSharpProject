using MediatR;
using Microsoft.AspNetCore.Identity;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Application.Interfaces.Services;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Auth.Register;

public sealed class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterHandler(
        UserManager<ApplicationUser> userManager,
        IUserProfileRepository userProfileRepository,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _userProfileRepository = userProfileRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var displayName = request.DisplayName.Trim();

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName,
            Role = "User"
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errorMessage = string.Join("; ", createResult.Errors.Select(error => error.Description));
            throw new InvalidOperationException(errorMessage);
        }

        await _userProfileRepository.AddAsync(new UserProfile
        {
            UserId = user.Id,
            DisplayName = displayName,
            Bio = string.Empty
        }, cancellationToken);

        var token = _jwtTokenService.GenerateToken(user);

        return new RegisterResponseDto(user.Id, user.Email, token.Token);
    }
}