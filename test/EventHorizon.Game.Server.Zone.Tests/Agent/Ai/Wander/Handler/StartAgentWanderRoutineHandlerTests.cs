using Xunit;
using Moq;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Ai.Wander.Handler;
using EventHorizon.Game.Server.Zone.Agent.Ai.Wander;
using EventHorizon.Game.Server.Zone.Core.RandomNumber;
using System.Dynamic;
using EventHorizon.Game.Server.Zone.Agent.Model;
using System.Collections.Generic;
using System.Numerics;
using MediatR;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Map;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Model.Map;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Events.Map;
using EventHorizon.Game.Server.Zone.External.RandomNumber;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Ai.Wander.Handler
{
    public class StartAgentWanderRoutineHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldCallExpectedEvents()
        {
            // Given
            var inputId = 123L;
            var inputCurrentPosition = new Vector3(3);
            var inputToPosition = new Vector3(100);
            var inputLookDistance = 100;
            var inputGetAgentEvent = new GetAgentEvent
            {
                AgentId = inputId
            };
            var inputGetMapNodesAroundPositionEvent = new GetMapNodesAroundPositionEvent
            {
                Position = inputCurrentPosition,
                Distance = inputLookDistance
            };

            var inputNodePosition = new Vector3(30);
            var expectedMapNodes = new List<MapNode>();
            expectedMapNodes.Add(new MapNode(inputNodePosition));
            var inputStartAgentMoveRoutineEvent = new StartAgentMoveRoutineEvent
            {
                AgentId = inputId,
                ToPosition = inputNodePosition
            };

            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    CurrentPosition = inputCurrentPosition
                },
                RawData = new Dictionary<string, object>()
                {
                    {
                        "Wander",
                        new AgentWanderState
                        {
                            LookDistance = inputLookDistance
                        }
                    }
                }
            };
            expectedAgent.PopulateData<AgentWanderState>("Wander");

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator.Send(inputGetAgentEvent, CancellationToken.None))
                    .ReturnsAsync(expectedAgent);
            mediatorMock.Setup(mediator => mediator.Send(inputGetMapNodesAroundPositionEvent, CancellationToken.None))
                    .ReturnsAsync(expectedMapNodes);
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            // When
            var handler = new StartAgentWanderRoutineHandler(mediatorMock.Object, randomNumberGeneratorMock.Object);
            await handler.Handle(new StartAgentWanderRoutineEvent
            {
                AgentId = inputId
            }, CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Send(
                inputGetAgentEvent,
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Send(
                inputGetMapNodesAroundPositionEvent,
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Publish(
                inputStartAgentMoveRoutineEvent,
                It.IsAny<CancellationToken>()
            ));
        }
        [Fact]
        public async Task TestHandle_ShouldNotCallEventAfterGetAgentEventWhenAgentIsNotFound()
        {
            // Given
            var inputId = 123L;
            var inputGetAgentEvent = new GetAgentEvent
            {
                AgentId = inputId
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator.Send(inputGetAgentEvent, CancellationToken.None))
                .ReturnsAsync(default(AgentEntity));
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            // When
            var handler = new StartAgentWanderRoutineHandler(mediatorMock.Object, randomNumberGeneratorMock.Object);
            await handler.Handle(new StartAgentWanderRoutineEvent
            {
                AgentId = inputId
            }, CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Send(
                inputGetAgentEvent,
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Send(
                It.IsAny<GetMapNodesAroundPositionEvent>(),
                It.IsAny<CancellationToken>()
            ), Times.Never());
            mediatorMock.Verify(mediator => mediator.Publish(
                It.IsAny<StartAgentMoveRoutineEvent>(),
                It.IsAny<CancellationToken>()
            ), Times.Never());
        }
        [Fact]
        public async Task TestHandle_ShouldNotCallStartAgentMoveRoutineEventWhenNoMapNodesAreFound()
        {
            // Given
            var inputId = 123L;
            var inputCurrentPosition = new Vector3(3);
            var inputToPosition = new Vector3(100);
            var inputLookDistance = 100;
            var inputGetAgentEvent = new GetAgentEvent
            {
                AgentId = inputId
            };
            var inputGetMapNodesAroundPositionEvent = new GetMapNodesAroundPositionEvent
            {
                Position = inputCurrentPosition,
                Distance = inputLookDistance
            };

            var inputNodePosition = new Vector3(30);
            var expectedMapNodes = new List<MapNode>();

            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    CurrentPosition = inputCurrentPosition
                },
                RawData = new Dictionary<string, object>() {
                    {
                        "Wander",
                        new AgentWanderState
                        {
                            LookDistance = inputLookDistance
                        }
                    }
                }
            };
            expectedAgent.PopulateData<AgentWanderState>("Wander");

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator.Send(inputGetAgentEvent, CancellationToken.None))
                .ReturnsAsync(expectedAgent);
            mediatorMock.Setup(mediator => mediator.Send(inputGetMapNodesAroundPositionEvent, CancellationToken.None))
                .ReturnsAsync(expectedMapNodes);
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            // When
            var handler = new StartAgentWanderRoutineHandler(mediatorMock.Object, randomNumberGeneratorMock.Object);
            await handler.Handle(new StartAgentWanderRoutineEvent
            {
                AgentId = inputId
            }, CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Send(
                inputGetAgentEvent,
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Send(
                inputGetMapNodesAroundPositionEvent,
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Publish(
                It.IsAny<StartAgentMoveRoutineEvent>(),
                It.IsAny<CancellationToken>()
            ), Times.Never());
        }
    }
}