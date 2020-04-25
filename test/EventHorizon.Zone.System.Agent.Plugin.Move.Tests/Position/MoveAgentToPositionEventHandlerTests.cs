namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Position
{
    using EventHorizon.Performance;
    using EventHorizon.Zone.Core.Events.Path;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Move;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
    using EventHorizon.Zone.System.Agent.Plugin.Move.Position;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class MoveAgentToPositionEventHandlerTests
    {
        [Fact]
        public async Task ShouldIgnoreRequestWhenAgentIsNull()
        {
            //Given
            var mediatorMock = new Mock<IMediator>();
            var performanceTrackerMock = new Mock<IPerformanceTracker>();

            //When
            var handler = new MoveAgentToPositionEventHandler(
                mediatorMock.Object,
                performanceTrackerMock.Object
            );

            await handler.Handle(
                new MoveAgentToPositionEvent(
                    0L,
                    Vector3.Zero
                ),
                CancellationToken.None
            );

            //Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<FindPathEvent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldQueueAgentToMoveWithFoundPathAndAgent()
        {
            //Given
            var expectedAgentId = 1L;
            var agentPosition = new Vector3(2, 2, 2);
            var agent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = expectedAgentId,
                Transform = new TransformState
                {
                    Position = agentPosition
                }
            };
            var expectedPath = new Queue<Vector3>();
            expectedPath.Enqueue(
                new Vector3(3, 3, 3)
            );
            expectedPath.Enqueue(
                new Vector3(4, 4, 4)
            );
            var expectedMoveToPosition = new Vector3(1, 1, 1);

            var mediatorMock = new Mock<IMediator>();
            var performanceTrackerMock = new Mock<IPerformanceTracker>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetAgentEvent(
                        expectedAgentId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agent
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindPathEvent(
                        agentPosition,
                        expectedMoveToPosition
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedPath
            );

            //When
            var handler = new MoveAgentToPositionEventHandler(
                mediatorMock.Object,
                performanceTrackerMock.Object
            );

            await handler.Handle(
                new MoveAgentToPositionEvent(
                    expectedAgentId,
                    expectedMoveToPosition
                ),
                CancellationToken.None
            );

            //Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new QueueAgentToMove(
                        expectedAgentId,
                        expectedPath,
                        expectedMoveToPosition
                    ),
                    It.IsAny<CancellationToken>()
                )
            );
        }
    }
}