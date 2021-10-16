namespace EventHorizon.Zone.System.Admin.ExternalHub.Tests
{

    using EventHorizon.Game.Server.Zone.Info.Query;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Server.Scripts.Model.Query;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.Security.Claims;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.AspNetCore.SignalR;

    using Moq;

    using Xunit;

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
            )
            {
                Context = contextMock.Object
            };
            await adminHub.OnConnectedAsync();

            //Then
            Assert.True(
                true
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
            )
            {
                Context = contextMock.Object
            };
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
            var hubCallerContextMock = new Mock<HubCallerContext>();

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
            )
            {
                Context = hubCallerContextMock.Object
            };
            var actual = await adminHub.ZoneInfo();

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task TestShouldReturnServerScriptsErrorDetailsWhenServerScripts_ErrorDetailsCalled()
        {
            // Given
            var expected = new CommandResult<ServerScriptsErrorDetailsResponse>(
                true,
                new ServerScriptsErrorDetailsResponse(
                    false,
                    string.Empty,
                    new List<GeneratedServerScriptErrorDetailsModel>()
                )
            );

            var mediatorMock = new Mock<IMediator>();
            var hubCallerContextMock = new Mock<HubCallerContext>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForServerScriptsErrorDetails(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expected
            );

            // When
            var adminHub = new AdminHub(
                mediatorMock.Object
            )
            {
                Context = hubCallerContextMock.Object
            };
            var actual = await adminHub.ServerScripts_ErrorDetails();

            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
