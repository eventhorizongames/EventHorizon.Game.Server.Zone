namespace EventHorizon.Server.Core.Connection.Tests.Internal
{
    using System;
    using System.Threading.Tasks;

    using EventHorizon.Server.Core.Connection;
    using EventHorizon.Server.Core.Connection.Internal;
    using EventHorizon.Server.Core.Connection.Model;

    using MediatR;

    using Microsoft.AspNetCore.Http.Connections.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class SystemCoreServerConnectionFactoryTests
    {
        [Fact]
        public async Task ShouldGetConnectionFromCacheWithSpecifiedServerAddress()
        {
            // Given
            var server = "core_server_url";
            var expected = $"{server}/zoneCore";
            var coreSettings = new CoreSettings
            {
                Server = server
            };

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
                connectionCacheMock.Object,
                new Mock<IServiceScopeFactory>().Object
            );

            var actual = await factory.GetConnection();

            // Then
            Assert.NotNull(
                actual
            );
            connectionCacheMock.Verify(
                mock => mock.GetConnection(
                    expected,
                    It.IsAny<Action<HttpConnectionOptions>>(),
                    It.IsAny<Func<Exception, Task>>()
                )
            );
        }
    }
}
