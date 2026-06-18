using System.Security.Claims;
using Microsoft.Extensions.Options;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Auth;
using Xunit;

namespace TuneVault.Application.Tests.Services;

public class JwtTokenServiceTests
{
    [Fact]
    public void GenerateToken_ShouldContainCorrectUserIdClaim()
    {
        // Arrange
        var settings = new JwtSettings
        {
            Key = "ThisIsMySuperSecretKeyForJwtTokenThatIsAtLeast64CharactersLong123456",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };

        var options = Options.Create(settings);
        var jwtService = new JwtTokenService(options);

        var userId = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = userId,
            Email = "test@example.com",
            Role = "User"
        };

        // Act
        var result = jwtService.GenerateToken(user);
        var token = result.Token;

        // Assert
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        Assert.NotNull(userIdClaim);
        Assert.Equal(user.Id.ToString(), userIdClaim.Value);
    }
}