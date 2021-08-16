namespace EventHorizon.Zone.System.Client.Scripts.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;

    using FluentAssertions;

    using Xunit;

    public class SystemClientScriptsPluginSharedExtensionsTests
    {
        [Fact]
        public void ShouldRegisterExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemClientScriptsPluginSharedExtensions.AddSystemClientScriptsPluginShared(
                serviceCollectionMock
            );

            // Then
            serviceCollectionMock
                .Should().BeEmpty();
        }

        [Fact]
        public void ShouldReturnPassedInApplicationBuilderWhenFinishedRunning()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            // When
            var actual = SystemClientScriptsPluginSharedExtensions.UseSystemClientScriptsPluginShared(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            actual
                .Should().Be(expected);
        }
    }
}
