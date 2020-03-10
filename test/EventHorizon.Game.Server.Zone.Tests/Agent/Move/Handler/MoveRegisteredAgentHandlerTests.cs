using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;
using Moq;
using Xunit;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Move.Register;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
using EventHorizon.Zone.System.Agent.Model.Path;
using EventHorizon.Zone.Core.Events.Entity.Movement;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class MoveRegisteredAgentHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldPublishMoveEventForAgent()
        {
            // Given
            var inputId = 123L;
            var expectedMoveTo = new Vector3(
                2,
                2,
                2
            );
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = inputId,
                Position = new PositionState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromMinutes(1)
                    ),
                    CurrentPosition = new Vector3(
                        4,
                        4,
                        4
                    ),
                    CurrentZone = "current-zone",
                    ZoneTag = "current-tag"
                },
            };
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState
                {
                    Path = new Queue<Vector3>(
                        new List<Vector3>()
                        {
                            expectedMoveTo,
                            expectedMoveTo
                        }
                    ),
                    MoveTo = expectedMoveTo
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var agentRepositoryMock = new Mock<IAgentRepository>();

            dateTimeMock.Setup(
                dataTime => dataTime.Now
            ).Returns(
                DateTime.Now
            );
            agentRepositoryMock.Setup(
                repository => repository.FindById(
                    inputId
                )
            ).ReturnsAsync(
                expectedAgent
            );

            // When
            var moveRegisteredAgentHandler = new MoveRegisteredAgentHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                agentRepositoryMock.Object
            );
            await moveRegisteredAgentHandler.Handle(
                new MoveRegisteredAgentEvent
                {
                    EntityId = inputId
                },
                CancellationToken.None
            );
            // Then
            agentRepositoryMock.Verify(
                repository => repository.FindById(
                    inputId
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new MoveEntityToPositionCommand(
                        expectedAgent,
                        expectedMoveTo,
                        false
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<QueueAgentToMoveEvent>(),
                    It.IsAny<CancellationToken>()
                )
            );

            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<AgentFinishedMoveEvent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );

        }

        [Fact]
        public async Task TestHandle_ShouldOnlyRemoveAgentWhenNoPathIsNull()
        {
            // Given
            var inputId = 123L;
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = inputId,
            };
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState
                {
                    Path = null
                }
            );
            var expectedAgentFinishedMoveEvent = new AgentFinishedMoveEvent
            {
                EntityId = inputId
            };

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var agentRepositoryMock = new Mock<IAgentRepository>();

            agentRepositoryMock.Setup(
                repository => repository.FindById(
                    inputId
                )
            ).ReturnsAsync(
                expectedAgent
            );

            // When
            var moveRegisteredAgentHandler = new MoveRegisteredAgentHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                agentRepositoryMock.Object
            );
            await moveRegisteredAgentHandler.Handle(
                new MoveRegisteredAgentEvent
                {
                    EntityId = inputId
                },
                CancellationToken.None
            );
            // Then
            agentRepositoryMock.Verify(
                repository => repository.FindById(
                    inputId
                )
            );
            agentRepositoryMock.Verify(
                repository => repository.Update(
                    It.IsAny<EntityAction>(),
                    It.IsAny<AgentEntity>()
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedAgentFinishedMoveEvent,
                    It.IsAny<CancellationToken>()
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<QueueAgentToMoveEvent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestHandle_ShouldDifferProcessingToFutureDateIfNextMoveRequestisStillInFuture()
        {
            // Given
            var inputId = 123L;
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = inputId,
                Position = new PositionState
                {
                    NextMoveRequest = DateTime.Now.Add(
                        TimeSpan.FromMinutes(
                            1
                        )
                    ),
                },
            };
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState
                {
                    Path = new Queue<Vector3>(
                        new List<Vector3>()
                    ),
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            
            agentRepositoryMock.Setup(
                repository => repository.FindById(
                    inputId
                )
            ).ReturnsAsync(
                expectedAgent
            );

            // When
            var moveRegisteredAgentHandler = new MoveRegisteredAgentHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                agentRepositoryMock.Object
            );
            await moveRegisteredAgentHandler.Handle(
                new MoveRegisteredAgentEvent
                {
                    EntityId = inputId
                },
                CancellationToken.None
            );

            // Then
            agentRepositoryMock.Verify(
                repository => repository.FindById(
                    inputId
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<QueueAgentToMoveEvent>(),
                    It.IsAny<CancellationToken>()
                )
            );

            agentRepositoryMock.Verify(
                repository => repository.Update(
                    It.IsAny<EntityAction>(),
                    It.IsAny<AgentEntity>()
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<AgentFinishedMoveEvent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestHandle_ShouldRemoveAgentIfPathQueueIsEmpty()
        {
            // Given
            var inputId = 123L;
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = inputId,
                Position = new PositionState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromMinutes(
                            1
                        )
                    ),
                },
            };
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState
                {
                    Path = new Queue<Vector3>(
                        new List<Vector3>()
                    ),
                }
            );
            var expectedAgentFinishedMoveEvent = new AgentFinishedMoveEvent
            {
                EntityId = inputId
            };

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var agentRepositoryMock = new Mock<IAgentRepository>();

            dateTimeMock.Setup(
                dateTime => dateTime.Now
            ).Returns(
                DateTime.Now
            );
            agentRepositoryMock.Setup(
                repository => repository.FindById(
                    inputId
                )
            ).ReturnsAsync(
                expectedAgent
            );

            // When
            var moveRegisteredAgentHandler = new MoveRegisteredAgentHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                agentRepositoryMock.Object
            );
            await moveRegisteredAgentHandler.Handle(
                new MoveRegisteredAgentEvent
                {
                    EntityId = inputId
                },
                CancellationToken.None
            );
            // Then
            agentRepositoryMock.Verify(
                repository => repository.FindById(
                    inputId
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedAgentFinishedMoveEvent,
                    It.IsAny<CancellationToken>()
                )
            );

            agentRepositoryMock.Verify(
                repository => repository.Update(
                    It.IsAny<EntityAction>(),
                    It.IsAny<AgentEntity>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestHandle_ShouldProcessThenRemoveAgentIfPathQueueIsEmptyAfterProcessing()
        {
            // Given
            var inputId = 123;
            var expectedMoveTo = new Vector3(2, 2, 2);
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = inputId,
                Position = new PositionState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromMinutes(
                            1
                        )
                    ),
                    CurrentPosition = new Vector3(
                        4,
                        4,
                        4
                    ),
                    CurrentZone = "current-zone",
                    ZoneTag = "current-tag"
                },
            };
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState
                {
                    Path = new Queue<Vector3>(
                        new List<Vector3>()
                        {
                            expectedMoveTo
                        }
                    ),
                    MoveTo = expectedMoveTo
                }
            );
            var expectedAgentFinishedMoveEvent = new AgentFinishedMoveEvent
            {
                EntityId = inputId
            };

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            
            dateTimeMock.Setup(
                dateTime => dateTime.Now
            ).Returns(
                DateTime.Now
            );
            agentRepositoryMock.Setup(
                repository => repository.FindById(
                    inputId
                )
            ).ReturnsAsync(
                expectedAgent
            );

            // When
            var moveRegisteredAgentHandler = new MoveRegisteredAgentHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                agentRepositoryMock.Object
            );
            await moveRegisteredAgentHandler.Handle(
                new MoveRegisteredAgentEvent
                {
                    EntityId = inputId
                },
                CancellationToken.None
            );
            // Then
            agentRepositoryMock.Verify(
                repository => repository.FindById(
                    inputId
                )
            );

            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedAgentFinishedMoveEvent,
                    It.IsAny<CancellationToken>()
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<QueueAgentToMoveEvent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }
    }
}