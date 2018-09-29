using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Player.State.Impl.Testing;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.External.Entity;

namespace EventHorizon.Game.Server.Zone.Tests.Player.State.Impl.Testing
{
    public class PlayerTestingRepositoryTests
    {
        [Fact]
        public async Task TestFindById_ShouldReturnEntityFromRepositoryWhenFound()
        {
            // Given
            var inputPlayerId = "123";
            var expected = new PlayerEntity()
            {
                PlayerId = inputPlayerId,
                Type = EntityType.PLAYER,
            };
            var entityList = new List<IObjectEntity>()
            {
                expected,
            };

            var entityRepositoryMock = new Mock<IEntityRepository>();
            entityRepositoryMock.Setup(a => a.All()).ReturnsAsync(entityList);

            // When
            var playerTestingRepository = new PlayerTestingRepository(
                entityRepositoryMock.Object
            );

            var actual = await playerTestingRepository.FindById(inputPlayerId);

            // Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestFindById_WhenPlayerIsNullShouldReturnNewEntityAndAddToRepository()
        {
            // Given
            var inputPlayerId = "123";
            var expected = new PlayerEntity()
            {
                PlayerId = inputPlayerId,
                Type = EntityType.PLAYER,
            };
            var entityList = new List<IObjectEntity>();

            var entityRepositoryMock = new Mock<IEntityRepository>();
            entityRepositoryMock.Setup(a => a.All()).ReturnsAsync(entityList);
            entityRepositoryMock.Setup(a => a.Add(It.IsAny<PlayerEntity>())).ReturnsAsync(expected);

            // When
            var playerTestingRepository = new PlayerTestingRepository(
                entityRepositoryMock.Object
            );

            var actual = await playerTestingRepository.FindById(inputPlayerId);

            // Then
            Assert.Equal(expected, actual);
            entityRepositoryMock.Verify(a => a.Add(It.IsAny<PlayerEntity>()));
        }

        [Fact]
        public async Task TestRemove_ShouldCallRemoveOnEntityRepository()
        {
            // Given
            var expectedId = 123;
            var input = new PlayerEntity()
            {
                Id = expectedId
            };


            var entityRepositoryMock = new Mock<IEntityRepository>();

            // When
            var playerTestingRepository = new PlayerTestingRepository(
                entityRepositoryMock.Object
            );

            await playerTestingRepository.Remove(input);

            // Then
            entityRepositoryMock.Verify(a => a.Remove(expectedId));
        }

        [Fact]
        public async Task TestUpdate_ShouldCallUpdateOnEntityRepository()
        {
            // Given
            var inputId = 123;
            var expectedAction = EntityAction.ADD;
            var expected = new PlayerEntity()
            {
                Id = inputId
            };

            var entityRepositoryMock = new Mock<IEntityRepository>();

            // When
            var playerTestingRepository = new PlayerTestingRepository(
                entityRepositoryMock.Object
            );

            await playerTestingRepository.Update(expectedAction, expected);

            // Then
            entityRepositoryMock.Verify(a => a.Update(expectedAction, expected));
        }
    }
}