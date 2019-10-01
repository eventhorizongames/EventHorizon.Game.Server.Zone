using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Model
{
    public class AgentBehaviorTests
    {
        [Fact]
        public void TestShouldBeAbleToAccessTreeIdThroughIndexBasedLookup()
        {
            // Given
            var expected = "tree-id";
            var treeIdIndex = "treeId";

            // When
            var actual = new AgentBehavior();
            actual[treeIdIndex] = "tree-id";

            // Then
            Assert.Equal(
                expected,
                actual[treeIdIndex]
            );
        }

        [Fact]
        public void TestShouldReturnNullWhenIndexIsNotSupported()
        {
            // Given
            var invalidIndex = "invalid-index";

            // When
            var agentBehavior = new AgentBehavior();
            agentBehavior[invalidIndex] = "something-invalid";
            var actual = agentBehavior[invalidIndex];

            // Then
            Assert.Null(
                actual
            );
        }
    }
}