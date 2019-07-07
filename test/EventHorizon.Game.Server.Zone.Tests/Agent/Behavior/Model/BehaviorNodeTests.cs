using System;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.Model
{
    public class BehaviorNodeTests
    {
        [Fact]
        public void ShouldThrowArgumentExceptionWhenNullBehaviorNodeIsPassedIn()
        {
            // Given
            var expectedParam = "serailzedNode";
            var expectedMessage = "BehaviorNode requires a valid SerializedBehaviorNode\r\nParameter name: serailzedNode";

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