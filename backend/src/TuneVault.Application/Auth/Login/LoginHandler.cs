using MediatR;
using Microsoft.AspNetCore.Identity;
using TuneVault.Application.Interfaces.Services;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Auth.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _jwtTokenService.GenerateToken(user);

        return new LoginResponseDto(user.Id, user.Email, token.Token, token.ExpiresAt);
    }
}