namespace EventHorizon.Game.Tests.Clear
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Clear;
    using EventHorizon.Game.Model.Client;
    using EventHorizon.Game.State;
    using EventHorizon.Zone.Core.Events.Client.Generic;

    using MediatR;

    using Moq;

    using Xunit;

    public class ClearPlayerScoreHandlerTests
    {
        [Fact]
        public async Task ShouldRemovePlayerByEntityIdWhenRequestIsHandled()
        {
            // Given
            var entityId = 123L;

            var mediatorMock = new Mock<IMediator>();
            var gameStateMock = new Mock<GameState>();

            // When
            var handler = new ClearPlayerScoreHandler(
                mediatorMock.Object,
                gameStateMock.Object
            );
            await handler.Handle(
                new ClearPlayerScore(
                    entityId
                ),
                CancellationToken.None
            );

            // Then
            gameStateMock.Verify(
                mock => mock.RemovePlayer(
                    entityId
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
            var handler = new ClearPlayerScoreHandler(
                mediatorMock.Object,
                gameStateMock.Object
            );
            await handler.Handle(
                new ClearPlayerScore(
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
