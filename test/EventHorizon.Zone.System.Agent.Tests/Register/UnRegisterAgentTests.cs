using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using MediatR;
using Moq;
using Xunit;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.System.Agent.UnRegister;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Register
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
                mediator => mediator.Publish(
                    It.IsAny<AgentUnRegisteredEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<UnRegisterEntityEvent>(),
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
            var agent = new AgentEntity(
                new Dictionary<string, object>()
            )
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
                mediator => mediator.Publish(
                    It.IsAny<UnRegisterEntityEvent>(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<AgentUnRegisteredEvent>(),
                    CancellationToken.None
                )
            );
        }
    }
}