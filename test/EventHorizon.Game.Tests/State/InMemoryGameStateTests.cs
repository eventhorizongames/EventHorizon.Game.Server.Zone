namespace EventHorizon.Game.Tests.State
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EventHorizon.Game.Client;
    using EventHorizon.Game.State;
    using FluentAssertions;
    using Xunit;

    public class InMemoryGameStateTests
    {
        [Fact]
        public void ShouldUpdateCurrentGameStateWhenIncrementPlayerIsCalled()
        {
            // Given
            var playerEntityId = 123L;
            var playerScore = 1;
            var scores = new List<GamePlayerScore>
            {
                new GamePlayerScore(
                    playerEntityId,
                    playerScore
                )
            };
            var expected = scores;

            // When
            var gameState = new InMemoryGameState();
            gameState.IncrementPlayer(
                playerEntityId
            );
            var actual = gameState.CurrentGameState;

            // Then
            actual.Scores
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldRemovePlayerFromCurrentGameStateWhenRemovePlayerIsCalled()
        {
            // Given
            var playerEntityId = 123L;

            // When
            var gameState = new InMemoryGameState();
            gameState.IncrementPlayer(
                playerEntityId
            );
            gameState.CurrentGameState
                .Scores
                .Should().NotBeEmpty();
            gameState.RemovePlayer(
                playerEntityId
            );
            var actual = gameState.CurrentGameState;

            // Then
            actual.Scores.Should().BeEmpty();
        }
    }
}
