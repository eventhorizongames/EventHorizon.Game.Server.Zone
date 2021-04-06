namespace EventHorizon.Server.Core.Tests.Timer
{
    using EventHorizon.Server.Core.Events.Check;
    using EventHorizon.Server.Core.Timer;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using FluentAssertions;
    using Xunit;

    public class CheckCoreServerConnectionTimerTaskTests
    {
        [Fact]
        public void ShouldHaveExpectedValuesWhenCreated()
        {
            // Given
            var expectedPeriod = 60000;
            var expectedTag = "CheckCoreServerConnection";
            var expectedOnValidationEvent = new IsServerStarted();
            var expectedOnRunEvent = new CheckCoreServerConnection();

            // When
            var actual = new CheckCoreServerConnectionTimerTask();

            // Then
            actual.Period
                .Should().Be(expectedPeriod);
            actual.Tag
                .Should().Be(expectedTag);
            actual.OnValidationEvent
                .Should().Be(expectedOnValidationEvent);
            actual.OnRunEvent
                .Should().Be(expectedOnRunEvent);
            actual.LogDetails
                .Should().BeTrue();
        }
    }
}
