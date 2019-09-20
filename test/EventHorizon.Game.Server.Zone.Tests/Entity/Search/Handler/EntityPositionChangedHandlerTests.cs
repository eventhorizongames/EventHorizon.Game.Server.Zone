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
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Core;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Search.Handler
{
    public class EntityPositionChangedHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldCallAddOnSearchTreeWhenEntityActionIsPosition()
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
            entitySearchTreeMock.Verify(a => a.Add(new SearchEntity(expectedEntityId, expectedCurrentPosition, expectedTagList)));
        }
        [Fact]
        public async Task TestHandle_ShouldNotCallAddOnSearchTreeWhenEntityActionIsNotPosition()
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
            entitySearchTreeMock.Verify(a => a.Add(It.IsAny<SearchEntity>()), Times.Never());
        }
    }
}