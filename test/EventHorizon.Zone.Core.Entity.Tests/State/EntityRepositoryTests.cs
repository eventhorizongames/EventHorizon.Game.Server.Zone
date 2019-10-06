using Xunit;
using Moq;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.TestUtils;
using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Events.Entity.Action;
using EventHorizon.Tests.TestUtils;

namespace EventHorizon.Zone.Core.Entity.Tests.State
{
    [CleanupInMemoryEntityRepository]
    public class EntityRepositoryTests
    {
        [Fact]
        public async Task TestShouldReturnAllEntitiesAddedToRepository()
        {
            // Given
            var expectedEntityId1 = 1L;
            var expectedEntityId2 = 2L;
            var inputEntity1 = new TestObjectEntity();
            var inputEntity2 = new TestObjectEntity();

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IdPool>();

            idPoolMock.SetupSequence(
                mock => mock.NextId()
            ).Returns(
                expectedEntityId1
            ).Returns(
                expectedEntityId2
            );

            // When
            var entityRepository = new InMemoryEntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            await entityRepository.Add(
                inputEntity1
            );
            await entityRepository.Add(
                inputEntity2
            );

            var actual = await entityRepository.All();

            // Then
            Assert.Collection(
                actual,
                entity => Assert.Equal(expectedEntityId1, entity.Id),
                entity => Assert.Equal(expectedEntityId2, entity.Id)
            );
        }
        [Fact]
        public async Task TestShouldReturnThePredicatedResultListWhenCalled()
        {
            // Given
            var expectedEntityId1 = 1020L;
            var entityId2 = 12300L;
            var inputEntity1 = new TestObjectEntity();
            var inputEntity2 = new TestObjectEntity();

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IdPool>();

            idPoolMock.SetupSequence(
                mock => mock.NextId()
            ).Returns(
                expectedEntityId1
            ).Returns(
                entityId2
            );

            // When
            var entityRepository = new InMemoryEntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            await entityRepository.Add(
                inputEntity1
            );
            await entityRepository.Add(
                inputEntity2
            );

            var actual = await entityRepository.Where(
                entity => entity.Id == 1020L
            );

            // Then
            Assert.Collection(
                actual,
                entity => Assert.Equal(expectedEntityId1, entity.Id)
            );
        }
        [Fact]
        public async Task TestShouldReturnEntityMatchingPassedInId()
        {
            // Given
            var expectedEntityId1 = 1L;
            var expectedEntityId2 = 2L;
            var inputEntity1 = new TestObjectEntity();
            var inputEntity2 = new TestObjectEntity();
            var expectedEntity1 = new TestObjectEntity
            {
                Id = expectedEntityId1
            };
            var expectedEntity2 = new TestObjectEntity
            {
                Id = expectedEntityId2
            };

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IdPool>();

            idPoolMock.SetupSequence(
                mock => mock.NextId()
            ).Returns(
                expectedEntityId1
            ).Returns(
                expectedEntityId2
            );

            // When
            var entityRepository = new InMemoryEntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            await entityRepository.Add(
                inputEntity1
            );
            await entityRepository.Add(
                inputEntity2
            );

            // Then
            var actualEntity1 = (TestObjectEntity)await entityRepository.FindById(
                expectedEntityId1
            );
            Assert.Equal(
                expectedEntity1,
                actualEntity1
            );
            var actualEntity2 = (TestObjectEntity)await entityRepository.FindById(
                expectedEntityId2
            );
            Assert.Equal(
                expectedEntity2,
                actualEntity2
            );
        }
        [Fact]
        public async Task TestShouldReturnDefaultEntityWhenNotFound()
        {
            // Given
            var inputId = 123;
            var expectedEntity = default(DefaultEntity);

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IdPool>();

            // When
            var entityRepository = new InMemoryEntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            // Then
            var actualEntity1 = await entityRepository.FindById(
                inputId
            );
            Assert.Equal(
                expectedEntity,
                actualEntity1
            );
        }
        [Fact]
        public async Task TestShouldAddOrUpdateEntityThenPublishEntityActionEvent()
        {
            // Given
            var expectedEntityId = 1L;
            var expectedEntityAction = EntityAction.POSITION;
            var expectedEntity = new TestObjectEntity
            {
                Id = expectedEntityId
            };

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IdPool>();

            idPoolMock.SetupSequence(
                mock => mock.NextId()
            ).Returns(
                expectedEntityId
            );

            // When
            var entityRepository = new InMemoryEntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );
            await entityRepository.Update(
                expectedEntityAction,
                expectedEntity
            );
            var actualEntity = await entityRepository.FindById(
                expectedEntityId
            );

            // Then
            Assert.Equal(
                expectedEntity, actualEntity
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    new EntityActionEvent
                    {
                        Action = expectedEntityAction,
                        Entity = expectedEntity
                    },
                    CancellationToken.None
                )
            );
        }
        [Fact]
        public async Task TestShouldRemoveEntityThenPublishEntityActionEvent()
        {
            // Given
            var expectedEntityId = 1L;
            var expectedEntityAction = EntityAction.REMOVE;
            var expectedEntity = new TestObjectEntity
            {
                Id = expectedEntityId
            };
            var expectedNotFoundEntity = default(DefaultEntity);

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IdPool>();

            idPoolMock.SetupSequence(
                mock => mock.NextId()
            ).Returns(
                expectedEntityId
            );

            // When
            var entityRepository = new InMemoryEntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );
            await entityRepository.Add(
                expectedEntity
            );
            await entityRepository.Remove(
                expectedEntityId
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new EntityActionEvent
                    {
                        Action = expectedEntityAction,
                        Entity = expectedEntity
                    },
                    CancellationToken.None
                )
            );
            var actualEntity = await entityRepository.FindById(
                expectedEntityId
            );
            Assert.Equal(
                expectedNotFoundEntity,
                actualEntity
            );
        }
    }
}