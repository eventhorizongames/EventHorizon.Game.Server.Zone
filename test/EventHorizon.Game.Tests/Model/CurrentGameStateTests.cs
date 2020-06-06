namespace EventHorizon.Game.Tests.Model
{
    using System.Collections.Generic;
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

        [Fact]
        public void ShouldSortScoresWhenCreated()
        {
            // Given
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
                new GamePlayerScore(
                    3L,
                    321
                ),
                new GamePlayerScore(
                    4L,
                    2451
                ),
                new GamePlayerScore(
                    5L,
                    9877
                ),
                new GamePlayerScore(
                    6L,
                    1234
                ),
            };
            var expected = new List<GamePlayerScore>()
            {
                new GamePlayerScore(
                    5L,
                    9877
                ),
                new GamePlayerScore(
                    4L,
                    2451
                ),
                new GamePlayerScore(
                    6L,
                    1234
                ),
                new GamePlayerScore(
                    2L,
                    1024
                ),
                new GamePlayerScore(
                    3L,
                    321
                ),
                new GamePlayerScore(
                    1L,
                    1
                ),
            };

            // When
            var actual = new CurrentGameState(
                scores
            );

            // Then
            actual.Scores
                .Should().ContainInOrder(expected);
        }

        [Fact]
        public void ShouldSortListDescendingWhenPlayerIsIncremented()
        {
            // Given
            var playerEntityId = 1L;
            var scores = new List<GamePlayerScore>()
            {
                new GamePlayerScore(
                    1L,
                    1
                ),
                new GamePlayerScore(
                    6L,
                    3
                ),
                new GamePlayerScore(
                    2L,
                    1
                ),
                new GamePlayerScore(
                    4L,
                    4
                ),
                new GamePlayerScore(
                    3L,
                    5
                ),
                new GamePlayerScore(
                    5L,
                    6
                ),
            };
            var expected = new List<GamePlayerScore>()
            {
                new GamePlayerScore(
                    5L,
                    6
                ),
                new GamePlayerScore(
                    3L,
                    5
                ),
                new GamePlayerScore(
                    4L,
                    4
                ),
                new GamePlayerScore(
                    6L,
                    3
                ),
                new GamePlayerScore(
                    1L,
                    2
                ),
                new GamePlayerScore(
                    2L,
                    1
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
                .Should().ContainInOrder(expected);
        }
    }
}