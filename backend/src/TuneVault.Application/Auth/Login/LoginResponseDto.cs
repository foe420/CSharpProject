namespace TuneVault.Application.Auth.Login;

public sealed record LoginResponseDto(Guid UserId, string Email, string Token, DateTime ExpiresAt);