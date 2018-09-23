using Xunit;
using Moq;
using MediatR;
using Microsoft.Extensions.Logging;
using EventHorizon.Game.Server.Zone.Agent.Move.Handler;
using EventHorizon.Game.Server.Zone.State.Repository;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Move;
using System.Threading;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Client;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Core.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class MoveRegisteredAgentHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldPublishMoveEventForAgent()
        {
            // Given
            var inputId = 123;
            var expectedMoveTo = new Vector3(2, 2, 2);
            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    NextMoveRequest = DateTime.Now.Subtract(TimeSpan.FromMinutes(1)),
                    CurrentPosition = new Vector3(4, 4, 4),
                    CurrentZone = "current-zone",
                    ZoneTag = "current-tag"
                },
                Path = new Queue<Vector3>(new List<Vector3>() { expectedMoveTo, expectedMoveTo }),
                Speed = 1
            };
            var expectedClientActionEvent = new ClientActionEvent
            {
                Action = "EntityClientMove",
                Data = new EntityClientMoveData
                {
                    EntityId = inputId,
                    MoveTo = expectedMoveTo
                }
            };

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(repository => repository.FindById(inputId)).ReturnsAsync(expectedAgent);
            var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

            // When
            var clearAgentRoutineHandler = new MoveRegisteredAgentHandler(
                loggerMock.Object,
                mediatorMock.Object,
                agentRepositoryMock.Object,
                moveAgentRepositoryMock.Object
            );
            await clearAgentRoutineHandler.Handle(new MoveRegisteredAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None);
            // Then
            agentRepositoryMock.Verify(repository => repository.FindById(inputId));
            agentRepositoryMock.Verify(repository => repository.Update(EntityAction.POSITION, It.IsAny<AgentEntity>())); // TODO: Look at validating this differently
            mediatorMock.Verify(mediator => mediator.Publish(expectedClientActionEvent, It.IsAny<CancellationToken>()));

            mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<AgentFinishedMoveEvent>(), It.IsAny<CancellationToken>()), Times.Never());
            moveAgentRepositoryMock.Verify(repository => repository.Remove(It.IsAny<long>()), Times.Never());
        }

        [Fact]
        public async Task TestHandle_ShouldOnlyRemoveAgentWhenNoPathIsNull()
        {
            // Given
            var inputId = 123;
            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Path = null,
            };
            var expectedAgentFinishedMoveEvent = new AgentFinishedMoveEvent
            {
                AgentId = inputId
            };

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(repository => repository.FindById(inputId)).ReturnsAsync(expectedAgent);
            var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

            // When
            var clearAgentRoutineHandler = new MoveRegisteredAgentHandler(
                loggerMock.Object,
                mediatorMock.Object,
                agentRepositoryMock.Object,
                moveAgentRepositoryMock.Object
            );
            await clearAgentRoutineHandler.Handle(new MoveRegisteredAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None);
            // Then
            agentRepositoryMock.Verify(repository => repository.FindById(inputId));
            agentRepositoryMock.Verify(repository => repository.Update(It.IsAny<EntityAction>(), It.IsAny<AgentEntity>()), Times.Never());
            mediatorMock.Verify(mediator => mediator.Publish(expectedAgentFinishedMoveEvent, It.IsAny<CancellationToken>()));
            mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<ClientActionEvent>(), It.IsAny<CancellationToken>()), Times.Never());
            moveAgentRepositoryMock.Verify(repository => repository.Remove(inputId));
        }
        [Fact]
        public async Task TestHandle_ShouldDifferProcessingToFutureDateIfNextMoveRequestisStillInFuture()
        {
            // Given
            var inputId = 123;
            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    NextMoveRequest = DateTime.Now.Add(TimeSpan.FromMinutes(1)),
                },
                Path = new Queue<Vector3>(new List<Vector3>() { }),
                Speed = 1
            };

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(repository => repository.FindById(inputId)).ReturnsAsync(expectedAgent);
            var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

            // When
            var clearAgentRoutineHandler = new MoveRegisteredAgentHandler(
                loggerMock.Object,
                mediatorMock.Object,
                agentRepositoryMock.Object,
                moveAgentRepositoryMock.Object
            );
            await clearAgentRoutineHandler.Handle(new MoveRegisteredAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None);
            // Then
            agentRepositoryMock.Verify(repository => repository.FindById(inputId));
            agentRepositoryMock.Verify(repository => repository.Update(It.IsAny<EntityAction>(), It.IsAny<AgentEntity>()), Times.Never());
            mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<AgentFinishedMoveEvent>(), It.IsAny<CancellationToken>()), Times.Never());
            mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<ClientActionEvent>(), It.IsAny<CancellationToken>()), Times.Never());
            moveAgentRepositoryMock.Verify(repository => repository.Remove(It.IsAny<long>()), Times.Never());
        }
        [Fact]
        public async Task TestHandle_ShouldRemoveAgentIfPathQueueIsEmpty()
        {
            // Given
            var inputId = 123;
            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    NextMoveRequest = DateTime.Now.Subtract(TimeSpan.FromMinutes(1)),
                },
                Path = new Queue<Vector3>(new List<Vector3>() { }),
                Speed = 1
            };
            var expectedAgentFinishedMoveEvent = new AgentFinishedMoveEvent
            {
                AgentId = inputId
            };

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(repository => repository.FindById(inputId)).ReturnsAsync(expectedAgent);
            var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

            // When
            var clearAgentRoutineHandler = new MoveRegisteredAgentHandler(
                loggerMock.Object,
                mediatorMock.Object,
                agentRepositoryMock.Object,
                moveAgentRepositoryMock.Object
            );
            await clearAgentRoutineHandler.Handle(new MoveRegisteredAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None);
            // Then
            agentRepositoryMock.Verify(repository => repository.FindById(inputId));
            moveAgentRepositoryMock.Verify(repository => repository.Remove(inputId));
            mediatorMock.Verify(mediator => mediator.Publish(expectedAgentFinishedMoveEvent, It.IsAny<CancellationToken>()));

            agentRepositoryMock.Verify(repository => repository.Update(It.IsAny<EntityAction>(), It.IsAny<AgentEntity>()), Times.Never());
            mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<ClientActionEvent>(), It.IsAny<CancellationToken>()), Times.Never());
        }
        [Fact]
        public async Task TestHandle_ShouldProcessThenRemoveAgentIfPathQueueIsEmptyAfterProcessing()
        {
            // Given
            var inputId = 123;
            var expectedMoveTo = new Vector3(2, 2, 2);
            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    NextMoveRequest = DateTime.Now.Subtract(TimeSpan.FromMinutes(1)),
                    CurrentPosition = new Vector3(4, 4, 4),
                    CurrentZone = "current-zone",
                    ZoneTag = "current-tag"
                },
                Path = new Queue<Vector3>(new List<Vector3>() { expectedMoveTo }),
                Speed = 1
            };
            var expectedClientActionEvent = new ClientActionEvent
            {
                Action = "EntityClientMove",
                Data = new EntityClientMoveData
                {
                    EntityId = inputId,
                    MoveTo = expectedMoveTo
                }
            };
            var expectedAgentFinishedMoveEvent = new AgentFinishedMoveEvent
            {
                AgentId = inputId
            };

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(repository => repository.FindById(inputId)).ReturnsAsync(expectedAgent);
            var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

            // When
            var clearAgentRoutineHandler = new MoveRegisteredAgentHandler(
                loggerMock.Object,
                mediatorMock.Object,
                agentRepositoryMock.Object,
                moveAgentRepositoryMock.Object
            );
            await clearAgentRoutineHandler.Handle(new MoveRegisteredAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None);
            // Then
            agentRepositoryMock.Verify(repository => repository.FindById(inputId));
            agentRepositoryMock.Verify(repository => repository.Update(EntityAction.POSITION, It.IsAny<AgentEntity>())); // TODO: Look at validating this differently
            mediatorMock.Verify(mediator => mediator.Publish(expectedClientActionEvent, It.IsAny<CancellationToken>()));

            mediatorMock.Verify(mediator => mediator.Publish(expectedAgentFinishedMoveEvent, It.IsAny<CancellationToken>()));
            moveAgentRepositoryMock.Verify(repository => repository.Remove(inputId));
        }
    }
}