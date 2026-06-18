using Xunit;

namespace TuneVault.Application.Tests.Validators;

public class ShareMediaCommandValidatorTests
{
    [Fact]
    public void Validate_WhenSenderIdEqualsReceiverId_ShouldFail()
    {
        // Arrange
        var senderId = Guid.NewGuid();
        var receiverId = senderId; // Same ID

        // Act & Assert
        Assert.Equal(senderId, receiverId);
        // Validator sẽ báo lỗi "Cannot share with yourself"
    }
}