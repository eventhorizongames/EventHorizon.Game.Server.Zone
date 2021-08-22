namespace EventHorizon.Zone.System.Player.ExternalHub.Tests
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Security.Claims;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Player.Events.Connected;

    using MediatR;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class PlayerHubTests
    {
        [Fact]
        public async Task TestShouldPublisUserConnectedEventWhenOnConnectedAsyncIsCalled()
        {
            // Given
            var playerId = "player-id";
            var connectionId = "connection-id";
            var expected = new PlayerConnectedEvent(
                playerId,
                connectionId
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
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldPublisUserDisconnectedEventWhenOnDisconnectedAsyncIsCalled()
        {
            // Given
            var playerId = "player-id";
            var expected = new PlayerDisconnectedEvent(
                playerId
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

            await playerHub.OnDisconnectedAsync(
                new Exception(
                    "error"
                )
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

        [Fact]
        public async Task TestShouldNotPublishUserDisconnectedEventWhenOnDisconnectedAsyncIsCalledWithBadUserClaim()
        {
            // Given

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

            // When
            var playerHub = new PlayerHub(
                loggerMock.Object,
                mediatorMock.Object
            );
            playerHub.Context = contextMock.Object;

            await playerHub.OnDisconnectedAsync(
                new Exception(
                    "error"
                )
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<PlayerDisconnectedEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}
