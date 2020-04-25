namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Run
{
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
    using global::System;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class IsValidActorBehaviorTickHandlerTests
    {
        [Fact]
        public async Task ShouldReturnIsValidResponseWhenAppValidationTasksPass()
        {
            // Given
            var now = DateTime.MinValue.AddSeconds(10);
            var nextTickRequest = DateTime.MinValue.AddSeconds(5);
            var actorId = 1L;
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = actorId
            }.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                new AgentBehavior
                {
                    NextTickRequest = nextTickRequest,
                }
            );
            var shapeId = "shape";
            var shape = new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var expected = new ActorBehaviorTickValidationResponse(
                actor,
                shape
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        actorId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                actor
            );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId
                )
            ).Returns(
                shape
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var actual = await handler.Handle(
                new IsValidActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task ShouldReturnIsValidFalseResponseWhenActorIsNull()
        {
            // Given
            var actorId = 1L;
            var actor = null as IObjectEntity;
            var shapeId = "shape";
            var shape = new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var expected = new ActorBehaviorTickValidationResponse(
                null,
                default
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        actorId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                actor
            );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId
                )
            ).Returns(
                shape
            );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var actual = await handler.Handle(
                new IsValidActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task ShouldReturnIsValidFalseResponseWhenActorIsNotFound()
        {
            // Given
            var actorId = 1L;
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );
            var shapeId = "shape";
            var shape = new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var expected = new ActorBehaviorTickValidationResponse(
                null,
                default
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        actorId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                actor
            );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId
                )
            ).Returns(
                shape
            );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var actual = await handler.Handle(
                new IsValidActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task ShouldReturnIsValidFalseWhenNextTickRequestDateTimeHasNotPassed()
        {
            // Given
            var now = DateTime.MinValue.AddSeconds(5);
            var nextTickRequest = DateTime.MinValue.AddSeconds(10);
            var actorId = 1L;
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = actorId
            }.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                new AgentBehavior
                {
                    NextTickRequest = nextTickRequest,
                }
            );
            var shapeId = "shape";
            var shape = new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var expected = new ActorBehaviorTickValidationResponse(
                null,
                default
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        actorId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                actor
            );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId
                )
            ).Returns(
                shape
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var actual = await handler.Handle(
                new IsValidActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task ShouldRegisterForNextTickCycleWhenNextTickRequestDateTimeHasNotPassed()
        {
            // Given
            var now = DateTime.MinValue.AddSeconds(5);
            var nextTickRequest = DateTime.MinValue.AddSeconds(10);
            var actorId = 1L;
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = actorId
            }.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                new AgentBehavior
                {
                    NextTickRequest = nextTickRequest,
                }
            );
            var shapeId = "shape";
            var shape = new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var expected = new ActorBehaviorTickValidationResponse(
                null,
                default
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        actorId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                actor
            );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId
                )
            ).Returns(
                shape
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var actual = await handler.Handle(
                new IsValidActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            queueMock.Verify(
                mock => mock.Register(
                    shapeId,
                    actorId
                )
            );
        }

        [Fact]
        public async Task ShouldUpdateActorAgentBehaviorWhenNextTickRequestDateTimeHasPassed()
        {
            // Given
            var now = DateTime.MinValue.AddSeconds(10);
            var nextTickRequest = DateTime.MinValue.AddSeconds(5);
            var expectedNextTickRequest = now.AddSeconds(15);
            var actorId = 1L;
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = actorId
            }.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                new AgentBehavior
                {
                    NextTickRequest = nextTickRequest,
                }
            );
            var shapeId = "shape";
            var shape = new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        actorId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                actor
            );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId
                )
            ).Returns(
                shape
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var response = await handler.Handle(
                new IsValidActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            var actual = response.Actor.GetProperty<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME
            );

            // Then
            Assert.Equal(
                expectedNextTickRequest,
                actual.NextTickRequest
            );
        }

        [Fact]
        public async Task ShouldReturnIsValidFalseWhenBehaviorTreeShapeIsNotInRepository()
        {
            // Given
            var now = DateTime.MinValue.AddSeconds(10);
            var nextTickRequest = DateTime.MinValue.AddSeconds(5);
            var actorId = 1L;
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = actorId
            }.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                new AgentBehavior
                {
                    NextTickRequest = nextTickRequest,
                }
            );
            var shapeId = "shape";
            var shape = default(ActorBehaviorTreeShape);
            var expectedActorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var expected = new ActorBehaviorTickValidationResponse(
                null,
                default
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        actorId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                actor
            );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId
                )
            ).Returns(
                shape
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var actual = await handler.Handle(
                new IsValidActorBehaviorTick(
                    expectedActorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            queueMock.Verify(
                mock => mock.RegisterFailed(
                    expectedActorBehaviorTick
                )
            );
        }

        [Fact]
        public async Task ShouldReturnIsValidFalseWhenBehaviorTreeShapeIsEmpty()
        {
            // Given
            var now = DateTime.MinValue.AddSeconds(10);
            var nextTickRequest = DateTime.MinValue.AddSeconds(5);
            var actorId = 1L;
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = actorId
            }.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                new AgentBehavior
                {
                    NextTickRequest = nextTickRequest,
                }
            );
            var shapeId = "shape";
            var shape = new ActorBehaviorTreeShape
            {
                NodeList = new List<BehaviorNode>(),
            };
            var expectedActorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var expected = new ActorBehaviorTickValidationResponse(
                null,
                default
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        actorId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                actor
            );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId
                )
            ).Returns(
                shape
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var actual = await handler.Handle(
                new IsValidActorBehaviorTick(
                    expectedActorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            queueMock.Verify(
                mock => mock.RegisterFailed(
                    expectedActorBehaviorTick
                )
            );
        }

        [Fact]
        public async Task ShouldSetNextTickRequestWhenActorTreeShapeDoesNotMatchPassedInTreeShape()
        {
            // Given
            var now = DateTime.MinValue.AddSeconds(10);
            var nextTickRequest = DateTime.MinValue.AddSeconds(5);
            var expectedNextTickRequest = now.AddMilliseconds(100);
            var actorId = 1L;
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = actorId
            }.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                new AgentBehavior
                {
                    NextTickRequest = nextTickRequest,
                }
            ).SetProperty(
                BehaviorTreeState.PROPERTY_NAME,
                new BehaviorTreeState(
                    new ActorBehaviorTreeShape(
                        "not-same-shape-id",
                        new SerializedAgentBehaviorTree
                        {
                            Root = new SerializedBehaviorNode()
                        }
                    )
                )
            );
            var shapeId = "shape";
            var shape = new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var expected = new ActorBehaviorTickValidationResponse(
                actor,
                default
            );

            var loggerMock = new Mock<ILogger<IsValidActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                        new GetEntityByIdEvent(
                            actorId
                        ),
                        CancellationToken.None
                    )
                ).ReturnsAsync(
                    actor
                );

            repositoryMock.Setup(
                mock => mock.FindTreeShape(
                    shapeId

                )
                ).Returns(
                    shape
                );

            dateTimeMock.Setup(
                mock => mock.Now
                ).Returns(
                    now
                );

            // When
            var handler = new IsValidActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object,
                dateTimeMock.Object,
                queueMock.Object
            );
            var actual = await handler.Handle(
                new IsValidActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            var actualNextTickRequest = actual.Actor.GetProperty<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME
            ).NextTickRequest;

            // Then
            Assert.Equal(
                expected,
                actual
            );
            Assert.Equal(
                expectedNextTickRequest,
                actualNextTickRequest
            );
        }
    }
}