namespace EventHorizon.Server.Core.Tests.Stop
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection;
    using EventHorizon.Server.Core.Events.Stop;
    using EventHorizon.Server.Core.Stop;
    using Moq;
    using Xunit;

    public class StopCoreServerConnectionHandlerTests
    {
        [Fact]
        public async Task ShouldCallStopOnConnectionWhenRequestIsHandled()
        {
            // Given

            var coreServerConnectionCacheMock = new Mock<CoreServerConnectionCache>();

            // When
            var handler = new StopCoreServerConnectionHandler(
                coreServerConnectionCacheMock.Object
            );
            await handler.Handle(
                new StopCoreServerConnection(),
                CancellationToken.None
            );

            // Then
            coreServerConnectionCacheMock.Verify(
                mock => mock.Stop()
            );
        }
    }
}