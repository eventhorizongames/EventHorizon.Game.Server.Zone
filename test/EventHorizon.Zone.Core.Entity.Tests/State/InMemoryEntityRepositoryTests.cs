namespace EventHorizon.Zone.Core.Entity.Tests.State
{
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.State;
    using EventHorizon.Zone.Core.Entity.Tests.Common;
    using EventHorizon.Zone.Core.Events.Entity.Action;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Id;

    using FluentAssertions;

    using MediatR;

    using Moq;

    using Xunit;

    [CleanupInMemoryEntityRepository]
    public class InMemoryEntityRepositoryTests
    {
        [Fact]
        public async Task TestShouldReturnAllEntitiesAddedToRepository()
        {
            // Given
            var expectedEntityId1 = 1L;
            var expectedEntityId2 = 2L;
            var inputEntity1 = new DefaultEntity();
            var inputEntity2 = new DefaultEntity();

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
            var inputEntity1 = new DefaultEntity();
            var inputEntity2 = new DefaultEntity();

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
            var inputEntity1 = new DefaultEntity();
            var inputEntity2 = new DefaultEntity();
            var expectedEntity1 = new DefaultEntity
            {
                Id = expectedEntityId1
            };
            var expectedEntity2 = new DefaultEntity
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
            var actualEntity1 = (DefaultEntity)await entityRepository.FindById(
                expectedEntityId1
            );
            Assert.Equal(
                expectedEntity1,
                actualEntity1
            );
            var actualEntity2 = (DefaultEntity)await entityRepository.FindById(
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
            var expectedEntity = DefaultEntity.NULL;

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
            var expectedEntity = new DefaultEntity
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
                expectedEntity,
                actualEntity
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    new EntityActionEvent(
                        expectedEntityAction,
                        expectedEntity
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldReplaceExistingEntityWhenAlreadyExistsById()
        {
            // Given
            var entityId = 1L;
            var existingEntity = new DefaultEntity
            {
                Id = entityId
            };
            var newEntity = new DefaultEntity
            {
                Id = entityId,
                Transform = new TransformState
                {
                    Position = new Vector3(2, 2, 2),
                },
            };

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IdPool>();

            idPoolMock.Setup(
                mock => mock.NextId()
            ).Returns(
                entityId
            );

            // When
            var entityRepository = new InMemoryEntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );
            await entityRepository.Add(
                existingEntity
            );
            var currentEntity = await entityRepository.FindById(
                entityId
            );
            currentEntity.Should().Be(existingEntity)
                .And.Should().NotBe(newEntity);
            await entityRepository.Update(
                EntityAction.PROPERTY_CHANGED,
                existingEntity
            );
            var actual = await entityRepository.FindById(
                entityId
            );


            // Then
            actual.Should().Be(newEntity)
                .And
                .Should().NotBe(existingEntity);
        }

        [Fact]
        public async Task TestShouldRemoveEntityThenPublishEntityActionEvent()
        {
            // Given
            var expectedEntityId = 1L;
            var expectedEntityAction = EntityAction.REMOVE;
            var expectedEntity = new DefaultEntity
            {
                Id = expectedEntityId
            };
            var expectedNotFoundEntity = DefaultEntity.NULL;

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
                    new EntityActionEvent(
                        expectedEntityAction,
                        expectedEntity
                    ),
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
