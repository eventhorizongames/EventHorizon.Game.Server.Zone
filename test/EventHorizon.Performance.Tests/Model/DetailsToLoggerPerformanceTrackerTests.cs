namespace EventHorizon.Performance.Tests.Model;

using EventHorizon.Performance.Model;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class DetailsToLoggerPerformanceTrackerTests
{
    [Fact]
    public void ShouldCauseExceptionWhenCreatedAndDisposed()
    {
        // Given
        var loggerMock = new Mock<ILogger>();


        // When
        var actual = new DetailsToLoggerPerformanceTracker(
            loggerMock.Object
        );
        actual.Dispose();

        // Then
        actual.Should().NotBeNull();
    }
}
