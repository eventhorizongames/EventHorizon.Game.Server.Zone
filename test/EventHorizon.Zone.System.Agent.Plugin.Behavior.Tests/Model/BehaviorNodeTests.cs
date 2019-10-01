using System;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Model
{
    public class BehaviorNodeTests
    {
        [Fact]
        public void ShouldThrowArgumentExceptionWhenNullBehaviorNodeIsPassedIn()
        {
            // Given
            var expectedParam = "serailzedNode";
            var expectedMessage = $"BehaviorNode requires a valid SerializedBehaviorNode{Environment.NewLine}Parameter name: serailzedNode";

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
    }
}