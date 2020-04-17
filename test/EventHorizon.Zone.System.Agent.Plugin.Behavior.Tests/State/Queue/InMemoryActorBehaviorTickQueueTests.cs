namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.State.Queue
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
    using Xunit;

    public class InMemoryActorBehaviorTickQueueTests
    {
        [Fact]
        public void ShouldReturnRegsiteredItemWhenQueueIsPrimed()
        {
            // Given
            var shapeId = "shape-id";
            var actorId = 1L;
            var expected = new ActorBehaviorTick(
                shapeId,
                actorId
            );

            // When
            var queue = new InMemoryActorBehaviorTickQueue();
            queue.Register(
                shapeId,
                actorId
            );
            queue.PrimeQueueWithRegisteredTicks();

            queue.Dequeue(
                out var actual
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void ShouldNotReturnExpectedItemWhenQueueIsNotPrimed()
        {
            // Given
            var expectedActorId = 0L;
            var shapeId = "shape-id";
            var actorId = 1L;

            // When
            var queue = new InMemoryActorBehaviorTickQueue();
            queue.Register(
                shapeId,
                actorId
            );

            queue.Dequeue(
                out var actual
            );

            // Then
            Assert.Null(
                actual.ShapeId
            );
            Assert.Equal(
                expectedActorId,
                actual.ActorId
            );
        }

        [Fact]
        public void ShouldIncrementFailedCountOneWhenRegisterFailedIsUsed()
        {
            // Given
            var expected = 1;
            var shapeId = "shape-id";
            var actorId = 1L;

            // When
            var queue = new InMemoryActorBehaviorTickQueue();
            queue.RegisterFailed(
                new ActorBehaviorTick(
                    shapeId,
                    actorId
                )
            );
            queue.PrimeQueueWithRegisteredTicks();

            queue.Dequeue(
                out var actual
            );

            // Then
            Assert.Equal(
                expected,
                actual.FailedCount
            );
        }
    }
}