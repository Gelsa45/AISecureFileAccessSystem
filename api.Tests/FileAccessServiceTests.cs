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
    [Fact]
    public void GetRiskLevel_ReturnsMedium_WhenScoreBetween40And69()
    {
        var service = new FileAccessService();

        var result = service.GetRiskLevel(50);

        Assert.Equal("Medium", result);
    }
    [Fact]
    public void GetAIReason_ReturnsHighRiskMessage_WhenRiskIsHigh()
    {
        var service = new FileAccessService();

        var result = service.GetAIReason(70, 25, "High");

        Assert.Contains("High risk", result);
    }
}