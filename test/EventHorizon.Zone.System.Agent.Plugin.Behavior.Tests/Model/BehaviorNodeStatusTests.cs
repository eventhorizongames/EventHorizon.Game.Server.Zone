namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Model
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

    using Xunit;

    public class BehaviorNodeStatusTests
    {
        [Fact]
        public void ShouldReturnFalseWhenTryingToEqualsNotRelatedObject()
        {
            // Given
            var expected = false;

            // When
            var actual = BehaviorNodeStatus.ERROR.Equals(
                1000L
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void GetHashCodeShouldNotReturnZeroWhenCalled()
        {
            // Given
            var expected = true;

            // When
            var actual = BehaviorNodeStatus.ERROR.GetHashCode();

            // Then
            Assert.Equal(
                expected,
                actual > 0
            );
        }
    }
}
