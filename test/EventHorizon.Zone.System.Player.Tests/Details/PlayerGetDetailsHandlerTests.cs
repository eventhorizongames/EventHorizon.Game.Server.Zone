using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.System.Player.Details;
using EventHorizon.Zone.System.Player.Events.Details;
using EventHorizon.Zone.System.Player.Model.Details;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Player.Tests.Details
{
    public class PlayerGetDetailsHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnPlayerDetailsWhenSendActionIsCalledWithGetPlayer()
        {
            // Given
            var playerId = "player-id";
            var expected = new PlayerDetails
            {
                Id = playerId
            };

            var playerServerConnectionMock = new Mock<PlayerServerConnection>();
            var playerServerConnectionFactoryMock = new Mock<PlayerServerConnectionFactory>();

            playerServerConnectionFactoryMock.Setup(
                mock => mock.GetConnection()
            ).ReturnsAsync(
                playerServerConnectionMock.Object
            );

            playerServerConnectionMock.Setup(
                mock => mock.SendAction<PlayerDetails>(
                    "GetPlayer",
                    playerId
                )
            ).ReturnsAsync(
                expected
            );

            // When
            var handler = new PlayerGetDetailsHandler(
                playerServerConnectionFactoryMock.Object
            );
            var actual = await handler.Handle(
                new PlayerGetDetailsEvent(
                    playerId
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}