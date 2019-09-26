using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.UnRegister;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.UnRegister;
using MediatR;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.UnRegister.UnRegisterAgent;
using EventHorizon.Zone.System.Agent.Events.Get;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.UnRegister
{
    public class UnRegisterAgentTests
    {
        [Fact]
        public async Task ShouldNotSendAnyEventsWhenAgentIsNotFound()
        {
            // Given
            var agentId = "123";
            var agent = default(AgentEntity);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mediator => mediator.Send(
                    new FindAgentByIdEvent(
                        agentId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agent
            );

            // When
            var unRegisterAgentHandler = new UnRegisterAgentHandler(
                mediatorMock.Object
            );

            await unRegisterAgentHandler.Handle(
                new UnRegisterAgent(
                    agentId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Send(
                    It.IsAny<FindAgentByIdEvent>(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    It.IsAny<UnRegisterActorWithBehaviorTreeUpdate>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<UnregisterEntityEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
        [Fact]
        public async Task ShouldSendExpectedEventsWhenAgentIsFound()
        {
            // Given
            var agentId = "123";
            var agent = new AgentEntity
            {
                AgentId = agentId
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mediator => mediator.Send(
                    new FindAgentByIdEvent(
                        agentId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agent
            );

            // When
            var unRegisterAgentHandler = new UnRegisterAgentHandler(
                mediatorMock.Object
            );

            await unRegisterAgentHandler.Handle(
                new UnRegisterAgent(
                    agentId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Send(
                    It.IsAny<FindAgentByIdEvent>(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    It.IsAny<UnRegisterActorWithBehaviorTreeUpdate>(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<UnregisterEntityEvent>(),
                    CancellationToken.None
                )
            );
        }
    }
}