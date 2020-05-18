namespace EventHorizon.Game.Tests
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Query;
    using EventHorizon.Game.State;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class QueryForCurrentGameStateHandlerTests
    {

        [Fact]
        public async Task ShouldReturnCurrentGameStateWhenRequestIsHandled()
        {
            // Given
            var expected = new CurrentGameState(
                new List<GamePlayerScore>()
            );

            var gameStateMock = new Mock<GameState>();

            gameStateMock.Setup(
                mock => mock.CurrentGameState
            ).Returns(
                expected
            );

            // When
            var handler = new QueryForCurrentGameStateHandler(
                gameStateMock.Object
            );
            var actual = await handler.Handle(
                new QueryForCurrentGameState(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
