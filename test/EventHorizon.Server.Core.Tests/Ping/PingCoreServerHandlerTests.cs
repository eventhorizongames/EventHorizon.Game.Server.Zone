namespace EventHorizon.Server.Core.Tests.Ping
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection;
    using EventHorizon.Server.Core.Events.Check;
    using EventHorizon.Server.Core.Events.Register;
    using EventHorizon.Server.Core.Ping;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class PingCoreServerHandlerTests
    {
        [Fact]
        public async Task ShouldDoNothingWhenNotRegisteredWithCoreServer()
        {
            // Given

            var loggerMock = new Mock<ILogger<PingCoreServerHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var connectionFactoryMock = new Mock<CoreServerConnectionFactory>();

            var coreServerConnectionMock = new Mock<CoreServerConnection>();

            connectionFactoryMock.Setup(
                mock => mock.GetConnection()
            ).ReturnsAsync(
                coreServerConnectionMock.Object
            );

            // When
            var handler = new PingCoreServerHandler(
                loggerMock.Object,
                mediatorMock.Object,
                connectionFactoryMock.Object
            );
            await handler.Handle(
                new Events.Ping.PingCoreServer(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new CheckCoreServerConnection(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldPingConnectionFactoryConnectionWhenConnectedToCoreServer()
        {
            // Given

            var coreServerConnectionApiMock = new Mock<CoreServerConnectionApi>();

            var loggerMock = new Mock<ILogger<PingCoreServerHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var connectionFactoryMock = new Mock<CoreServerConnectionFactory>();

            var coreServerConnectionMock = new Mock<CoreServerConnection>();

            connectionFactoryMock.Setup(
                mock => mock.GetConnection()
            ).ReturnsAsync(
                coreServerConnectionMock.Object
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForRegistrationWithCoreServer(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            coreServerConnectionMock.Setup(
                mock => mock.Api
            ).Returns(
                coreServerConnectionApiMock.Object
            );

            // When
            var handler = new PingCoreServerHandler(
                loggerMock.Object,
                mediatorMock.Object,
                connectionFactoryMock.Object
            );
            await handler.Handle(
                new Events.Ping.PingCoreServer(),
                CancellationToken.None
            );

            // Then
            coreServerConnectionApiMock.Verify(
                mock => mock.Ping()
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    new CheckCoreServerConnection(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldPublishCheckCoreServerConnectionEventWhenAnyExceptionIsThrown()
        {
            // Given

            var coreServerConnectionApiMock = new Mock<CoreServerConnectionApi>();

            var loggerMock = new Mock<ILogger<PingCoreServerHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var connectionFactoryMock = new Mock<CoreServerConnectionFactory>();

            var coreServerConnectionMock = new Mock<CoreServerConnection>();

            connectionFactoryMock.Setup(
                mock => mock.GetConnection()
            ).ReturnsAsync(
                coreServerConnectionMock.Object
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForRegistrationWithCoreServer(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            coreServerConnectionMock.Setup(
                mock => mock.Api
            ).Returns(
                coreServerConnectionApiMock.Object
            );

            coreServerConnectionApiMock.Setup(
                mock => mock.Ping()
            ).Throws(
                new Exception("error")
            );

            // When
            var handler = new PingCoreServerHandler(
                loggerMock.Object,
                mediatorMock.Object,
                connectionFactoryMock.Object
            );
            await handler.Handle(
                new Events.Ping.PingCoreServer(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new CheckCoreServerConnection(),
                    CancellationToken.None
                )
            );
        }
    }
}
