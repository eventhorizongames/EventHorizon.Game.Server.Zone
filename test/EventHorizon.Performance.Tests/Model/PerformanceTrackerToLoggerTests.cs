namespace EventHorizon.Performance.Tests.Model
{
    using EventHorizon.Performance.Model;

    using FluentAssertions;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class ToLoggerPerformanceTrackerFactoryTests
    {
        [Fact]
        public void ShouldReturnReturnEmptyTrackerInstanceWhenSettingsAreNotEnabled()
        {
            // Given

            var loggerFactorMock = new Mock<ILoggerFactory>();
            var settingsMock = new Mock<PerformanceSettings>();

            settingsMock.Setup(
                mock => mock.IsEnabled
            ).Returns(
                false
            );

            // When
            var factory = new ToLoggerPerformanceTrackerFactory(
                loggerFactorMock.Object,
                settingsMock.Object
            );

            var actual = factory.Build(
                "track me"
            );

            // Then
            actual.Should().BeOfType<EmptyPerformanceTracker>();
        }
        [Fact]
        public void ShouldCreateLoggerByPassedInNameWhenBuildIsCalledWithName()
        {
            // Given
            var trackerName = "track me";

            var loggerMock = new Mock<ILogger>();

            var loggerFactorMock = new Mock<ILoggerFactory>();
            var settingsMock = new Mock<PerformanceSettings>();

            loggerFactorMock.Setup(
                mock => mock.CreateLogger(
                    trackerName
                )
            ).Returns(
                loggerMock.Object
            );

            settingsMock.Setup(
                mock => mock.IsEnabled
            ).Returns(
                true
            );

            // When
            var factory = new ToLoggerPerformanceTrackerFactory(
                loggerFactorMock.Object,
                settingsMock.Object
            );

            var actual = factory.Build(
                trackerName
            );

            // Then
            actual.Should().BeOfType<DetailsToLoggerPerformanceTracker>();

            loggerFactorMock.Verify(
                mock => mock.CreateLogger(
                    trackerName
                )
            );
        }
    }
}
