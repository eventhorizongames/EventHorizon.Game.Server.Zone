namespace EventHorizon.Game.Tests.Increment
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Increment;
    using EventHorizon.Game.Model.Client;
    using EventHorizon.Game.State;
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using MediatR;
    using Moq;
    using Xunit;

    public class IncrementPlayerScoreHandlerTests
    {
        [Fact]
        public async Task ShouldIncrementlayerOnGameStateWithPlayerEntityId()
        {
            // Given
            var playerEntityId = 123L;

            var mediatorMock = new Mock<IMediator>();
            var gameStateMock = new Mock<GameState>();

            // When
            var handler = new IncrementPlayerScoreHandler(
                mediatorMock.Object,
                gameStateMock.Object
            );
            await handler.Handle(
                new IncrementPlayerScore(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            gameStateMock.Verify(
                mock => mock.IncrementPlayer(
                    playerEntityId
                )
            );
        }

        [Fact]
        public async Task ShouldPublishClientActionWheRequestIsHandled()
        {
            // Given
            var playerEntityId = 123L;
            var currentGameState = new CurrentGameState(
                new List<GamePlayerScore>()
            );
            var expected = new GameStateChangedData
            {
                GameState = currentGameState,
            };

            var mediatorMock = new Mock<IMediator>();
            var gameStateMock = new Mock<GameState>();

            gameStateMock.Setup(
                mock => mock.CurrentGameState
            ).Returns(
                currentGameState
            );

            // When
            var handler = new IncrementPlayerScoreHandler(
                mediatorMock.Object,
                gameStateMock.Object
            );
            await handler.Handle(
                new IncrementPlayerScore(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.Is<ClientActionGenericToAllEvent>(
                        evt => evt.Data.Equals(
                            expected
                        )
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
