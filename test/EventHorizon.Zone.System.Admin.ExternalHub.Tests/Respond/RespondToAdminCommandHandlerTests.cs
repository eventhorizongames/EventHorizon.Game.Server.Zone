namespace EventHorizon.Zone.System.Admin.ExternalHub.Tests.Respond
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Admin.ExternalHub.Respond;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model;

    using Microsoft.AspNetCore.SignalR;

    using Moq;

    using Xunit;

    public class RespondToAdminCommandHandlerTests
    {
        [Fact]
        public async Task TestShouldSendAdminCommandResponseToClientOfConnectionIdWhenValidConnectionId()
        {
            // Given
            var expected = true;
            var expectedCommand = "AdminCommandResponse";
            var expectedResponse = default(IAdminCommandResponse);
            var connectionId = "connection-id";


            var hubContextMock = new Mock<IHubContext<AdminHub>>();
            var hubClientsMock = new Mock<IHubClients>();
            var hubClientMock = new Mock<IClientProxy>();

            hubContextMock.Setup(
                mock => mock.Clients
            ).Returns(
                hubClientsMock.Object
            );
            hubClientsMock.Setup(
                mock => mock.Client(
                    connectionId
                )
            ).Returns(
                hubClientMock.Object
            );

            // When
            var handler = new RespondToAdminCommandHandler(
                hubContextMock.Object
            );
            var actual = await handler.Handle(
                new RespondToAdminCommand(
                    connectionId,
                    expectedResponse
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            hubClientMock.Verify(
                mock => mock.SendCoreAsync(
                    expectedCommand,
                    It.Is<object[]>(
                        match =>
                            match.Length == 1
                            &&
                            match[0] == expectedResponse
                    ),
                    CancellationToken.None
                )
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task TestShoudlReturnFalseWhenConnectionIdIsEmpty(
            string connectionId
        )
        {
            // Given
            var expected = false;

            var hubContextMock = new Mock<IHubContext<AdminHub>>();

            // When
            var handler = new RespondToAdminCommandHandler(
                hubContextMock.Object
            );
            var actual = await handler.Handle(
                new RespondToAdminCommand(
                    connectionId,
                    default
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            hubContextMock.Verify(
                mock => mock.Clients,
                Times.Never()
            );
        }
    }
}
