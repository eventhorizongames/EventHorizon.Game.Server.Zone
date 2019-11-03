using EventHorizon.Zone.Core.Reporter.Save;
using EventHorizon.Zone.Core.Reporter.Timer;
using FluentAssertions;
using Xunit;

namespace EventHorizon.Zone.Core.Reporter.Tests.Timer
{
    public class SavePendingReportItemsTimerTests
    {
        [Fact]
        public void TestShouldHaveExpectedPropertiesWhenCreatedNew()
        {
            // Given
            var expectedPeriod = 15000;
            var expectedTag = "SavePendingReportItems";
            var expectedOnRunEvent = new SavePendingReportItemsEvent();

            // When
            var actual = new SavePendingReportItemsTimer();
            
            // Then
            actual.Period
                .Should()
                .Be(
                    expectedPeriod
                );
            actual.Tag
                .Should()
                .Be(
                    expectedTag
                );
            actual.OnRunEvent
                .Should()
                .Be(
                    expectedOnRunEvent
                );
        }
    }
}