using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Entity.Search.Handler;
using EventHorizon.Game.Server.Zone.Entity.Action;
using System.Threading;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Core.Model;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Search.Handler
{
    public class EntityPositionChangedHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldCallUpdateOnSearchTreeWhenEntityActionIsPosition()
        {
            // Given
            var inputEntityAction = EntityAction.POSITION;
            var expectedEntityId = 123;
            var expectedCurrentPosition = new Vector3(20);
            var expectedTagList = new List<string>();
            var expectedEntityMock = new Mock<IObjectEntity>();
            expectedEntityMock.Setup(a => a.Id).Returns(expectedEntityId);
            expectedEntityMock.Setup(a => a.Position).Returns(new PositionState
            {
                CurrentPosition = expectedCurrentPosition
            });
            expectedEntityMock.Setup(a => a.TagList).Returns(expectedTagList);

            var entitySearchTreeMock = new Mock<IEntitySearchTree>();

            // When
            var entityPositionChangedHandler = new EntityPositionChangedHandler(
                entitySearchTreeMock.Object
            );

            await entityPositionChangedHandler.Handle(new EntityActionEvent
            {
                Action = inputEntityAction,
                Entity = expectedEntityMock.Object
            }, CancellationToken.None);

            // Then
            entitySearchTreeMock.Verify(a => a.Update(new SearchEntity(expectedEntityId, expectedCurrentPosition, expectedTagList)));
        }
        [Fact]
        public async Task TestHandle_ShouldNotCallUpdateOnSearchTreeWhenEntityActionIsNotPosition()
        {
            // Given
            var inputEntityAction = EntityAction.ADD;

            var entitySearchTreeMock = new Mock<IEntitySearchTree>();

            // When
            var entityPositionChangedHandler = new EntityPositionChangedHandler(
                entitySearchTreeMock.Object
            );

            await entityPositionChangedHandler.Handle(new EntityActionEvent
            {
                Action = inputEntityAction,
                Entity = null
            }, CancellationToken.None);

            // Then
            entitySearchTreeMock.Verify(a => a.Update(It.IsAny<SearchEntity>()), Times.Never());
        }
    }
}