namespace EventHorizon.Game.Tests.Model.Client
{
    using System.Collections.Generic;

    using EventHorizon.Game.Model.Client;
    using EventHorizon.Game.State;

    using FluentAssertions;

    using Xunit;

    public class GameStateChangedDataTests
    {
        [Fact]
        public void ShouldReturnExpectedCurrentGameStateWhenCreated()
        {
            // Given
            var gameState = new CurrentGameState(
                new List<GamePlayerScore>()
            );
            var expected = gameState;

            // When
            var actual = new GameStateChangedData
            {
                GameState = gameState,
            };

            // Then
            actual.GameState
                .Should().Be(expected);
        }
    }
}
