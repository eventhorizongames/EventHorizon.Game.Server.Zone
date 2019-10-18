using MediatR;
using Moq;
using Xunit;
using EventHorizon.Zone.System.Player.Events.Connected;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EventHorizon.Zone.System.Player.ExternalHub.Tests
{
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