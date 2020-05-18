namespace EventHorizon.Game.Tests.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EventHorizon.Game.State;
    using FluentAssertions;
    using Xunit;
    
    public class CurrentGameStateTests
    {
        [Fact]
        public void ShouldNotFailWhenPlayerIsNotFoundInScoresList()
        {
            // Given
            var playerEntityId = 123L;
            var scores = new List<GamePlayerScore>()
            {
                new GamePlayerScore(
                    1L,
                    1
                ),
                new GamePlayerScore(
                    2L,
                    1024
                ),
            };

            // When
            var actual = new CurrentGameState(
                scores
            ).IncrementPlayer(
                playerEntityId
            );

            // Then
            actual.Scores
                .Should().HaveCount(3);
        }
    }
}