using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using System.Threading;
using System.Numerics;
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Entity.Search;
using EventHorizon.Zone.Core.Events.Entity.Action;

namespace EventHorizon.Zone.Core.Entity.Tests.Search
{
    public class EntityPositionChangedHandlerTests
    {
        [Fact]
        public async Task TestShouldCallAddOnSearchTreeWhenEntityActionIsPosition()
        {
            // Given
            var inputEntityAction = EntityAction.POSITION;
            var expectedEntityId = 123;
            var expectedPosition = new Vector3(20);
            var expectedTagList = new List<string>();

            var inputEntity = new DefaultEntity()
            {
                Id = expectedEntityId,
                Transform = new TransformState
                {
                    Position = expectedPosition
                },
                TagList = expectedTagList
            };
            var entitySearchTreeMock = new Mock<EntitySearchTree>();

            // When
            var entityPositionChangedHandler = new EntityPositionChangedHandler(
                entitySearchTreeMock.Object
            );

            await entityPositionChangedHandler.Handle(
                new EntityActionEvent
                {
                    Action = inputEntityAction,
                    Entity = inputEntity
                },
                CancellationToken.None
            );

            // Then
            entitySearchTreeMock.Verify(
                mock => mock.Add(
                    new SearchEntity(
                        expectedEntityId,
                        expectedPosition,
                        expectedTagList
                    )
                )
            );
        }
        [Fact]
        public async Task TestShouldNotCallAddOnSearchTreeWhenEntityActionIsNotPosition()
        {
            // Given
            var inputEntityAction = EntityAction.ADD;

            var entitySearchTreeMock = new Mock<EntitySearchTree>();

            // When
            var entityPositionChangedHandler = new EntityPositionChangedHandler(
                entitySearchTreeMock.Object
            );

            await entityPositionChangedHandler.Handle(
                new EntityActionEvent
                {
                    Action = inputEntityAction,
                    Entity = null
                },
                CancellationToken.None
            );

            // Then
            entitySearchTreeMock.Verify(
                mock => mock.Add(
                    It.IsAny<SearchEntity>()
                ),
                Times.Never()
            );
        }
    }
}