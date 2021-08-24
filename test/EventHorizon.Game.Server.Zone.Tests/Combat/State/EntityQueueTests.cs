namespace EventHorizon.Game.Server.Zone.Tests.Combat.State
{
    using System.Threading.Tasks;

    using EventHorizon.Zone.System.Combat.State;

    using Xunit;

    public class EntityQueueTests
    {
        [Fact]
        public async Task TestEnqueueAndDequeueExpectedBehavior()
        {
            // Given
            var testEntity1 = new EntityQueueTestStruct
            {
                Index = 1
            };
            var testEntity2 = new EntityQueueTestStruct
            {
                Index = 2
            };
            var testEntity3 = new EntityQueueTestStruct
            {
                Index = 3
            };
            var testEntity4 = new EntityQueueTestStruct
            {
                Index = 4
            };
            var testEntity5 = new EntityQueueTestStruct
            {
                Index = 5
            };
            var testEntity6 = new EntityQueueTestStruct
            {
                Index = 6
            };
            // When
            var entityQueue = new EntityQueue<EntityQueueTestStruct>();
            await entityQueue.Enqueue(testEntity1);
            await entityQueue.Enqueue(testEntity2);
            await entityQueue.Enqueue(testEntity3);
            await entityQueue.Enqueue(testEntity4);
            await entityQueue.Enqueue(testEntity5);
            await entityQueue.Enqueue(testEntity6);

            // Then
            var actualEntity1 = await entityQueue.Dequeue();
            Assert.Equal(1, actualEntity1.Index);
            var actualEntity2 = await entityQueue.Dequeue();
            Assert.Equal(2, actualEntity2.Index);
            var actualEntity3 = await entityQueue.Dequeue();
            Assert.Equal(3, actualEntity3.Index);
            var actualEntity4 = await entityQueue.Dequeue();
            Assert.Equal(4, actualEntity4.Index);
            var actualEntity5 = await entityQueue.Dequeue();
            Assert.Equal(5, actualEntity5.Index);
            var actualEntity6 = await entityQueue.Dequeue();
            Assert.Equal(6, actualEntity6.Index);
            var actualEntity7 = await entityQueue.Dequeue();
            Assert.Equal(default, actualEntity7);
        }

        public struct EntityQueueTestStruct
        {
            public int Index { get; set; }
        }
    }
}
