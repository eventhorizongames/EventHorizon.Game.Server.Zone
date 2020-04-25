namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Run
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
    using global::System;
    using global::System.Collections.Concurrent;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class RunActorBehaviorTickHandlerTests
    {
        [Fact]
        public async Task ShouldNotRegisterFailedActorBehaviorTickWhenSuccesfulHandleRequest()
        {
            // Given
            var shapeId = "shape-id";
            var actorId = 1L;
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var actor = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = actorId,
            };
            var behaviorTreeShape = new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode(),
                }
            );
            var validationResponse = new ActorBehaviorTickValidationResponse(
                actor,
                behaviorTreeShape
            );

            var loggerMock = new Mock<ILogger<RunActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var kernelMock = new Mock<BehaviorInterpreterKernel>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new IsValidActorBehaviorTick(
                        actorBehaviorTick
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                validationResponse
            );

            // When
            var handler = new RunActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                kernelMock.Object,
                queueMock.Object
            );
            await handler.Handle(
                new RunActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            kernelMock.Verify(
                mock => mock.Tick(
                    behaviorTreeShape,
                    actor
                )
            );
            queueMock.Verify(
                mock => mock.RegisterFailed(
                    It.IsAny<ActorBehaviorTick>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldCallKernelWhenValidationRequestNotValid()
        {
            // Given
            var shapeId = "shape-id";
            var actorId = 1L;
            var actorBehaviorTick = new ActorBehaviorTick(
                shapeId,
                actorId
            );
            var validationResponse = new ActorBehaviorTickValidationResponse(
                null,
                default
            );

            var loggerMock = new Mock<ILogger<RunActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var kernelMock = new Mock<BehaviorInterpreterKernel>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new IsValidActorBehaviorTick(
                        actorBehaviorTick
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                validationResponse
            );

            // When
            var handler = new RunActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                kernelMock.Object,
                queueMock.Object
            );
            await handler.Handle(
                new RunActorBehaviorTick(
                    actorBehaviorTick
                ),
                CancellationToken.None
            );

            // Then
            kernelMock.Verify(
                mock => mock.Tick(
                    It.IsAny<ActorBehaviorTreeShape>(),
                    It.IsAny<IObjectEntity>()
                ),
                Times.Never()
            );
            queueMock.Verify(
                mock => mock.RegisterFailed(
                    It.IsAny<ActorBehaviorTick>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldRegisterFailedToQueueWhenGeneralExceptionIsThrown()
        {
            // Given
            var loggerMock = new Mock<ILogger<RunActorBehaviorTickHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var kernelMock = new Mock<BehaviorInterpreterKernel>();
            var queueMock = new Mock<ActorBehaviorTickQueue>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new IsValidActorBehaviorTick(
                        It.IsAny<ActorBehaviorTick>()
                    ),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception("any-exception")
            );

            // When
            var handler = new RunActorBehaviorTickHandler(
                loggerMock.Object,
                mediatorMock.Object,
                kernelMock.Object,
                queueMock.Object
            );
            await handler.Handle(
                new RunActorBehaviorTick(
                    new ActorBehaviorTick()
                ),
                CancellationToken.None
            );

            // Then
            queueMock.Verify(
                mock => mock.RegisterFailed(
                    It.IsAny<ActorBehaviorTick>()
                )
            );
        }
    }
}