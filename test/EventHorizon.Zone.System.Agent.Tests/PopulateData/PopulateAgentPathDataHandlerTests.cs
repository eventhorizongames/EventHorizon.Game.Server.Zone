namespace EventHorizon.Zone.System.Agent.Tests.PopulateData
{
    using global::System.Collections.Concurrent;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Agent.Events.PopulateData;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.Path;
    using EventHorizon.Zone.System.Agent.PopulateData;
    using Xunit;
    using EventHorizon.Zone.Core.Model.Entity;
    using FluentAssertions;

    public class PopulateAgentPathDataHandlerTests
    {
        [Fact]
        public async Task ShouldPopulateNewPathWhenAgentDoesNotHavePathState()
        {
            // Given
            var expected = PathState.NEW;
            var agent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            );

            // When
            var handler = new PopulateAgentPathDataHandler();
            await handler.Handle(
                new PopulateAgentEntityDataEvent
                {
                    Agent = agent,
                },
                CancellationToken.None
            );

            var actual = agent.GetProperty<PathState>(
                PathState.PROPERTY_NAME
            );
            // Then
            actual.Should()
                .Be(expected);
        }
    }
}