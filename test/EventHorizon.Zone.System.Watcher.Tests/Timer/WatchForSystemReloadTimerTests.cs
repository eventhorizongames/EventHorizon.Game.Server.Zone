using EventHorizon.Zone.System.Watcher.Check;
using EventHorizon.Zone.System.Watcher.Timer;
using Xunit;

namespace EventHorizon.Zone.System.Watcher.Tests.Timer
{
    public class WatchForSystemReloadTimerTests
    {
        [Fact]
        public void TestShouldHaveExpectedPeriodWhenCreated()
        {
            // Given
            var expected = 5000;
            
            // When
            var actual = new WatchForSystemReloadTimer();

            // Then
            Assert.Equal(
                expected,
                actual.Period
            );
        }

        [Fact]
        public void TestShouldHaveExpectedTagWhenCreated()
        {
            // Given
            var expected = "WatchForSystemReload";
            
            // When
            var actual = new WatchForSystemReloadTimer();

            // Then
            Assert.Equal(
                expected,
                actual.Tag
            );
        }

        [Fact]
        public void TestShouldHaveExpectedOnRunEventWhenCreated()
        {
            // Given
            var expected = new CheckPendingReloadEvent();
            
            // When
            var actual = new WatchForSystemReloadTimer();

            // Then
            Assert.Equal(
                expected,
                actual.OnRunEvent
            );
        }
    }
}