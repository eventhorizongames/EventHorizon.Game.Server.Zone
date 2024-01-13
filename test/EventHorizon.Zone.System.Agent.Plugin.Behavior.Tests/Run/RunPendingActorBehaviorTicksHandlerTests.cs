namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Run;

using EventHorizon.Performance;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class RunPendingActorBehaviorTicksHandlerTests
{
    private delegate void ActorBehaviorTickSetter(out ActorBehaviorTick actorBehaviorTick);

    [Fact]
    public async Task ShouldPublishRunActorBehaviorTickForEachActorBehaviorTickInQueue()
    {
        // Given
        var actorBehaviorTick = new ActorBehaviorTick();
        var inputId1 = new ActorBehaviorTick(
            "shape-1",
            1L
        );
        var inputId2 = new ActorBehaviorTick(
            "shape-2",
            2L
        );
        var inputId3 = new ActorBehaviorTick(
            "shape-3",
            3L
        );
        var expectedMoveRegisteredAgentEvent1 = new RunActorBehaviorTick(
            inputId1
        );
        var expectedMoveRegisteredAgentEvent2 = new RunActorBehaviorTick(
            inputId2
        );
        var expectedMoveRegisteredAgentEvent3 = new RunActorBehaviorTick(
            inputId3
        );

        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<RunPendingActorBehaviorTicksHandler>>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();
        var performanceTrackerFactoryMock = new Mock<PerformanceTrackerFactory>();

        var called = 0;
        queueMock.Setup(
            mock => mock.Dequeue(
                out actorBehaviorTick
            )
        ).Callback(
            new ActorBehaviorTickSetter((out ActorBehaviorTick tick) =>
            {
                if (called == 0)
                    tick = inputId1;
                else if (called == 1)
                    tick = inputId2;
                else if (called == 2)
                    tick = inputId3;
                else
                    tick = default;
                called++;
            })
        ).Returns(
            () => called <= 3
        );

        // When
        var handler = new RunPendingActorBehaviorTicksHandler(
            loggerMock.Object,
            mediatorMock.Object,
            queueMock.Object,
            performanceTrackerFactoryMock.Object
        );
        await handler.Handle(
            new RunPendingActorBehaviorTicks(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mediator => mediator.Send(
                It.IsAny<RunActorBehaviorTick>(),
                CancellationToken.None
            ),
            Times.Exactly(
                3
            )
        );
        mediatorMock.Verify(
            mediator => mediator.Send(
                expectedMoveRegisteredAgentEvent1,
                CancellationToken.None
            )
        );
        mediatorMock.Verify(
            mediator => mediator.Send(
                expectedMoveRegisteredAgentEvent2,
                CancellationToken.None
            )
        );
        mediatorMock.Verify(
            mediator => mediator.Send(
                expectedMoveRegisteredAgentEvent3,
                CancellationToken.None
            )
        );
    }

    [Fact]
    public async Task ShouldNotSendRunActorBehaviorTickWhenNothingIsDequeued()
    {
        // Given
        var actorBehaviorTick = new ActorBehaviorTick();

        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<RunPendingActorBehaviorTicksHandler>>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();
        var performanceTrackerFactoryMock = new Mock<PerformanceTrackerFactory>();

        queueMock.Setup(
            mock => mock.Dequeue(
                out actorBehaviorTick
            )
        ).Returns(
            false
        );

        // When
        var handler = new RunPendingActorBehaviorTicksHandler(
            loggerMock.Object,
            mediatorMock.Object,
            queueMock.Object,
            performanceTrackerFactoryMock.Object
        );
        await handler.Handle(
            new RunPendingActorBehaviorTicks(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mediator => mediator.Publish(
                It.IsAny<RunActorBehaviorTick>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }

    [Fact]
    public async Task ShouldNotBubbleExceptionsWhenRunActorBehaviorTickThrownsAnyException()
    {
        // Given
        var expectedCallCount = 15;

        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<RunPendingActorBehaviorTicksHandler>>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();
        var performanceTrackerFactoryMock = new Mock<PerformanceTrackerFactory>();

        var actorBehaviorTick = new ActorBehaviorTick();
        var called = 0;
        queueMock.Setup(
            mock => mock.Dequeue(
                out actorBehaviorTick
            )
        ).Callback(
            new ActorBehaviorTickSetter((out ActorBehaviorTick tick) =>
            {
                if (called <= expectedCallCount)
                    tick = actorBehaviorTick;
                else
                    tick = default;
                called++;
            })
        ).Returns(
            () => called <= expectedCallCount
        );

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunActorBehaviorTick>(),
                CancellationToken.None
            )
        ).ThrowsAsync(
            new Exception(
                "error"
            )
        );

        // When
        var handler = new RunPendingActorBehaviorTicksHandler(
            loggerMock.Object,
            mediatorMock.Object,
            queueMock.Object,
            performanceTrackerFactoryMock.Object
        );
        await handler.Handle(
            new RunPendingActorBehaviorTicks(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mediator => mediator.Send(
                It.IsAny<RunActorBehaviorTick>(),
                CancellationToken.None
            ),
            Times.Exactly(
                expectedCallCount
            )
        );
    }

    [Fact]
    public async Task ShouldNotStopProcessingWhenQueueCountIsOver75()
    {
        var expectedCallCount = 75;

        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<RunPendingActorBehaviorTicksHandler>>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();
        var performanceTrackerFactoryMock = new Mock<PerformanceTrackerFactory>();

        var actorBehaviorTick = new ActorBehaviorTick();
        var called = 0;
        queueMock.Setup(
            mock => mock.Dequeue(
                out actorBehaviorTick
            )
        ).Callback(
            new ActorBehaviorTickSetter((out ActorBehaviorTick tick) =>
            {
                if (called <= expectedCallCount)
                    tick = actorBehaviorTick;
                else
                    tick = default;
                called++;
            })
        ).Returns(
            () => called <= expectedCallCount
        );

        // When
        var handler = new RunPendingActorBehaviorTicksHandler(
            loggerMock.Object,
            mediatorMock.Object,
            queueMock.Object,
            performanceTrackerFactoryMock.Object
        );
        await handler.Handle(
            new RunPendingActorBehaviorTicks(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mediator => mediator.Send(
                It.IsAny<RunActorBehaviorTick>(),
                CancellationToken.None
            ),
            Times.Exactly(
                expectedCallCount
            )
        );
    }
}
