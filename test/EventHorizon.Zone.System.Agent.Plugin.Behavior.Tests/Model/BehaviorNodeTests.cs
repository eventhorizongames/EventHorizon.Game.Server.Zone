namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Model
{
    using global::System;

    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

    using Xunit;

    public class BehaviorNodeTests
    {
        [Fact]
        public void ShouldThrowArgumentExceptionWhenNullBehaviorNodeIsPassedIn()
        {
            // Given
            var expectedParam = "serailzedNode";
            var expectedMessage = $"BehaviorNode requires a valid SerializedBehaviorNode (Parameter 'serailzedNode')";

            // When
            Action testAction = () => new BehaviorNode(null);

            var actual = Record.Exception(
                testAction
            ) as ArgumentException;

            // Then
            Assert.IsType<ArgumentException>(
                actual
            );
            Assert.Equal(
                expectedParam,
                actual.ParamName
            );
            Assert.Equal(
                expectedMessage,
                actual.Message
            );
        }

        [Fact]
        public void TestShouldShowTokenAndTypeWhenToStringIsCalled()
        {
            // Given
            var type = BehaviorNodeType.ACTION;

            // When
            var actual = new BehaviorNode(
                new SerializedBehaviorNode
                {
                    Type = type.ToString()
                }
            );
            var token = actual.Token;
            var expected = $"{token} : {type}";

            // Then
            Assert.Equal(
                expected,
                actual.ToString()
            );
        }
    }
}
