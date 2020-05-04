namespace EventHorizon.Game.Server.Zone.Tests.Setup
{
    using EventHorizon.Game.Server.Zone.Setup;
    using EventHorizon.Game.Server.Zone.Tests.TestUtil;
    using EventHorizon.Test.Common;
    using Xunit;

    public class ServerSetupExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            ServerSetupExtensions.AddServerSetup(
                serviceCollectionMock
            );

            // Then
            Assert.Empty(
                serviceCollectionMock
            );
        }

        [Fact]
        public void TestUseSetupServer_ShouldSendAndPublishExpectedEvent()
        {
            // Given
            var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = mocks.ApplicationBuilderMock.Object;

            // When
            var actual = ServerSetupExtensions.UseServerSetup(
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