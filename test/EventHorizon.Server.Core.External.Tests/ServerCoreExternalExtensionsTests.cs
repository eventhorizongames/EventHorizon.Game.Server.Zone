
using System;
using EventHorizon.Server.Core.External.Connection;
using EventHorizon.Server.Core.External.Connection.Internal;
using EventHorizon.Server.Core.External.Model;
using EventHorizon.Tests.TestUtils;
using Microsoft.Extensions.Options;
using Xunit;

namespace EventHorizon.Server.Core.Tests
{
    public class ServerCoreExternalExtensionsTests
    {
        [Fact]
        public void TestShouldConfigureServiceCollectionWithExpectedServices()
        {
            // Given
            Action<CoreSettings> configureCoreSettings = options => { };

            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            ServerCoreExternalExtensions.AddServerCoreExternal(
                serviceCollectionMock,
                configureCoreSettings
            );

            // Then
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(CoreServerConnectionCache)
                    &&
                    service.ImplementationType == typeof(SystemCoreServerConnectionCache)
            );
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(CoreServerConnectionFactory)
                    &&
                    service.ImplementationType == typeof(SystemCoreServerConnectionFactory)
            );
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(IConfigureOptions<CoreSettings>)
            );
        }
    }
}