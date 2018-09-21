using Xunit;
using Moq;
using System.Threading.Tasks;
using MediatR;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Entity.State.Impl;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Tests.TestUtil.Models;
using EventHorizon.Game.Server.Zone.Entity.Action;
using System.Threading;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.State.Impl
{
    public class EntityRepositoryTests
    {
        [Fact]
        public async Task TestAll_ShouldReturnAllEntitiesAddedToRepository()
        {
            // Given
            var expectedEntityId1 = 1L;
            var expectedEntityId2 = 2L;
            var inputEntity1 = new TestObjectEntity();
            var inputEntity2 = new TestObjectEntity();

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IIdPool>();

            idPoolMock.SetupSequence(a => a.NextId())
                .Returns(expectedEntityId1)
                .Returns(expectedEntityId2);

            // When
            var entityRepository = new EntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            await entityRepository.Add(inputEntity1);
            await entityRepository.Add(inputEntity2);

            var list = await entityRepository.All();

            // Then
            Assert.Collection(list,
                a => Assert.Equal(expectedEntityId1, a.Id),
                a => Assert.Equal(expectedEntityId2, a.Id)
            );
        }
        [Fact]
        public async Task TestFindById_ShouldReturnEntityMatchingPassedInId()
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
            var idPoolMock = new Mock<IIdPool>();

            idPoolMock.SetupSequence(a => a.NextId())
                .Returns(expectedEntityId1)
                .Returns(expectedEntityId2);

            // When
            var entityRepository = new EntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            await entityRepository.Add(inputEntity1);
            await entityRepository.Add(inputEntity2);

            // Then
            var actualEntity1 = (TestObjectEntity)await entityRepository.FindById(expectedEntityId1);
            Assert.Equal(expectedEntity1, actualEntity1);
            var actualEntity2 = (TestObjectEntity)await entityRepository.FindById(expectedEntityId2);
            Assert.Equal(expectedEntity2, actualEntity2);
        }
        [Fact]
        public async Task TestFindById_ShouldReturnDefaultEntityWhenNotFound()
        {
            // Given
            var inputId = 123;
            var expectedEntity = default(DefaultEntity);

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IIdPool>();

            // When
            var entityRepository = new EntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            // Then
            var actualEntity1 = await entityRepository.FindById(inputId);
            Assert.Equal(expectedEntity, actualEntity1);
        }
        [Fact]
        public async Task TestUpdate_ShouldAddOrUpdateEntityThenPublishEntityActionEvent()
        {
            // Given
            var expectedEntityId = 1L;
            var expectedEntityAction = EntityAction.POSITION;
            var expectedEntity = new TestObjectEntity
            {
                Id = expectedEntityId
            };

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IIdPool>();

            idPoolMock.SetupSequence(a => a.NextId())
                .Returns(expectedEntityId);

            // When
            var entityRepository = new EntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            // Then
            await entityRepository.Update(expectedEntityAction, expectedEntity);
            var actualEntity = await entityRepository.FindById(expectedEntityId);
            Assert.Equal(expectedEntity, actualEntity);
            mediatorMock.Verify(a => a.Publish(new EntityActionEvent
            {
                Action = expectedEntityAction,
                Entity = expectedEntity
            }, CancellationToken.None));
        }
        [Fact]
        public async Task TestRemove_ShouldRemoveEntityThenPublishEntityActionEvent()
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
            var idPoolMock = new Mock<IIdPool>();

            idPoolMock.SetupSequence(a => a.NextId())
                .Returns(expectedEntityId);

            // When
            var entityRepository = new EntityRepository(
                mediatorMock.Object,
                idPoolMock.Object
            );

            // Then
            await entityRepository.Add(expectedEntity);
            await entityRepository.Remove(expectedEntityId);
            mediatorMock.Verify(a => a.Publish(new EntityActionEvent
            {
                Action = expectedEntityAction,
                Entity = expectedEntity
            }, CancellationToken.None));
            var actualEntity = await entityRepository.FindById(expectedEntityId);
            Assert.Equal(expectedNotFoundEntity, actualEntity);
        }
    }
}