using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Info.Query;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Admin.ExternalHub.Tests
{
    public class AdminHubTests
    {
        [Fact]
        public async Task TestShouldSuccessfullyConnectWhenRoleIsAdmin()
        {
            //Given
            var role = "Admin";

            var mediatorMock = new Mock<IMediator>();
            var contextMock = new Mock<HubCallerContext>();
            var userMock = new Mock<ClaimsPrincipal>();

            contextMock.Setup(
                mock => mock.User
            ).Returns(
                userMock.Object
            );

            userMock.Setup(
                mock => mock.IsInRole(
                    role
                )
            ).Returns(
                true
            );

            //When
            var adminHub = new AdminHub(
                mediatorMock.Object
            );
            adminHub.Context = contextMock.Object;
            await adminHub.OnConnectedAsync();

            //Then
            Assert.True(
                true
            );
        }

        [Fact]
        public async Task TestShouldThrowNoRoleExceptionWhenWhenRoleIsNotAdmin()
        {
            //Given
            var role = "Admin";
            var expected = "no_role";

            var mediatorMock = new Mock<IMediator>();
            var contextMock = new Mock<HubCallerContext>();
            var userMock = new Mock<ClaimsPrincipal>();

            contextMock.Setup(
                mock => mock.User
            ).Returns(
                userMock.Object
            );

            userMock.Setup(
                mock => mock.IsInRole(
                    role
                )
            ).Returns(
                false
            );

            //When
            var adminHub = new AdminHub(
                mediatorMock.Object
            );
            adminHub.Context = contextMock.Object;
            Func<Task> action = async () => await adminHub.OnConnectedAsync();

            var actual = await Assert.ThrowsAsync<Exception>(
                action
            );

            //Then
            Assert.Equal(
                expected,
                actual.Message
            );
        }

        [Fact]
        public async Task TestShouldPublishAdminCommandEventWhenCommandCalled()
        {
            // Given
            var expectedConnectionId = "connection-id";
            var expectedData = new { };

            var command = "random-command";

            var mediatorMock = new Mock<IMediator>();
            var contextMock = new Mock<HubCallerContext>();

            contextMock.Setup(
                mock => mock.ConnectionId
            ).Returns(
                expectedConnectionId
            );

            // When
            var adminHub = new AdminHub(
                mediatorMock.Object
            );
            adminHub.Context = contextMock.Object;
            await adminHub.Command(
                command,
                expectedData
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.Is<AdminCommandEvent>(
                        commandEvent =>
                            commandEvent.ConnectionId == expectedConnectionId
                            && commandEvent.Data == expectedData
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldReturnZoneInfoWhenZoneInfoCalled()
        {
            // Given
            var expected = new Dictionary<string, object>();

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForFullZoneInfo(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expected
            );

            // When
            var adminHub = new AdminHub(
                mediatorMock.Object
            );
            var actual = await adminHub.ZoneInfo();

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}