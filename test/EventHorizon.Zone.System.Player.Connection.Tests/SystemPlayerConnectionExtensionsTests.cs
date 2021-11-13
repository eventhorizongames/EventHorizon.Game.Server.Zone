namespace EventHorizon.Server.Core.Tests
{

    using System.Collections.Generic;

    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Player.Connection;
    using EventHorizon.Zone.System.Player.Connection.Internal;
    using EventHorizon.Zone.System.Player.Connection.Model;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

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
                service => typeof(PlayerServerConnectionCache) == service.ServiceType
                        && typeof(SystemPlayerServerConnectionCache) == service.ImplementationType
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service => typeof(PlayerServerConnectionFactory) == service.ServiceType
                        && typeof(SystemPlayerServerConnectionFactory) == service.ImplementationType
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service => typeof(IConfigureOptions<PlayerServerConnectionSettings>) == service.ServiceType
            );
        }
    }
}
