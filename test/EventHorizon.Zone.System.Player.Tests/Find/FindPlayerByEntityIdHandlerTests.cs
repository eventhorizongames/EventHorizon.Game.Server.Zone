namespace EventHorizon.Zone.System.Player.Tests.Find
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Events.Find;
    using EventHorizon.Zone.System.Player.Find;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class FindPlayerByEntityIdHandlerTests
    {
        [Fact]
        public async Task ShouldReturnDefaultWhenPlayerIsNotFound()
        {
            // Given
            var playerEntityId = 123L;
            var expected = default(PlayerEntity);

            var repositoryMock = new Mock<EntityRepository>();

            // When
            var handler = new FindPlayerByEntityIdHandler(
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new FindPlayerByEntityId(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnDefaultWhenPlayerEntityDoesNotMatch()
        {
            // Given
            var playerEntityId = 123L;
            var expected = default(PlayerEntity);
            var entityInList = new PlayerEntity
            {
                Id = 321L,
                Type = EntityType.PLAYER,
            };

            var repositoryMock = new Mock<EntityRepository>();

            repositoryMock.Setup(
                mock => mock.Where(
                    It.IsAny<Func<IObjectEntity, bool>>()
                )
            ).Returns((Func<IObjectEntity, bool> query) =>
            {
                return new List<IObjectEntity>
                {
                    expected,
                }.Where(
                    query
                ).FromResult();
            });

            // When
            var handler = new FindPlayerByEntityIdHandler(
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new FindPlayerByEntityId(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnDefaultWhenEntityIsNotTypeOfPlayer()
        {
            // Given
            var playerEntityId = 123L;
            var expected = default(PlayerEntity);
            var entityInList = new PlayerEntity
            {
                Id = playerEntityId,
                Type = EntityType.OTHER,
            };

            var repositoryMock = new Mock<EntityRepository>();

            repositoryMock.Setup(
                mock => mock.Where(
                    It.IsAny<Func<IObjectEntity, bool>>()
                )
            ).Returns((Func<IObjectEntity, bool> query) =>
            {
                return new List<IObjectEntity>
                {
                    expected,
                }.Where(
                    query
                ).FromResult();
            });

            // When
            var handler = new FindPlayerByEntityIdHandler(
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new FindPlayerByEntityId(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnEntityWhenPlayerTypeAndEntityIdMatch()
        {
            // Given
            var playerEntityId = 123L;
            var expected = new PlayerEntity()
            {
                Id = playerEntityId,
                Type = EntityType.PLAYER,
            };

            var repositoryMock = new Mock<EntityRepository>();

            repositoryMock.Setup(
                mock => mock.Where(
                    It.IsAny<Func<IObjectEntity, bool>>()
                )
            ).Returns((Func<IObjectEntity, bool>  query) =>
            {
                return new List<IObjectEntity>
                {
                    expected,
                }.Where(
                    query
                ).FromResult();
            });

            // When
            var handler = new FindPlayerByEntityIdHandler(
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new FindPlayerByEntityId(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
