namespace EventHorizon.Zone.System.DataStorage.Tests.Timer
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System;
    using EventHorizon.Zone.System.DataStorage.Save;
    using EventHorizon.Zone.System.DataStorage.Timer;
    using FluentAssertions;
    using Xunit;

    public class SaveDataStoreTimerTaskTests
    {
        [Fact]
        public void ShouldHaveExpectedPropertiesWhenCreated()
        {
            // Given
            var expectedPeriod = 30000;
            var expectedTag = nameof(SaveDataStoreTimerTask);
            var expectedOnValidateEvent = new IsServerStarted();
            var expectedOnRunEvent = new RunSaveDataStoreEvent();

            // When
            var actual = new SaveDataStoreTimerTask();

            // Then
            actual.Period
                .Should().Be(expectedPeriod);
            actual.Tag
                .Should().Be(expectedTag);
            actual.OnValidationEvent
                .Should().Be(expectedOnValidateEvent);
            actual.OnRunEvent
                .Should().Be(expectedOnRunEvent);
            actual.LogDetails
                .Should().BeFalse();
        }

    }
}
