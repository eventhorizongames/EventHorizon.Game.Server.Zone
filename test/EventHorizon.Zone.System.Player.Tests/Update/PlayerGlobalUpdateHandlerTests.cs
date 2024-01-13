namespace EventHorizon.Zone.System.Player.Tests.Update;

using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.System.Player.Events.Update;
using EventHorizon.Zone.System.Player.Model.Details;
using EventHorizon.Zone.System.Player.Update;

using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class PlayerGlobalUpdateHandlerTests
{
    [Fact]
    public async Task ShouldSendActionToPlayerConnectionWhenEventIsHandled()
    {
        // Given
        var playerId = "player-id";
        var player = new PlayerEntity
        {
            PlayerId = playerId
        };
        var expected = "UpdatePlayer";

        var playerServerConnectionMock = new Mock<PlayerServerConnection>();
        var playerServerConnectionFactoryMock = new Mock<PlayerServerConnectionFactory>();

        playerServerConnectionFactoryMock.Setup(
            mock => mock.GetConnection()
        ).ReturnsAsync(
            playerServerConnectionMock.Object
        );

        // When
        var handler = new PlayerGlobalUpdateHandler(
            playerServerConnectionFactoryMock.Object
        );
        await handler.Handle(
            new PlayerGlobalUpdateEvent(
                player
            ),
            CancellationToken.None
        );

        // Then
        playerServerConnectionMock.Verify(
            mock => mock.SendAction(
                expected,
                It.IsAny<PlayerDetails>()
            )
        );
    }
}
