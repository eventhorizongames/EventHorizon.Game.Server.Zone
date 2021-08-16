namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Register
{
    using EventHorizon.Zone.Core.Events.Entity.Movement;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Move;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.Path;
    using EventHorizon.Zone.System.Agent.Model.State;
    using EventHorizon.Zone.System.Agent.Move.Register;
    using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

    using global::System;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class MoveRegisteredAgentHandlerTests
    {
        [Fact]
        public async Task ShouldPublishMoveEventForAgent()
        {
            // Given
            var inputId = 123L;
            var expectedMoveTo = new Vector3(
                2,
                2,
                2
            );
            var expectedAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = inputId,
                Transform = new TransformState
                {
                    Position = new Vector3(
                        4,
                        4,
                        4
                    ),
                },
            };
            expectedAgent.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                new LocationState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromMinutes(1)
                    ),
                    CurrentZone = "current-zone",
                    ZoneTag = "current-tag"
                }
            );
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState()
                {
                    MoveTo = expectedMoveTo
                }.SetPath(
                    new Queue<Vector3>(
                        new List<Vector3>()
                        {
                            expectedMoveTo,
                            expectedMoveTo
                        }
                    )
                )
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
                new MoveRegisteredAgentEvent(
                    inputId
                ),
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
                mock => mock.Send(
                    It.IsAny<QueueAgentToMove>(),
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
        public async Task ShouldOnlyRemoveAgentWhenNoPathIsNull()
        {
            // Given
            var inputId = 123L;
            var expectedAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = inputId,
            };
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState
                {
                }.SetPath(
                    null
                )
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
                new MoveRegisteredAgentEvent(
                    inputId
                ),
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
                    It.IsAny<QueueAgentToMove>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldDifferProcessingToFutureDateIfNextMoveRequestisStillInFuture()
        {
            // Given
            var inputId = 123L;
            var expectedAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = inputId,
            };
            expectedAgent.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                new LocationState
                {
                    NextMoveRequest = DateTime.Now.Add(
                        TimeSpan.FromMinutes(
                            1
                        )
                    ),
                }
            );
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState()
                    .SetPath(
                        new Queue<Vector3>(
                            new List<Vector3>()
                        )
                    )
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
                new MoveRegisteredAgentEvent(
                    inputId
                ),
                CancellationToken.None
            );

            // Then
            agentRepositoryMock.Verify(
                repository => repository.FindById(
                    inputId
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    It.IsAny<QueueAgentToMove>(),
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
        public async Task ShouldRemoveAgentIfPathQueueIsEmpty()
        {
            // Given
            var inputId = 123L;
            var expectedAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = inputId,
            };
            expectedAgent.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                new LocationState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromMinutes(
                            1
                        )
                    ),
                }
            );
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState()
                    .SetPath(
                        new Queue<Vector3>(
                            new List<Vector3>()
                        )
                    )
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
                new MoveRegisteredAgentEvent(
                    inputId
                ),
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
        public async Task ShouldProcessThenRemoveAgentIfPathQueueIsEmptyAfterProcessing()
        {
            // Given
            var inputId = 123;
            var expectedMoveTo = new Vector3(2, 2, 2);
            var expectedAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = inputId,
                Transform = new TransformState
                {
                    Position = new Vector3(
                        4,
                        4,
                        4
                    ),
                },
            };
            expectedAgent.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                new LocationState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromMinutes(
                            1
                        )
                    ),
                    CurrentZone = "current-zone",
                    ZoneTag = "current-tag"
                }
            );
            expectedAgent.PopulateData<PathState>(
                PathState.PROPERTY_NAME,
                new PathState
                {
                    MoveTo = expectedMoveTo
                }.SetPath(
                    new Queue<Vector3>(
                        new List<Vector3>()
                        {
                            expectedMoveTo
                        }
                    )
                )
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
                new MoveRegisteredAgentEvent(
                    inputId
                ),
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
                    It.IsAny<QueueAgentToMove>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldNotDoAnythingWhenAgentIsNotFound()
        {
            // Given
            var inputId = 123L;
            var agent = default(AgentEntity);

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var agentRepositoryMock = new Mock<IAgentRepository>();

            agentRepositoryMock.Setup(
                repository => repository.FindById(
                    inputId
                )
            ).ReturnsAsync(
                agent
            );

            // When
            var moveRegisteredAgentHandler = new MoveRegisteredAgentHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                agentRepositoryMock.Object
            );
            await moveRegisteredAgentHandler.Handle(
                new MoveRegisteredAgentEvent(
                    inputId
                ),
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
                    It.IsAny<MoveEntityToPositionCommand>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<QueueAgentToMove>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }
    }
}
