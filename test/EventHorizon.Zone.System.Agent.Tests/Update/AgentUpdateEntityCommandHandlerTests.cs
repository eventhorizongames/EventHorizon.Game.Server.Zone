namespace EventHorizon.Zone.System.Agent.Tests.Update
{
    using EventHorizon.Zone.System.Agent.Events.Update;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.State;
    using EventHorizon.Zone.System.Agent.Update;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class AgentUpdateEntityCommandHandlerTests
    {
        [Fact]
        public async Task ShouldUpdateAgentWhenHandlingRequest()
        {
            // Given
            var agent = new AgentEntity();
            var updateAction = AgentAction.SCRIPT;
            var request = new AgentUpdateEntityCommand(
                agent,
                updateAction
            );

            var agentRepositoryMock = new Mock<IAgentRepository>();

            // When
            var handler = new AgentUpdateEntityCommandHandler(
                agentRepositoryMock.Object
            );
            await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            agentRepositoryMock.Verify(
                mock => mock.Update(
                    updateAction,
                    agent
                )
            );
        }
    }
}