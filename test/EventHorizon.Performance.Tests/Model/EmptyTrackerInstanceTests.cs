namespace EventHorizon.Performance.Tests.Model
{
    using EventHorizon.Performance.Model;
    using FluentAssertions;
    using Xunit;

    public class EmptyTrackerInstanceTests
    {
        [Fact]
        public void ShouldRunDisposeWithoutIssueWhenCreated()
        {
            // Given

            // When
            var actual = new EmptyPerformanceTracker();
            actual.Dispose();

            // Then
            actual.Should().NotBeNull();
        }
    }
}
