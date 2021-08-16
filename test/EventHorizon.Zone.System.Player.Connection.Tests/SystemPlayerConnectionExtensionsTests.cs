
using System.Collections.Generic;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.System.Player.Connection.Internal;
using EventHorizon.Zone.System.Player.Connection.Model;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

namespace EventHorizon.Server.Core.Tests
{
    public class SystemPlayerConnectionExtensionsTests
    {
        [Fact]
        public void TestShouldConfigureServiceCollectionWithExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            var configurationMock = new Mock<IConfiguration>();
            var config = new ConfigurationSection(
                new ConfigurationRoot(
                    new List<IConfigurationProvider>()
                ),
                "Player"
            );
            configurationMock.Setup(
                mock => mock.GetSection(
                    "Player"
                )
            ).Returns(
                config
            );

            // When
            SystemPlayerConnectionExtensions.AddSystemPlayerConnection(
                serviceCollectionMock,
                configurationMock.Object
            );

            // Then
            Assert.NotEmpty(
                serviceCollectionMock
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service => typeof(PlayerServerConnectionCache) == service.Value.ServiceType
                        && typeof(SystemPlayerServerConnectionCache) == service.Value.ImplementationType
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service => typeof(PlayerServerConnectionFactory) == service.Value.ServiceType
                        && typeof(SystemPlayerServerConnectionFactory) == service.Value.ImplementationType
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service => typeof(IConfigureOptions<PlayerServerConnectionSettings>) == service.Value.ServiceType
            );
        }
    }
}
