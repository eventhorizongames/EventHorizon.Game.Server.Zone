using System;
using System.Threading.Tasks;
using EventHorizon.Server.Core.External.Connection;
using EventHorizon.Server.Core.External.Connection.Internal;
using EventHorizon.Server.Core.External.Model;
using MediatR;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EventHorizon.Server.Core.External.Tests.Connection.Internal
{
    public class SystemCoreServerConnectionFactoryTests
    {
        [Fact]
        public async Task TestShouldDoSomething()
        {
            // Given
            var server = "core_server_url";
            var coreSettings = new CoreSettings();
            coreSettings.Server = server;

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            var mediatorMock = new Mock<IMediator>();
            var optionsMock = new Mock<IOptions<CoreSettings>>();
            var connectionCacheMock = new Mock<CoreServerConnectionCache>();

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
                coreSettings
            );

            // When
            var factory = new SystemCoreServerConnectionFactory(
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
                    $"{server}/zoneCore",
                    It.IsAny<Action<HttpConnectionOptions>>()
                )
            );
        }
    }
}