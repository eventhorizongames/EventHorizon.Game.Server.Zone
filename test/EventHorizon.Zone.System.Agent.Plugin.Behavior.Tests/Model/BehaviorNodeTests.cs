namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Model
{
    using global::System;

    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

    using Xunit;
    using FluentAssertions;

    public class BehaviorNodeTests
    {
        [Fact]
        public void ShouldThrowArgumentExceptionWhenNullBehaviorNodeIsPassedIn()
        {
            // Given
            var expectedParam = "serailzedNode";
            var expectedMessage = $"BehaviorNode requires a valid SerializedBehaviorNode (Parameter 'serailzedNode')";

            // When
            Func<BehaviorNode> testAction = () => new BehaviorNode(0, null);

            var actual = testAction.Should().Throw<ArgumentException>();

            // Then
            actual.WithMessage(expectedMessage)
                .Which
                .ParamName.Should().Be(expectedParam);
        }

        [Fact]
        public void TestShouldShowTokenAndTypeWhenToStringIsCalled()
        {
            // Given
            var type = BehaviorNodeType.ACTION;

            // When
            var node = new BehaviorNode(
                0,
                new SerializedBehaviorNode
                {
                    Type = type.ToString()
                }
            );
            var token = node.Token;
            var expected = $"{token} : {type}";

            var actual = node.ToString();

            // Then
            actual.Should().Be(expected);
        }
    }
}
