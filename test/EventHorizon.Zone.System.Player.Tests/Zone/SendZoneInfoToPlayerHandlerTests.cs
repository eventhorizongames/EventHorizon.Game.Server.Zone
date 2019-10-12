using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.System.Player.Events.Info;
using EventHorizon.Zone.System.Player.Events.Update;
using EventHorizon.Zone.System.Player.Events.Zone;
using EventHorizon.Zone.System.Player.ExternalHub;
using EventHorizon.Zone.System.Player.Model.Details;
using EventHorizon.Zone.System.Player.Update;
using EventHorizon.Zone.System.Player.Zone;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Player.Tests.Zone
{
    public class SendZoneInfoToPlayerHandlerTests
    {
        [Fact]
        public async Task TestShouldSendAsyncRequestToZoneInfoWhenEventIsHandled()
        {
            // Given
            var connectionId = "connection-id";
            var player = new PlayerEntity
            {
                ConnectionId = connectionId
            };
            var expected = "ZoneInfo";
            var expectedZoneInfoMap = new Dictionary<string, object>();

            var mediatorMock = new Mock<IMediator>();
            var playerHubContextMock = new Mock<IHubContext<PlayerHub>>();
            var clientsContextMock = new Mock<IHubClients>();
            var clientProxyMock = new Mock<IClientProxy>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForPlayerZoneInfo(
                        player
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedZoneInfoMap
            );

            playerHubContextMock.Setup(
                mock => mock.Clients
            ).Returns(
                clientsContextMock.Object
            );

            clientsContextMock.Setup(
                mock => mock.Client(
                    connectionId
                )
            ).Returns(
                clientProxyMock.Object
            );

            // When
            var handler = new SendZoneInfoToPlayerHandler(
                mediatorMock.Object,
                playerHubContextMock.Object
            );
            await handler.Handle(
                new SendZoneInfoToPlayerEvent(
                    player
                ),
                CancellationToken.None
            );

            // Then
            clientProxyMock.Verify(
                mock => mock.SendCoreAsync(
                    expected,
                    It.Is<object[]>(
                        match =>
                            match.Length == 1
                            &&
                            match[0] == expectedZoneInfoMap
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}