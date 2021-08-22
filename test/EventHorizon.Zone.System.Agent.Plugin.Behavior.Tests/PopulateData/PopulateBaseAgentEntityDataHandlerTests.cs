namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.PopulateData
{
    using global::System.Collections.Concurrent;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.PopulateData;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.PopulateData;

    using Xunit;

    public class PopulateBaseAgentEntityDataHandlerTests
    {
        [Fact]
        public async Task TestShouldPopulateAgentBehaviorWithDefaultWhenAgentTreeIdIsNull()
        {
            // Given
            var expected = "DEFAULT";
            var agent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            );
            var behaviorPropertyName = "behavior";

            // When
            var handler = new PopulateBaseAgentEntityDataHandler();
            await handler.Handle(
                new PopulateAgentEntityDataEvent
                {
                    Agent = agent
                },
                CancellationToken.None
            );
            var actual = agent.GetProperty<AgentBehavior>(
                behaviorPropertyName
            );

            // Then
            Assert.Equal(
                expected,
                actual.TreeId
            );
        }
    }
}
