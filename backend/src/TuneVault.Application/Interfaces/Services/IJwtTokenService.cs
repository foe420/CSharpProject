using TuneVault.Application.Common.Models;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Services;

public interface IJwtTokenService
{
    JwtTokenResult GenerateToken(ApplicationUser user);
}