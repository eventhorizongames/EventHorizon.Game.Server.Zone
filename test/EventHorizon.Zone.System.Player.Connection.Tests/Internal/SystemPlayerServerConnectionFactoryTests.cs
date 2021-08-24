namespace EventHorizon.Zone.System.Player.Connection.Tests.Internal
{
    using global::System;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Player.Connection.Internal;
    using EventHorizon.Zone.System.Player.Connection.Model;

    using MediatR;

    using Microsoft.AspNetCore.Http.Connections.Client;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class SystemPlayerServerConnectionFactoryTests
    {
        [Fact]
        public async Task TestShouldDoSomething()
        {
            // Given
            var server = "server_server_url";
            var connectionSettings = new PlayerServerConnectionSettings
            {
                Server = server
            };

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            var mediatorMock = new Mock<IMediator>();
            var optionsMock = new Mock<IOptions<PlayerServerConnectionSettings>>();
            var connectionCacheMock = new Mock<PlayerServerConnectionCache>();

            var loggerMock = new Mock<ILogger>();
            loggerFactoryMock.Setup(
                mock => mock.CreateLogger(
                    It.IsAny<string>()
                )
            ).Returns(
                loggerMock.Object
            );

            optionsMock.Setup(
                mock => mock.Value
            ).Returns(
                connectionSettings
            );

            // When
            var factory = new SystemPlayerServerConnectionFactory(
                loggerFactoryMock.Object,
                mediatorMock.Object,
                optionsMock.Object,
                connectionCacheMock.Object
            );

            var actual = await factory.GetConnection();

            // Then
            Assert.NotNull(
                actual
            );
            connectionCacheMock.Verify(
                mock => mock.GetConnection(
                    $"{server}/playerBus",
                    It.IsAny<Action<HttpConnectionOptions>>()
                )
            );
        }
    }
}
