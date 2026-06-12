namespace TuneVault.Application.Auth.Register;

public sealed record RegisterResponseDto(Guid UserId, string Email, string Token);