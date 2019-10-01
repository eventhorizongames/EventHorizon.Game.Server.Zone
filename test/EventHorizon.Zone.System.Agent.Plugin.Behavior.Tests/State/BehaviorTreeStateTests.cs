using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.State
{
    public class BehaviorTreeStateTests
    {
        [Fact]
        public void TestShouldBeInvalidNodeWhenActiveNodeTokenIsZero()
        {
            // Given
            var treeShape = new ActorBehaviorTreeShape(
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            // When
            var behaviorTreeState = new BehaviorTreeState(
                treeShape
            );
            var actual = behaviorTreeState.IsActiveNodeValidAndNotRunning();

            // Then
            Assert.False(
                actual
            );
        }
    }
}