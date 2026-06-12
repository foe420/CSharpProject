using MediatR;

namespace TuneVault.Application.Auth.Register;

public sealed class RegisterCommand : IRequest<RegisterResponseDto>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
}