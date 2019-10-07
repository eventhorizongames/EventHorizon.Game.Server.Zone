
using EventHorizon.Server.Core.External.Connection;
using EventHorizon.Server.Core.External.Connection.Internal;
using EventHorizon.Tests.TestUtils;
using Xunit;

namespace EventHorizon.Server.Core.Tests
{
    public class ServerCoreExternalExtensionsTests
    {
        [Fact]
        public void TestShouldConfigureServiceCollectionWithExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            ServerCoreExternalExtensions.AddServerCoreExternal(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(CoreServerConnectionCache), service.ServiceType);
                    Assert.Equal(typeof(SystemCoreServerConnectionCache), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(CoreServerConnectionFactory), service.ServiceType);
                    Assert.Equal(typeof(SystemCoreServerConnectionFactory), service.ImplementationType);
                }
            );
        }
    }
}