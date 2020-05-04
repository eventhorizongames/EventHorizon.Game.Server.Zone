namespace EventHorizon.Server.Core.Tests.Disconnected
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection.Disconnected;
    using EventHorizon.Server.Core.Disconnected;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using Moq;
    using Xunit;

    public class CoreServerConnectionDisconnectedHandlerTests
    {
        [Fact]
        public async Task ShouldNullServerIdWhenRequestIsHandled()
        {
            // Given

            var serverPropertyMock = new Mock<IServerProperty>();

            // When
            var handler = new CoreServerConnectionDisconnectedHandler(
                serverPropertyMock.Object
            );
            await handler.Handle(
                new ServerCoreConnectionDisconnected(),
                CancellationToken.None
            );

            // Then
            serverPropertyMock.Verify(
                mock => mock.Set(
                    ServerPropertyKeys.SERVER_ID,
                    null
                )
            );
        }
    }
}