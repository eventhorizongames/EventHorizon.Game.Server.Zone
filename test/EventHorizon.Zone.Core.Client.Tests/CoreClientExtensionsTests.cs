using Xunit;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Game.Server.Zone;

namespace EventHorizon.Zone.Core.Client.Tests
{
    public class CoreClientExtensionsTests
    {
        [Fact]
        public void TestShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            CoreClientExtensions.AddCoreClient(
                serviceCollectionMock
            );

            // Then
            Assert.Empty(
                serviceCollectionMock
            );
        }
    }
}