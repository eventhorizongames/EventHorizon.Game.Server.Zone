using Xunit;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Game.Server.Zone;

namespace EventHorizon.Zone.System.Admin.Tests
{
    public class SystemAdminExtensionsTests
    {
        [Fact]
        public void TestShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemAdminExtensions.AddSystemAdmin(
                serviceCollectionMock
            );

            // Then
            Assert.Empty(
                serviceCollectionMock
            );
        }

        [Fact]
        public void TestShouldConfigurationApplicationBuilder()
        {
            // Given
            var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = mocks.ApplicationBuilderMock.Object;

            // When
            var actual = SystemAdminExtensions.UseSystemAdmin(
                mocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}