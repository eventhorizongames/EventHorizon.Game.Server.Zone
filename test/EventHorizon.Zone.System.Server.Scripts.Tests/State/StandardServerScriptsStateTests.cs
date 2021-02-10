namespace EventHorizon.Zone.System.Server.Scripts.Tests.State
{
    using EventHorizon.Zone.System.Server.Scripts.State;
    using FluentAssertions;
    using Xunit;


    public class StandardServerScriptsStateTests
    {
        [Fact]
        public void ShouldHaveEmptyCurrentHashWhenFirstCreated()
        {
            // Given
            var expected = string.Empty;

            // When
            var state = new StandardServerScriptsState();
            var actual = state.CurrentHash;

            // Then
            actual
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldSetCurrentHasWhenUpdateHashIsCalled()
        {
            // Given
            var hash = "hash";
            var expected = hash;

            // When
            var state = new StandardServerScriptsState();
            state.UpdateHash(
                hash
            );

            var actual = state.CurrentHash;

            // Then
            actual
                .Should().Be(expected);
        }
    }
}
