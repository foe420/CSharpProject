using MediatR;

namespace TuneVault.Application.Auth.Login;

public sealed class LoginCommand : IRequest<LoginResponseDto>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}