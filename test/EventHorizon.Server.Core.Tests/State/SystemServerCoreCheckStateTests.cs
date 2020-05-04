namespace EventHorizon.Server.Core.Tests.State
{
    using EventHorizon.Server.Core.State;
    using FluentAssertions;
    using Xunit;

    public class SystemServerCoreCheckStateTests
    {
        [Fact]
        public void ShouldIncrementTimesCheckedWhenCheckIsCalled()
        {
            // Given
            var expected = 1;

            // When
            var state = new SystemServerCoreCheckState();
            state.Check();
            var actual = state.TimesChecked();

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldSetTimesCheckedToZeroWhenResetIsCalled()
        {
            // Given
            var expected = 0;

            // When
            var state = new SystemServerCoreCheckState();
            state.Check();
            state.Check();
            state.Check();
            state.TimesChecked()
                .Should().NotBe(expected);
            state.Reset();
            var actual = state.TimesChecked();

            // Then
            actual.Should().Be(expected);
        }
    }
}