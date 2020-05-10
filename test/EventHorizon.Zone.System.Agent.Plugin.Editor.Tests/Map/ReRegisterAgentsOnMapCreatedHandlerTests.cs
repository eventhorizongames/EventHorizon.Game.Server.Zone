namespace EventHorizon.Zone.System.Agent.Plugin.Editor.Tests.Map
{
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Register;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Editor.Map;
    using MediatR;
    using Moq;
    using Xunit;

    public class ReRegisterAgentsOnMapCreatedHandlerTests
    {
        [Fact]
        public async Task ShouldUnRegisterThenRegisterAgentWhenReturnedFromReturnedFromGetAgentListEvent()
        {
            // Given
            var agent1Id = "agent-1-id";
            var agentEntity1 = new AgentEntity
            {
                AgentId = agent1Id,
            };
            var agent2Id = "agent-2-id";
            var agentEntity2 = new AgentEntity
            {
                AgentId = agent2Id,
            };
            var agentList = new List<AgentEntity>
            {
                agentEntity1,
                agentEntity2,
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<GetAgentListEvent>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agentList
            );

            // When
            var handler = new ReRegisterAgentsOnMapCreatedHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new MapCreatedEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new UnRegisterAgent(
                        agent1Id
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new RegisterAgentEvent(
                        agentEntity1
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new UnRegisterAgent(
                        agent2Id
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new RegisterAgentEvent(
                        agentEntity2
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}