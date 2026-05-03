using Xunit;
using FileAccessSystem.Services;

public class FileAccessServiceTests
{
    [Fact]
    public void GetRiskLevel_ReturnsHigh_WhenScoreAbove70()
    {
        var service = new FileAccessService();

        var result = service.GetRiskLevel(80);

        Assert.Equal("High", result);
    }

    [Fact]
    public void GetRiskLevel_ReturnsLow_WhenScoreLow()
    {
        var service = new FileAccessService();

        var result = service.GetRiskLevel(10);

        Assert.Equal("Low", result);
    }
}