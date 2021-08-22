namespace EventHorizon.Game.Server.Zone.Tests.Player.Move.Direction
{
    using System.Collections.Generic;

    using EventHorizon.Game.Server.Zone.Player.Action.Direction;
    using EventHorizon.Zone.Core.Model.Player;

    using Xunit;

    public class MovePlayerEventTests
    {
        [Fact]
        public void TestShouldBeANewReferenceWhenSetDataIsCalled()
        {
            // Given
            var startingImplementation = new MovePlayerEvent();

            // When
            var movePlayerEvent = startingImplementation;
            var actual = movePlayerEvent
                .SetData(
                    new Dictionary<string, object>()
                    {
                        { "moveDirection", 1L }
                    }
                );

            // Then
            Assert.NotEqual(
                startingImplementation,
                actual
            );
        }

        [Fact]
        public void TestShouldSetMoveDirectionToDefaultWhenNotFoundInData()
        {
            // Given
            var expected = -1;

            // When
            var actual = (MovePlayerEvent)new MovePlayerEvent()
                .SetData(
                    new Dictionary<string, object>()
                );

            // Then
            Assert.Equal(
                expected,
                actual.MoveDirection
            );
        }

        [Fact]
        public void TestShouldBeANewReferenceWhenSetPlayerIsCalled()
        {
            // Given
            var startingImplementation = new MovePlayerEvent();

            // When
            var movePlayerEvent = startingImplementation;
            var actual = movePlayerEvent
                .SetPlayer(
                    new PlayerEntity
                    {
                        PlayerId = "player-id"
                    }
                );

            // Then
            Assert.NotEqual(
                startingImplementation,
                actual
            );
        }
    }
}
