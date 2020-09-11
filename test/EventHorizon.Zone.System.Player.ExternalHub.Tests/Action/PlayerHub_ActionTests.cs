using MediatR;
using Moq;
using Xunit;
using EventHorizon.Zone.System.Player.Events.Connected;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Player.Plugin.Action.Events;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Player.ExternalHub.Action.Tests
{
    public class PlayerHub_ActionTests
    {
        [Fact]
        public async Task TestShouldPublisUserConnectedEventWhenOnConnectedAsyncIsCalled()
        {
            // Given
            var playerId = "player-id";
            var actionName = "action-name";
            var actionData = new Dictionary<string, object>();
            var expected = new RunPlayerServerAction(
                playerId,
                actionName,
                actionData
            );

            var loggerMock = new Mock<ILogger<PlayerHub>>();
            var mediatorMock = new Mock<IMediator>();
            var contextMock = new Mock<HubCallerContext>();
            var userMock = new Mock<ClaimsPrincipal>();

            contextMock.Setup(
                mock => mock.User
            ).Returns(
                userMock.Object
            );

            userMock.Setup(
                mock => mock.Claims
            ).Returns(
                new List<Claim>
                {
                    new Claim(
                        "sub",
                        playerId
                    )
                }
            );

            // When
            var playerHub = new PlayerHub(
                loggerMock.Object,
                mediatorMock.Object
            );
            playerHub.Context = contextMock.Object;

            await playerHub.PlayerAction(
                actionName,
                actionData
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldNotPublishUserConnectedEventWhenOnConnectedAsyncIsCalledWithBadUserClaim()
        {
            // Given
            var connectionId = "connection-id";

            var loggerMock = new Mock<ILogger<PlayerHub>>();
            var mediatorMock = new Mock<IMediator>();
            var contextMock = new Mock<HubCallerContext>();
            var userMock = new Mock<ClaimsPrincipal>();

            contextMock.Setup(
                mock => mock.User
            ).Returns(
                userMock.Object
            );

            userMock.Setup(
                mock => mock.Claims
            ).Returns(
                new List<Claim>()
            );
            contextMock.Setup(
                mock => mock.ConnectionId
            ).Returns(
                connectionId
            );

            // When
            var playerHub = new PlayerHub(
                loggerMock.Object,
                mediatorMock.Object
            );
            playerHub.Context = contextMock.Object;

            await playerHub.OnConnectedAsync();

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<PlayerConnectedEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}