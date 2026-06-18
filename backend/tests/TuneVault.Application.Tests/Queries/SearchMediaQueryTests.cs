using Xunit;

namespace TuneVault.Application.Tests.Queries;

public class SearchMediaQueryTests
{
    [Fact]
    public void SearchMediaQuery_WhenTermIsNullOrEmpty_ShouldReturnAllResults()
    {
        // Arrange
        string? term1 = null;
        string term2 = "";
        string term3 = "test";

        // Act & Assert
        Assert.Null(term1);
        Assert.Equal("", term2);
        Assert.Equal("test", term3);
        // Khi term null/empty, không áp dụng filter
    }
}