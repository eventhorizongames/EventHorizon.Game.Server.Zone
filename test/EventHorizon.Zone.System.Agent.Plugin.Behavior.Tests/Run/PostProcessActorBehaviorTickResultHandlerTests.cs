namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Run;

using global::System;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
using global::System.Collections.Concurrent;
using global::System.Threading;
using global::System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class PostProcessActorBehaviorTickResultHandlerTests
{
    [Fact]
    public async Task ShouldRegisterShapeAndActorToQueueWhenResultIsValid()
    {
        // Given
        var now = DateTime.MinValue.AddMilliseconds(400);
        var expectedNextTickRequest = now.AddMilliseconds(100);
        var shapeId = "shape";
        var shape = new ActorBehaviorTreeShape(
            shapeId,
            new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode() }
        );
        var actorId = 1L;
        var actor = new DefaultEntity(new ConcurrentDictionary<string, object>())
        {
            Id = actorId
        }.SetProperty(AgentBehavior.PROPERTY_NAME, new AgentBehavior { NextTickRequest = now, });
        var result = new BehaviorTreeState(shape);
        var actorBehaviorTick = new ActorBehaviorTick(shapeId, actorId);

        var loggerMock = new Mock<ILogger<PostProcessActorBehaviorTickResultHandler>>();
        var dateTimeMock = new Mock<IDateTimeService>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();

        dateTimeMock.Setup(mock => mock.Now).Returns(now);

        // When
        var handler = new PostProcessActorBehaviorTickResultHandler(
            loggerMock.Object,
            dateTimeMock.Object,
            queueMock.Object
        );
        await handler.Handle(
            new PostProcessActorBehaviorTickResult(result, actorBehaviorTick, actor, shape),
            CancellationToken.None
        );

        // Then
        Assert.True(actor.ContainsProperty(BehaviorTreeState.PROPERTY_NAME));
        queueMock.Verify(mock => mock.Register(shapeId, actorId));
    }

    [Fact]
    public async Task ShouldRegisterFailedActorBehaviorTickToQueueWhenResultIsNotValid()
    {
        // Given
        var now = DateTime.MinValue.AddMilliseconds(400);
        var expectedNextTickRequest = now.AddMilliseconds(100);
        var shapeId = "shape";
        var shape = new ActorBehaviorTreeShape(
            shapeId,
            new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode() }
        );
        var actorId = 1L;
        var actor = new DefaultEntity(new ConcurrentDictionary<string, object>());
        var result = new BehaviorTreeState(default);
        var actorBehaviorTick = new ActorBehaviorTick(shapeId, actorId);

        var loggerMock = new Mock<ILogger<PostProcessActorBehaviorTickResultHandler>>();
        var dateTimeMock = new Mock<IDateTimeService>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();

        dateTimeMock.Setup(mock => mock.Now).Returns(now);

        // When
        var handler = new PostProcessActorBehaviorTickResultHandler(
            loggerMock.Object,
            dateTimeMock.Object,
            queueMock.Object
        );
        await handler.Handle(
            new PostProcessActorBehaviorTickResult(result, actorBehaviorTick, actor, shape),
            CancellationToken.None
        );

        // Then
        queueMock.Verify(mock => mock.RegisterFailed(actorBehaviorTick));
    }

    [Fact]
    public async Task ShouldNotUpdateBehavoirTreeStateToResultOnActorWhenActorTreeStateIsValidOnActorAndDoesNotMatchShapeId()
    {
        // Given
        var now = DateTime.MinValue.AddMilliseconds(400);
        var expectedNextTickRequest = now.AddMilliseconds(100);
        var shapeId = "shape";
        var shape = new ActorBehaviorTreeShape(
            shapeId,
            new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode() }
        );
        var actorShapeId = "actor-shape-id";
        var actorBehaviorTreeShape = new ActorBehaviorTreeShape(
            actorShapeId,
            new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode(), }
        );
        var actorId = 1L;
        var actorBehaviorTreeState = new BehaviorTreeState(actorBehaviorTreeShape);
        var actor = new DefaultEntity(new ConcurrentDictionary<string, object>()) { Id = actorId }
            .SetProperty(AgentBehavior.PROPERTY_NAME, new AgentBehavior { NextTickRequest = now, })
            .SetProperty(BehaviorTreeState.PROPERTY_NAME, actorBehaviorTreeState);
        var result = new BehaviorTreeState(shape);
        var actorBehaviorTick = new ActorBehaviorTick(shapeId, actorId);
        var expected = actorBehaviorTreeState;

        var loggerMock = new Mock<ILogger<PostProcessActorBehaviorTickResultHandler>>();
        var dateTimeMock = new Mock<IDateTimeService>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();

        dateTimeMock.Setup(mock => mock.Now).Returns(now);

        // When
        var handler = new PostProcessActorBehaviorTickResultHandler(
            loggerMock.Object,
            dateTimeMock.Object,
            queueMock.Object
        );
        await handler.Handle(
            new PostProcessActorBehaviorTickResult(result, actorBehaviorTick, actor, shape),
            CancellationToken.None
        );

        // Then
        Assert.Equal(
            expected,
            actor.GetProperty<BehaviorTreeState>(BehaviorTreeState.PROPERTY_NAME)
        );
    }

    [Fact]
    public async Task ShouldNotUpdateBehavoirTreeStateToResultOnActorWhenActorTreeStateIsValidOnActorAndBehaviorTreeShapeIsInvalid()
    {
        // Given
        var now = DateTime.MinValue.AddMilliseconds(400);
        var expectedNextTickRequest = now.AddMilliseconds(100);
        var shapeId = "shape";
        var shape = new ActorBehaviorTreeShape();
        var actorShapeId = "actor-shape-id";
        var actorBehaviorTreeShape = new ActorBehaviorTreeShape(
            actorShapeId,
            new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode(), }
        );
        var actorId = 1L;
        var actorBehaviorTreeState = new BehaviorTreeState(actorBehaviorTreeShape);
        var actor = new DefaultEntity(new ConcurrentDictionary<string, object>()) { Id = actorId }
            .SetProperty(AgentBehavior.PROPERTY_NAME, new AgentBehavior { NextTickRequest = now, })
            .SetProperty(BehaviorTreeState.PROPERTY_NAME, actorBehaviorTreeState);
        var result = new BehaviorTreeState(shape);
        var actorBehaviorTick = new ActorBehaviorTick(shapeId, actorId);
        var expected = actorBehaviorTreeState;

        var loggerMock = new Mock<ILogger<PostProcessActorBehaviorTickResultHandler>>();
        var dateTimeMock = new Mock<IDateTimeService>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();

        dateTimeMock.Setup(mock => mock.Now).Returns(now);

        // When
        var handler = new PostProcessActorBehaviorTickResultHandler(
            loggerMock.Object,
            dateTimeMock.Object,
            queueMock.Object
        );
        await handler.Handle(
            new PostProcessActorBehaviorTickResult(result, actorBehaviorTick, actor, shape),
            CancellationToken.None
        );

        // Then
        Assert.Equal(
            expected,
            actor.GetProperty<BehaviorTreeState>(BehaviorTreeState.PROPERTY_NAME)
        );
    }

    [Fact]
    public async Task ShouldNotUpdateBehavoirTreeStateOnActorWhenActorTreeStateIsValidOnActorAndShapeIsNotValid()
    {
        // Given
        var now = DateTime.MinValue.AddMilliseconds(400);
        var expectedNextTickRequest = now.AddMilliseconds(100);
        var shapeId = "shape";
        var shape = new ActorBehaviorTreeShape();
        var actorBehaviorTreeShape = new ActorBehaviorTreeShape(
            shapeId,
            new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode(), }
        );
        var actorId = 1L;
        var actorBehaviorTreeState = new BehaviorTreeState(actorBehaviorTreeShape);
        var actor = new DefaultEntity(new ConcurrentDictionary<string, object>()) { Id = actorId }
            .SetProperty(AgentBehavior.PROPERTY_NAME, new AgentBehavior { NextTickRequest = now, })
            .SetProperty(BehaviorTreeState.PROPERTY_NAME, actorBehaviorTreeState);
        var result = new BehaviorTreeState(
            new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode(), }
            )
        );
        var actorBehaviorTick = new ActorBehaviorTick(shapeId, actorId);
        var expected = actorBehaviorTreeState;

        var loggerMock = new Mock<ILogger<PostProcessActorBehaviorTickResultHandler>>();
        var dateTimeMock = new Mock<IDateTimeService>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();

        dateTimeMock.Setup(mock => mock.Now).Returns(now);

        // When
        var handler = new PostProcessActorBehaviorTickResultHandler(
            loggerMock.Object,
            dateTimeMock.Object,
            queueMock.Object
        );
        await handler.Handle(
            new PostProcessActorBehaviorTickResult(result, actorBehaviorTick, actor, shape),
            CancellationToken.None
        );

        // Then
        Assert.Equal(
            expected,
            actor.GetProperty<BehaviorTreeState>(BehaviorTreeState.PROPERTY_NAME)
        );
    }

    [Fact]
    public async Task ShouldNotUpdateBehavoirTreeStateOnActorWhenActorTreeStateIsValidOnActorAndDoesMatchShapeId()
    {
        // Given
        var now = DateTime.MinValue.AddMilliseconds(400);
        var expectedNextTickRequest = now.AddMilliseconds(100);
        var shapeId = "shape";
        var shape = new ActorBehaviorTreeShape(
            shapeId,
            new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode(), }
        );
        var actorBehaviorTreeShape = new ActorBehaviorTreeShape(
            shapeId,
            new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode(), }
        );
        var actorId = 1L;
        var actorBehaviorTreeState = new BehaviorTreeState(actorBehaviorTreeShape);
        var actor = new DefaultEntity(new ConcurrentDictionary<string, object>()) { Id = actorId }
            .SetProperty(AgentBehavior.PROPERTY_NAME, new AgentBehavior { NextTickRequest = now, })
            .SetProperty(BehaviorTreeState.PROPERTY_NAME, actorBehaviorTreeState);
        var result = new BehaviorTreeState(
            new ActorBehaviorTreeShape(
                shapeId,
                new SerializedAgentBehaviorTree { Root = new SerializedBehaviorNode() }
            )
        );
        var actorBehaviorTick = new ActorBehaviorTick(shapeId, actorId);
        var expected = result;

        var loggerMock = new Mock<ILogger<PostProcessActorBehaviorTickResultHandler>>();
        var dateTimeMock = new Mock<IDateTimeService>();
        var queueMock = new Mock<ActorBehaviorTickQueue>();

        dateTimeMock.Setup(mock => mock.Now).Returns(now);

        // When
        var handler = new PostProcessActorBehaviorTickResultHandler(
            loggerMock.Object,
            dateTimeMock.Object,
            queueMock.Object
        );
        await handler.Handle(
            new PostProcessActorBehaviorTickResult(result, actorBehaviorTick, actor, shape),
            CancellationToken.None
        );

        // Then
        Assert.Equal(
            expected,
            actor.GetProperty<BehaviorTreeState>(BehaviorTreeState.PROPERTY_NAME)
        );
    }
}
